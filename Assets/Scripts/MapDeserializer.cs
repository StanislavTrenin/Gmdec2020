using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class MapDeserializer
{

    public static FieldType[,] Deserialize(string mapName)
    {
        mapName = mapName.Remove(mapName.IndexOf(".txt"));
        var textAsset = Resources.Load<TextAsset>($"Data/Maps/{mapName}");
        string[] rows = textAsset.text.Split('\n');
        int rowsCount = rows.Length;
        int columnsCount = rows[0].Split(' ').Length;
        FieldType[,] result = new FieldType[rowsCount, columnsCount];
        for (int i = 0; i < rowsCount; i++)
        {
            string[] row = rows[i].Split(' ');
            for (int j = 0; j < columnsCount; j++)
            {
                switch (Int32.Parse(row[j]))
                {
                    case 0:
                    {
                        result[j, i] = FieldType.FLOR;
                        break;
                    }
                    case 1:
                    {
                        result[j, i] = FieldType.WALL;
                        break;
                    }
                    case 2:
                    {
                        result[j, i] = FieldType.OBSTACLE;
                        break;
                    }
                }
            }
        }
        return result;
    }
    
}
