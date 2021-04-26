using System;
using UnityEngine;

[RequireComponent(typeof(CharacterAction))]
public class Character : MonoBehaviour
{
    [Header("General")]
    public int level;
    public CharacterClass clazz;
    
    [NonSerialized] public CharacterAction CharacterAction;

    [NonSerialized] public CharacterStats stats;

    public bool isPlayer
    {
        get { return _isPlayer; }
        set
        {
            _isPlayer = value;
            _spriteRenderer.flipX = !_isPlayer;
        }
    }
    public bool _isPlayer;
    public Field field
    {
        get { return _field; }
        set
        {
            if (_field != null)
            {
                _field.character = null;
            }
            _field = value;
            _field.character = this;
        }
    }
    private Field _field;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        stats = new CharacterStats(clazz, level);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        CharacterAction = GetComponent<CharacterAction>();
    }

}
