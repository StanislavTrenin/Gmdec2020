using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private GameObject characterInstance;
    [SerializeField] private GameObject fieldInstance;

    private Field[,] fields;
    private Character[,] characters;

    // Start is called before the first frame update
    void Start()
    {
        GenerateField();
        GenerateCharacters();
    }

    private void GenerateField()
    {
        FieldType[,] fieldTypes = MapDeserializer.Deserialize("demo_map.txt");
        int rowsCount = fieldTypes.GetUpperBound(0) + 1;
        int columnsCount = fieldTypes.GetUpperBound(1) + 1;
        for (int i = 0; i < rowsCount; i++)
        {
            for (int j = 0; j < columnsCount; j++)
            {
                GameObject fieldObject = Instantiate(fieldInstance);
                Field field = fieldObject.GetComponent<Field>();
                field.type = fieldTypes[i, j];
                fields[i, j] = field;
            }
        }
    }

    private void GenerateCharacters()
    {
        
    }
}
