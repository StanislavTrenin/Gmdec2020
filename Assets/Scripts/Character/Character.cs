using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CharacterAction))]
public class Character : MonoBehaviour
{
    private static List<Skill> SKILLS = new List<Skill>();

    private static int[][] OFFSETS =
    {
        new[] {-1, -1}, new[] {-1, 1}, new[] {1, -1}, new[] {1, 1}
    };
    
    public delegate void OnDestroy(Character character);
    public event OnDestroy Destroyed;

    public delegate void OnHit();
    public event OnHit Attacked;
    
    [Header("General")]
    public int level;
    public CharacterClass clazz;

    [NonSerialized] public CharacterAction CharacterAction;

    [NonSerialized] public CharacterStats stats;

    [NonSerialized] public int stunnedSteps = 0;

    [NonSerialized] public Controller controller;

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

    [NonSerialized] public bool isMagnitAttack = false;

    private void Awake()
    {
        UpdateStats();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        CharacterAction = GetComponent<CharacterAction>();
    }

    public void UpdateStats()
    {
        stats = new CharacterStats(clazz, level);
    }

    public void Hit(int minDamage, int maxDamage, int crit, int penetration)
    {
        foreach (var offset in OFFSETS)
        {
            Field field = null;
            try
            {
                field = controller.fieldData.Fields[this.field.x + offset[0], this.field.y + offset[1]];
            }
            catch (IndexOutOfRangeException)
            {
                continue;
            }

            if (field.character == null) continue;
            if (field.character.isPlayer != isPlayer) continue;
            if (field.character.isMagnitAttack)
            {
                field.character.isMagnitAttack = false;
                field.character.Hit(minDamage, maxDamage, crit, penetration);
                return;
            }
        }
        Attacked?.Invoke();
        int damage = Random.Range(minDamage, maxDamage);
        if (Random.Range(0, 100) < crit)
        {
            damage += damage;
        }

        int currentArmor = stats.protection - penetration;

        int hit = damage - currentArmor;

        if (hit < 0)
        {
            stats.currentHealth -= minDamage / 10;
        }
        else
        {
            stats.currentHealth -= damage;
        }

        if (stats.currentHealth < 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        _field.character = null;
        Destroyed?.Invoke(this);
        Destroy(gameObject);
    }

    public void UpdateSkillsSteps()
    {
        foreach (Skill skill in stats.skills)
        {
            skill.UpdateStep();
        }
    }

    public List<Skill> GetActiveSkills()
    {
        SKILLS.Clear();
        foreach (Skill skill in stats.skills)
        {
            if (skill.isReady())
            {
                SKILLS.Add(skill);
            }
        }

        return SKILLS;
    }
}
