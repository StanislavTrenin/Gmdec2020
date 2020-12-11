using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Controller : MonoBehaviour
{
    [SerializeField] private GameObject characterInstance;
    [SerializeField] private GameObject fieldInstance;
    [SerializeField] private Material lineMaterial;

    private Vector2 fieldSize;
    private Field[,] fields;
    private Queue<Character> playerCharactersQueue = new Queue<Character>();
    private Queue<Character> enemyCharactersQueue = new Queue<Character>();
    private bool isPlayerStep;
    private Character activeCharacter;
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
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
                field.Notify += GenerateShortestPath;
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
        lineRenderer.positionCount = 0;
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

    private void GenerateShortestPath(int finishX, int finishY)
    {
        Field startField = activeCharacter.field;
        Field finishField = fields[finishX, finishY];
        if (finishField.type != FieldType.FLOR)
        {
            lineRenderer.positionCount = 0;
            return;
        }
        Queue<Field> fieldsToCheck = new Queue<Field>();
        Stack<Field> path = new Stack<Field>();
        Dictionary<Field, int> visited = new Dictionary<Field, int>();
        visited.Add(startField, 0);
        fieldsToCheck.Enqueue(startField);
        int[] addX = {-1, 0, 1, -1, 1, -1, 0, 1};
        int[] addY = {-1, -1, -1, 0, 0, 1, 1, 1};
        while (fieldsToCheck.Count > 0)
        {
            Field f = fieldsToCheck.Dequeue();
            int value = visited[f];
            int x = f.x;
            int y = f.y;
            if (f == finishField)
            {
                path.Push(f);
                while (value > 0)
                {
                    value--;
                    for (int i = 0; i < addX.Length; i++)
                    {
                        try
                        {
                            Field nearField = fields[x + addX[i], y + addY[i]];
                            if (visited.ContainsKey(nearField) && visited[nearField] == value)
                            {
                                f = nearField;
                                path.Push(f);
                                x = f.x;
                                y = f.y;
                                break;
                            }
                        }
                        catch (IndexOutOfRangeException e)
                        {
                    
                        }
                    }
                }
                int rowsCount = fields.GetUpperBound(0) + 1;
                Vector3[] shortestPath = new Vector3[path.Count];
                for (int i = 0; i < shortestPath.Length; i++)
                {
                    Field pathPoint = path.Pop();
                    shortestPath[i] = new Vector2(pathPoint.x + 0.5f, rowsCount - pathPoint.y - 0.5f) * fieldSize;
                }
                lineRenderer.positionCount = shortestPath.Length;
                lineRenderer.SetPositions(shortestPath);
                return;
            }
            for (int i = 0; i < addX.Length; i++)
            {
                try
                {
                    Field nearField = fields[x + addX[i], y + addY[i]];
                    if (nearField.type == FieldType.FLOR && !visited.ContainsKey(nearField))
                    {
                        visited[nearField] = value + 1;
                        fieldsToCheck.Enqueue(nearField);
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    
                }
            }
        }

        lineRenderer.positionCount = 0;
    }

    private void SetCamera()
    {
        CameraController cameraController = Camera.main.GetComponent<CameraController>();
        cameraController.SetSize((fields.GetUpperBound(1) + 1) * fieldSize.x,
            (fields.GetUpperBound(0) + 1) * fieldSize.y);
    }
}
