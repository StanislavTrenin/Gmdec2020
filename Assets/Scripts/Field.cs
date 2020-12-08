using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public delegate void FieldClicked(int x, int y);
    public event FieldClicked Notify;

    public int x;
    public int y;

    public FieldType type
    {
        get { return _character == null ? _type : FieldType.WALL; }
        set { _type = value; }
    }

    public Character character
    {
        get { return _character; }
        set { _character = value; }
    }

    private FieldType _type;
    private Character _character;

    void OnPointerClick()
    {
        Notify?.Invoke(x, y);
    }

}
