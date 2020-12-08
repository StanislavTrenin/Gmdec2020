using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class Controller : MonoBehaviour
{
    [SerializeField] private Vector2 fieldSize;
    [SerializeField] private GameObject characterInstance;
    [SerializeField] private GameObject fieldInstance;

    private Field[,] fields;
    private Queue<Character> playerCharactersQueue;
    private Queue<Character> enemyCharactersQueue;
    private bool isPlayerStep;
    private Character activeCharacter;

    // Start is called before the first frame update
    void Start()
    {
        GenerateField();
        GenerateCharacters();
        isPlayerStep = true;
        activeCharacter = playerCharactersQueue.Dequeue();
    }

    private void GenerateField()
    {
        FieldType[,] fieldTypes = MapDeserializer.Deserialize("demo_map.txt");
        int rowsCount = fieldTypes.GetUpperBound(0) + 1;
        int columnsCount = fieldTypes.GetUpperBound(1) + 1;
        for (int i = 0; i < columnsCount; i++)
        {
            for (int j = 0; j < rowsCount; j++)
            {
                GameObject fieldObject = Instantiate(fieldInstance);
                fieldObject.transform.position = new Vector2(i, j) * fieldSize;
                Field field = fieldObject.GetComponent<Field>();
                field.type = fieldTypes[i, j];
                field.x = i;
                field.y = j;
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
        GameObject characterObject = Instantiate(characterInstance);
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
}
