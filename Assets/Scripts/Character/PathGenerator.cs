using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    private FieldVisibilityType fieldVisibilityType;

    public List<Vector2> LinePositions
    {
        get
        {
            var lineList = new List<Vector2>();

            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                lineList.Add(lineRenderer.GetPosition(i));
            }

            return lineList;
        }
    }

    public void ResetLinePath()
    {
        lineRenderer.positionCount = 0;
    }

    public void GenerateShortestPath(int finishX, int finishY, FieldData fieldData)
    {
        Field startField = fieldData.ActiveCharacter.field;
        Field finishField = fieldData.Fields[finishX, finishY];

        if (finishField.type != FieldType.OBSTACLE)
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
                            Field nearField = fieldData.Fields[x + addX[i], y + addY[i]];
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
                int rowsCount = fieldData.Fields.GetUpperBound(0) + 1;
                Vector3[] shortestPath = new Vector3[path.Count];
                for (int i = 0; i < shortestPath.Length; i++)
                {
                    Field pathPoint = path.Pop();
                    
                    shortestPath[i] = new Vector2(pathPoint.x + 0.5f, rowsCount - pathPoint.y - 0.5f) * fieldData.FieldSize;
                }
                
                lineRenderer.positionCount = shortestPath.Length;
                lineRenderer.SetPositions(shortestPath);

                return;
            }
            for (int i = 0; i < addX.Length; i++)
            {
                try
                {
                    Field nearField = fieldData.Fields[x + addX[i], y + addY[i]];
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

    public FieldVisibilityType  GenerateStraightPath(int finishX, int finishY, FieldData fieldData)
    {
        //TODO надо здесь написать алгоритм который будет строить прямую линию и смотреть какие клетки под ней находятся
        return fieldVisibilityType;
    }
    
}
