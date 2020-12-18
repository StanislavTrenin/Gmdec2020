﻿using System;
using UnityEngine;

[RequireComponent(typeof(CharacterAction))]
public class Character : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private string name;
    [SerializeField] private string nickName;
    [SerializeField][Range(1, 25)] private int level;
    [SerializeField][Range(0, 10000)] private int experience;
    [SerializeField][Range(0, 8)] private int actionPoints;

    [Header("Health")]
    [SerializeField] [Range(0, 200)] private int body;
    [SerializeField] [Range(0, 200)] private int sanity;
    [SerializeField] [Range(0, 200)] private int resolve;

    [Header("Resistance")]
    [SerializeField] [Range(0, 200)] private int painThreshold;
    [SerializeField] [Range(0, 200)] private int sagacity;
    [SerializeField] [Range(0, 200)] private int bravery;

    [Header("Attack Skills")]
    [SerializeField] [Range(0, 100)] private int combat;
    [SerializeField] [Range(0, 100)] private int ballistic;
    [SerializeField] [Range(-50, 50)] private int reflexes;
    [SerializeField] [Range(-50, 50)] private int premonition;

    [Header("Defence Skills")]
    [SerializeField] [Range(0, 300)] private int headArmor;
    [SerializeField] [Range(0, 300)] private int bodyArmor;
    [SerializeField] [Range(0, 300)] private int armsArmor;
    [SerializeField] [Range(0, 300)] private int legsArmor;
    [SerializeField] [Range(0, 50)] private int logic;
    [SerializeField] [Range(0, 50)] private int guts;
    
    [Header("Other")]
    [SerializeField] public int initiative;
    [NonSerialized] public CharacterAction CharacterAction;

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
        _spriteRenderer = GetComponent<SpriteRenderer>();
        CharacterAction = GetComponent<CharacterAction>();
    }

}