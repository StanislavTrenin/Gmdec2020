using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public FieldType type
    {
        get { return _type; }
        set { _type = value; }
    }
    private FieldType _type;
}
