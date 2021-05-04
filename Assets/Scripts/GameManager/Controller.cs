using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Controller : MonoBehaviour
{
    [Serializable]
    struct CharacterInstancePair
    {
        public CharacterClass characterClass;
        public GameObject gameObject;
    }
    
    [SerializeField] private List<CharacterInstancePair> characterInstances;
    private Dictionary<CharacterClass, GameObject> characterInstancesDict = new Dictionary<CharacterClass, GameObject>();
    [SerializeField] private GameObject fieldInstance;
    [SerializeField] private PathGeneratorVisual pathGeneratorVisual;
    [SerializeField] private GameObject skillButtonInstance;
    [SerializeField] private Transform skillButtonsPanel;

    [NonSerialized] public FieldData fieldData;
    private SortedDictionary<int, List<Character>> charactersQueue = new SortedDictionary<int, List<Character>>();

    public Dictionary<bool, int> CountCharacterDict = new Dictionary<bool, int>
    {
        {true, 0},
        {false, 0}
    };

    public Action onLose;
    public Action onWin;

    private LevelInfo currentLevelInfo;

    private void Start()
    {
        foreach (CharacterInstancePair characterInstancePair in characterInstances)
        {
            characterInstancesDict[characterInstancePair.characterClass] = characterInstancePair.gameObject;
        }
        currentLevelInfo = GameManager.GetCurrentLevelInfo();
        fieldData = new FieldData
        {
            FieldSize = fieldInstance.transform.localScale
        };
        
        GenerateField();
        GenerateCharacters();
        SetCamera();
        fieldData.ActiveCharacter = GetNextActiveCharacter();
        if (!fieldData.ActiveCharacter.isPlayer)
        {
            fieldData.ActiveCharacter.AI(EndOfTurn);
        }
    }

    private Character GetNextActiveCharacter()
    {
        foreach(Transform child in skillButtonsPanel)
        {
            Destroy(child.gameObject);
        }
        fieldData.ActiveSkill = null;
        List<Character> characters = charactersQueue.First().Value;
        Character currentCharacter = characters[Random.Range(0, characters.Count - 1)];
        characters.Remove(currentCharacter);
        if (currentCharacter.isPlayer) {
            if (currentCharacter.stunnedSteps <= 0)
            {
                currentCharacter.UpdateSkillsSteps();
                List<Skill> skills = currentCharacter.GetActiveSkills();
                foreach (Skill skill in skills)
                {
                    GameObject skillButtonGO = Instantiate(skillButtonInstance.gameObject, skillButtonsPanel);
                    SkillButton skillButton = skillButtonGO.GetComponent<SkillButton>();
                    skillButton.skill = skill;
                    skillButton.controller = this;
                    skillButton.UpdateText();
                }
            }
        }
        return currentCharacter;
    }

    private void AddCharacterToQueue(Character character)
    {
        character.stunnedSteps = Mathf.Max(character.stunnedSteps - 1, 0);
        if (!charactersQueue.ContainsKey(character.stats.initiative))
        {
            charactersQueue[character.stats.initiative] = new List<Character>();
        }
        charactersQueue[character.stats.initiative].Add(character);
    }

    private void GenerateField()
    {
        FieldType[,] fieldTypes = MapDeserializer.Deserialize(currentLevelInfo.mapName);
        int rowsCount = fieldTypes.GetUpperBound(0) + 1;
        int columnsCount = fieldTypes.GetUpperBound(1) + 1;
        fieldData.Fields = new Field[columnsCount, rowsCount];
        
        for (int i = 0; i < columnsCount; i++)
        {
            for (int j = 0; j < rowsCount; j++)
            {
                GameObject fieldObject = Instantiate(fieldInstance);
                fieldObject.transform.position = new Vector2(i, rowsCount - j - 1) * fieldData.FieldSize + fieldData.FieldSize * 0.5f;
                Field field = fieldObject.GetComponent<Field>();
                field.type = fieldTypes[i, j];
                field.x = i;
                field.y = j;
                FieldSelector fieldSelector = new FieldSelector(fieldData, pathGeneratorVisual);
                field.Notify += fieldSelector.OnSelectField;
                fieldData.Fields[i, j] = field;
            }
        }
    }

    private void GenerateCharacters()
    {
        foreach (SpawnPoint spawnPoint in currentLevelInfo.spawnPoints)
        {
            GenerateCharacter(spawnPoint);
        }
    }

    private void GenerateCharacter(SpawnPoint spawnPoint)
    {
        GameObject characterObject = Instantiate(characterInstancesDict[spawnPoint.characterClass], fieldData.Fields[spawnPoint.x - 1, spawnPoint.y - 1].gameObject.transform);
        Character character = characterObject.GetComponent<Character>();
        character.level = spawnPoint.level;
        character.field = fieldData.Fields[spawnPoint.x - 1, spawnPoint.y - 1];
        character.isPlayer = spawnPoint.isPlayer;
        character.Destroyed += OnCharacterDestroyed;
        character.Attacked += OnCharacterHit;
        character.controller = this;
        character.UpdateStats();
        CountCharacterDict[spawnPoint.isPlayer]++;
        
        AddCharacterToQueue(character);
    }

    public void EndOfTurn()
    {
        pathGeneratorVisual.ResetLinePath();
        AddCharacterToQueue(fieldData.ActiveCharacter);
        fieldData.ActiveCharacter = GetNextActiveCharacter();
        if (!fieldData.ActiveCharacter.isPlayer)
        {
            fieldData.ActiveCharacter.AI(EndOfTurn);
        }
    }
    
    private void SetCamera()
    {
        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        cameraController.SetSize((fieldData.Fields.GetUpperBound(1) + 1) * fieldData.FieldSize.x,
            (fieldData.Fields.GetUpperBound(0) + 1) * fieldData.FieldSize.y);
    }

    public void OnSkillButton(Skill skill)
    {
        fieldData.ActiveSkill = skill;
    }

    private void OnCharacterDestroyed(Character character)
    {
        charactersQueue[character.stats.initiative].Remove(character);
        character.Destroyed -= OnCharacterDestroyed;
        character.Attacked -= OnCharacterHit;
        CountCharacterDict[character.isPlayer]--;

        if (CountCharacterDict[character.isPlayer] <= 1)
        {
            if (character.isPlayer)
            {
                onLose?.Invoke();
            }
            else
            {
                onWin?.Invoke();
            }
        }
    }

    private void OnCharacterHit()
    {
        foreach(Transform child in skillButtonsPanel)
        {
            Destroy(child.gameObject);
        }
    }
}
