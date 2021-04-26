using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CharacterAction))]
public class Character : MonoBehaviour
{
    private static List<Skill> SKILLS = new List<Skill>();
    
    public delegate void OnDestroy(Character character);
    public event OnDestroy Destroyed;

    public delegate void OnHit();
    public event OnHit Attacked;
    
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

    public void Hit(int minDamage, int maxDamage, int crit, int penetration)
    {
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
            stats.health -= minDamage / 10;
        }
        else
        {
            stats.health -= damage;
        }

        if (stats.health < 0)
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
