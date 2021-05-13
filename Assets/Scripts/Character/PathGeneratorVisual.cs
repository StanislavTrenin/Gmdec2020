using System;
using System.Collections.Generic;
using UnityEngine;

public class PathGeneratorVisual : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    public PathGenerator PathShortestGenerator { get; private set; }
    public PathGenerator PathStraightGenerator { get; private set; }
    
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
    
    private void Awake()
    {
        PathShortestGenerator = new PathShortestGenerator(lineRenderer);
        PathStraightGenerator = new PathStraightGenerator(lineRenderer);
    }

    public void ResetLinePath()
    {
        lineRenderer.positionCount = 0;
    }
}

public abstract class PathGenerator
{
    protected LineRenderer lineRenderer;
    public FieldVisibilityType FieldVisibilityType { get; protected set; }

    protected PathGenerator(LineRenderer lineRenderer)
    {
        this.lineRenderer = lineRenderer;
    }

    public abstract void GeneratePath(int finishX, int finishY, FieldData fieldData);

}

public class PathShortestGenerator: PathGenerator
{
    public PathShortestGenerator(LineRenderer lineRenderer) : base(lineRenderer)
    {
        
    }
    
    public override void GeneratePath(int finishX, int finishY, FieldData fieldData)
    {
        Field startField = fieldData.ActiveCharacter.field;
        Field finishField = fieldData.Fields[finishX, finishY];

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
        int[] addX = {-1, 0, 0, 1};
        int[] addY = {0, -1, 1, 0};
        
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
}

public class PathStraightGenerator : PathGenerator
{
    private FieldData fieldData;
    private int slope;
    private int x;
    private int y;
    private int x0;
    private int y0;
    private int x1;
    private int y1;

    public PathStraightGenerator(LineRenderer lineRenderer) : base(lineRenderer)
    {

    }

    public override void GeneratePath(int finishX, int finishY, FieldData fieldData)
    {
        this.fieldData = fieldData;
        
        var rowsCount = fieldData.Fields.GetUpperBound(0) + 1;
        var linePositions = new Vector3[]
        {
            new Vector2(fieldData.ActiveCharacter.field.x + 0.5f,
                rowsCount - fieldData.ActiveCharacter.field.y - 0.5f) * fieldData.FieldSize,
            new Vector2(finishX + 0.5f, rowsCount - finishY - 0.5f) * fieldData.FieldSize
        };

        lineRenderer.positionCount = linePositions.Length;
        lineRenderer.SetPositions(linePositions);
        
        CheckPath(fieldData.ActiveCharacter.field.x, fieldData.ActiveCharacter.field.y, finishX, finishY, fieldData);
    }

    public void CheckPath(int x0, int y0, int x1, int y1, FieldData fieldData)
    {
        FieldVisibilityType = FieldVisibilityType.Visible;
        x = x0;
        y = y0;
        slope = 0;
        
        int yLength = y1 - y0;
        int xLength = x1 - x0;
        int yDirection;
        int xDirection;

        if (yLength < 0) yDirection = -1;
        else if (yLength > 0) yDirection = 1;
        else yDirection = 0;

        if (xLength < 0) xDirection = -1;
        else if (xLength > 0) xDirection = 1;
        else xDirection = 0;

        while (x != x1 || y != y1)
        {
            if (Math.Abs(yLength) > Math.Abs(xLength))
            {
                MoveInFieldsY(xLength, yLength, xDirection, yDirection);
            }
            else
            {
                MoveInFieldsX(xLength, yLength, xDirection, yDirection);
            }

            if (fieldData.Fields[x, y].type == FieldType.OBSTACLE)
            {
                FieldVisibilityType = FieldVisibilityType.PartiallyVisible;
            }
            else if (fieldData.Fields[x, y].type == FieldType.WALL)
            {
                FieldVisibilityType = FieldVisibilityType.NoVisible;
                return;
            }
        }
    }

    private void MoveInFieldsY(int xLength, int yLength, int xDirection, int yDirection)
    {
        slope += xLength * xDirection;
        if (slope > 0)
        {
            slope -= yLength * yDirection;
            x += xDirection;
        }

        y += yDirection;
    }
    
    private void MoveInFieldsX(int xLength, int yLength, int xDirection, int yDirection)
    {
        slope += yLength * yDirection;
        if (slope > 0)
        {
            slope -= xLength * xDirection;
            y += yDirection;
        }

        x += xDirection;
    }
}
