using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Controller : MonoBehaviour
{
    [SerializeField] private GameObject characterInstance;
    [SerializeField] private GameObject fieldInstance;
    [SerializeField] private PathGenerator pathGenerator;

    private Vector2 fieldSize;
    private Field[,] fields;
    private Queue<Character> playerCharactersQueue = new Queue<Character>();
    private Queue<Character> enemyCharactersQueue = new Queue<Character>();
    private Character activeCharacter;
    private bool isPlayerStep;

    private Field prevField;
    
    private void Start()
    {
        fieldSize = fieldInstance.transform.localScale;
        GenerateField();
        GenerateCharacters();
        SetCamera();
        isPlayerStep = true;
        activeCharacter = playerCharactersQueue.Dequeue();
    }

    private void GenerateField()
    {
        FieldType[,] fieldTypes = MapDeserializer.Deserialize("demo_map.txt");
        int rowsCount = fieldTypes.GetUpperBound(0) + 1;
        int columnsCount = fieldTypes.GetUpperBound(1) + 1;
        fields = new Field[columnsCount, rowsCount];
        
        for (int i = 0; i < columnsCount; i++)
        {
            for (int j = 0; j < rowsCount; j++)
            {
                GameObject fieldObject = Instantiate(fieldInstance);
                fieldObject.transform.position = new Vector2(i, rowsCount - j - 1) * fieldSize + fieldSize * 0.5f;
                Field field = fieldObject.GetComponent<Field>();
                field.type = fieldTypes[i, j];
                field.x = i;
                field.y = j;
                field.Notify += OnSelectField;
                fields[i, j] = field;
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
        int rowsCount = fields.GetUpperBound(0) + 1;
        int columnsCount = fields.GetUpperBound(1) + 1;
        int randX, randY;
        
        do
        {
            randX = Random.Range(0, columnsCount);
            randY = Random.Range(0, rowsCount);
        } while (fields[randX, randY].type != FieldType.FLOR);
        
        GameObject characterObject = Instantiate(characterInstance, fields[randX, randY].gameObject.transform);
        Character character = characterObject.GetComponent<Character>();
        character.field = fields[randX, randY];
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
        pathGenerator.ResetLinePath();
        
        if (isPlayerStep)
        {
            playerCharactersQueue.Enqueue(activeCharacter);
            isPlayerStep = false;
            activeCharacter = enemyCharactersQueue.Dequeue();
        }
        else
        {
            enemyCharactersQueue.Enqueue(activeCharacter);
            isPlayerStep = true;
            activeCharacter = playerCharactersQueue.Dequeue();
        }
    }

    private void OnSelectField(int finishX, int finishY)
    {
        if (prevField == fields[finishX, finishY])
        {
            activeCharacter.CharacterAction.EnableMove(activeCharacter, pathGenerator.LinePositions, fields);
            prevField = null;
        }
        else
        {
            pathGenerator.GenerateShortestPath(finishX, finishY, fieldSize, fields, activeCharacter);
        }
        
        prevField = fields[finishX, finishY];
    }

    private void SetCamera()
    {
        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        cameraController.SetSize((fields.GetUpperBound(1) + 1) * fieldSize.x,
            (fields.GetUpperBound(0) + 1) * fieldSize.y);
    }
}
