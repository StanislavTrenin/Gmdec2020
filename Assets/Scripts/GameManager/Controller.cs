using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class Controller : MonoBehaviour
{
    [SerializeField] private GameObject characterInstance;
    [SerializeField] private GameObject fieldInstance;
    [SerializeField] private PathGeneratorVisual pathGeneratorVisual;

    private FieldData fieldData;
    private Queue<Character> playerCharactersQueue = new Queue<Character>();
    private Queue<Character> enemyCharactersQueue = new Queue<Character>();
    private bool isPlayerStep;

    private void Start()
    {
        fieldData = new FieldData
        {
            FieldSize = fieldInstance.transform.localScale
        };
        
        GenerateField();
        GenerateCharacters();
        SetCamera();
        isPlayerStep = true;
        fieldData.ActiveCharacter = playerCharactersQueue.Dequeue();
    }

    private void GenerateField()
    {
        FieldType[,] fieldTypes = MapDeserializer.Deserialize("demo_map.txt");
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
                field.Notify += new FieldSelector(fieldData, pathGeneratorVisual).OnSelectField;
                fieldData.Fields[i, j] = field;
            }
        }
    }

    private void GenerateCharacters()
    {
        for (int i = 0; i < 10; i++)
        {
            GenerateCharacter(true, i + 1);
            GenerateCharacter(false, i + 1);
        }
    }

    private void GenerateCharacter(bool isPlayer, int initiative)
    {
        int rowsCount = fieldData.Fields.GetUpperBound(0) + 1;
        int columnsCount = fieldData.Fields.GetUpperBound(1) + 1;
        int randX, randY;
        
        do
        {
            randX = Random.Range(0, columnsCount);
            randY = Random.Range(0, rowsCount);
        } while (fieldData.Fields[randX, randY].type != FieldType.FLOR);
        
        GameObject characterObject = Instantiate(characterInstance, fieldData.Fields[randX, randY].gameObject.transform);
        Character character = characterObject.GetComponent<Character>();
        character.field = fieldData.Fields[randX, randY];
        character.isPlayer = isPlayer;
        character.initiative = initiative;
        
        if (isPlayer)
        {
            playerCharactersQueue.Enqueue(character);
        }
        else
        {
            enemyCharactersQueue.Enqueue(character);
        }
    }

    public void EndOfTurn()
    {
        pathGeneratorVisual.ResetLinePath();
        
        if (isPlayerStep)
        {
            playerCharactersQueue.Enqueue(fieldData.ActiveCharacter);
            isPlayerStep = false;
            fieldData.ActiveCharacter = enemyCharactersQueue.Dequeue();
        }
        else
        {
            enemyCharactersQueue.Enqueue(fieldData.ActiveCharacter);
            isPlayerStep = true;
            fieldData.ActiveCharacter = playerCharactersQueue.Dequeue();
        }
    }
    
    private void SetCamera()
    {
        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        cameraController.SetSize((fieldData.Fields.GetUpperBound(1) + 1) * fieldData.FieldSize.x,
            (fieldData.Fields.GetUpperBound(0) + 1) * fieldData.FieldSize.y);
    }
}
