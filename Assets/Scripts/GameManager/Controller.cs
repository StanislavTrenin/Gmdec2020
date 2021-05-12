using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
    private SortedDictionary<int, List<Character>> charactersQueue2 = new SortedDictionary<int, List<Character>>();

    private SortedDictionary<int, List<Character>> currentCharactersQueue;

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

        currentCharactersQueue = charactersQueue;
        if (GameManager.currentBuff != GameManager.Buff.NO)
        {
            List<Character> charactersToEffect = new List<Character>();
            foreach (var characters in charactersQueue)
            {
                foreach (var character in characters.Value)
                {
                    switch (GameManager.currentBuff)
                    {
                        case GameManager.Buff.WIN:
                        {
                            if (!character.isPlayer) charactersToEffect.Add(character);
                            break;
                        }
                        case GameManager.Buff.FAIL:
                        {
                            if (character.isPlayer) charactersToEffect.Add(character);
                            break;
                        }
                    }
                }
            }

            switch (GameManager.currentBuff)
            {
                case GameManager.Buff.WIN:
                {
                    if (Random.value > 0.5)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            int index = Random.Range(0, charactersToEffect.Count);
                            Character character = charactersToEffect[index];
                            charactersToEffect.Remove(character);
                            character.Kill();
                        }
                    }
                    else
                    {
                        for (int i = 0; i < charactersToEffect.Count / 2; i++)
                        {
                            int index = Random.Range(0, charactersToEffect.Count);
                            while (charactersToEffect[index].stunnedSteps > 0)
                            {
                                index = Random.Range(0, charactersToEffect.Count);
                            }

                            charactersToEffect[index].stunnedSteps = 2;
                        }
                    }
                    break;
                }
                case GameManager.Buff.FAIL:
                {
                    foreach (var character in charactersToEffect)
                    {
                        if (character.clazz == CharacterClass.TANK ||
                             character.clazz == CharacterClass.MELEE_FIGHTER)
                        {
                            character.stats.currentHealth = Mathf.RoundToInt(character.stats.currentHealth * 0.8f);
                        }
                    }
                    break;
                }
            }
        }

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
        if (currentCharactersQueue.Count == 0)
        {
            if (currentCharactersQueue == charactersQueue)
            {
                currentCharactersQueue = charactersQueue2;
            }
            else
            {
                currentCharactersQueue = charactersQueue;
            }
        }
        List<Character> characters = currentCharactersQueue.First().Value;
        Character currentCharacter = characters[Random.Range(0, characters.Count)];
        characters.Remove(currentCharacter);
        if (characters.Count == 0)
        {
            currentCharactersQueue.Remove(currentCharactersQueue.First().Key);
        }
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
        var nextQueue = currentCharactersQueue == charactersQueue ? charactersQueue2 : charactersQueue;
        character.stunnedSteps = Mathf.Max(character.stunnedSteps - 1, 0);
        if (!nextQueue.ContainsKey(character.stats.initiative))
        {
            nextQueue[character.stats.initiative] = new List<Character>();
        }
        nextQueue[character.stats.initiative].Add(character);
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
                if(field.type == FieldType.FLOR) field.gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(0.8f, 0.5f, 0.5f, 1);
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
        character.isAi = spawnPoint.isAi;
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
        if (charactersQueue.ContainsKey(character.stats.initiative) &&
            charactersQueue[character.stats.initiative].Contains(character))
        {
            charactersQueue[character.stats.initiative].Remove(character);
        }
        else
        {
            charactersQueue2[character.stats.initiative].Remove(character);
        }
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
