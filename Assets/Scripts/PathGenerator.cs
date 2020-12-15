using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGenerator : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    public int LinePositionCount => lineRenderer.positionCount;

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
    
    public void GenerateShortestPath(int finishX, int finishY, Vector2 fieldSize, Field[,] fields, Character activeCharacter)
    {
        for (int i = 0; i < fields.GetUpperBound(1) + 1; i++)
        {
            for (int j = 0; j < fields.GetUpperBound(0) + 1; j++)
            {
                Debug.Log(new Vector2(fields[i, j].x, fields[i, j].y));
            }
        }

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
    
}
