using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Field : MonoBehaviour, IPointerClickHandler
{
    public delegate void FieldClicked(int x, int y);
    public event FieldClicked Notify;

    public int x;
    public int y;

    public FieldType type
    {
        get { return _character == null ? _type : FieldType.WALL; }
        set
        {
            _type = value;
            switch (_type)
            {
                case FieldType.FLOR:
                {
                    _spriteRenderer.color = Color.green;
                    break;
                }
                case FieldType.WALL:
                {
                    _spriteRenderer.color = Color.black;
                    break;
                }
                case FieldType.OBSTACLE:
                {
                    _spriteRenderer.color = new Color(0.647f, 0.165f, 0.165f);
                    break;
                }
            }
        }
    }

    public Character character
    {
        get { return _character; }
        set { _character = value; }
    }

    private FieldType _type;
    private Character _character;
    private SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Notify?.Invoke(x, y);
    }

}
