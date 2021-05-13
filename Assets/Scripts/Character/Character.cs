using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CharacterAction))]
public class Character : MonoBehaviour
{
    private static List<Skill> SKILLS = new List<Skill>();

    private static int[][] OFFSETS =
    {
        new[] {-1, 0}, new[] {0, -1}, new[] {0, 1}, new[] {1, 0}
    };

    private static List<Character> ENEMIES = new List<Character>();
    private static List<Field> FIELDS = new List<Field>();
    private static PathStraightGenerator _pathStraightGenerator = new PathStraightGenerator(null);
    
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

    [NonSerialized] public int currentStep = 0;

    [SerializeField] private GameObject damageText;

    public bool isPlayer
    {
        get { return _isPlayer; }
        set
        {
            _isPlayer = value;
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

    [NonSerialized] public bool isMagnitAttack = false;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        UpdateStats();
        spriteRenderer = GetComponent<SpriteRenderer>();
        CharacterAction = GetComponent<CharacterAction>();
    }

    private void Update()
    {
        spriteRenderer.sortingOrder = -Mathf.RoundToInt(_field.transform.position.y) + 4;
    }

    public void UpdateStats()
    {
        stats = new CharacterStats(clazz, level);
    }

    private void GetNearestEnemies()
    {
        ENEMIES.Clear();
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
            if (field.character.isPlayer == isPlayer) continue;
            ENEMIES.Add(field.character);
        }
    }

    private Field GetNearWeakEnemy(bool isAttack)
    {
        float minDist = Single.PositiveInfinity;
        Character nearEnemy = null;
        Field[,] fields = controller.fieldData.Fields;
        int rowsCount = fields.GetUpperBound(0) + 1;
        int columnsCount = fields.GetUpperBound(1) + 1;

        for (var i = 0; i < columnsCount; i++)
        {
            for (var j = 0; j < rowsCount; j++)
            {
                Character character = fields[i, j].character;
                if (character == null) continue;
                if (character.isPlayer == isPlayer) continue;
                float dist = Math.Abs(field.x - i) + Math.Abs(field.y - j);
                
                if (dist < minDist ||
                    (nearEnemy != null && dist == minDist &&
                     character.stats.currentHealth < nearEnemy.stats.currentHealth))
                {
                    nearEnemy = character;
                    minDist = dist;
                }
            }
        }

        List<Field> nearestFields = new List<Field>();
        
        if (nearEnemy != null)
        {
            if (!isAttack)
            {
                if (nearEnemy.field.x - 1 >= 0 &&
                    fields[nearEnemy.field.x - 1, nearEnemy.field.y].type == FieldType.FLOR)
                    nearestFields.Add(fields[nearEnemy.field.x - 1, nearEnemy.field.y]);

                if (nearEnemy.field.x + 1 < fields.Length &&
                    fields[nearEnemy.field.x + 1, nearEnemy.field.y].type == FieldType.FLOR)
                    nearestFields.Add(fields[nearEnemy.field.x + 1, nearEnemy.field.y]);

                if (nearEnemy.field.y - 1 >= 0 &&
                    fields[nearEnemy.field.y, nearEnemy.field.y - 1].type == FieldType.FLOR)
                    nearestFields.Add(fields[nearEnemy.field.x, nearEnemy.field.y - 1]);

                if (nearEnemy.field.y + 1 < fields.Length &&
                    fields[nearEnemy.field.y, nearEnemy.field.y + 1].type == FieldType.FLOR)
                    nearestFields.Add(fields[nearEnemy.field.x, nearEnemy.field.y + 1]);
            }
            else
            {
                return nearEnemy.field;
            }
        }

        return nearestFields[0];
    }

    private void GetVisibleEnemies()
    {
        ENEMIES.Clear();
        Field[,] fields = controller.fieldData.Fields;
        int rowsCount = fields.GetUpperBound(0) + 1;
        int columnsCount = fields.GetUpperBound(1) + 1;
        for (var i = 0; i < columnsCount; i++)
        {
            for (var j = 0; j < rowsCount; j++)
            {
                Field field = fields[i, j];
                Character character = field.character;
                if (character == null) continue;
                if (character.isPlayer == isPlayer) continue;
                _pathStraightGenerator.CheckPath(field.x, field.y, controller.fieldData);
                if (_pathStraightGenerator.FieldVisibilityType == FieldVisibilityType.Visible)
                {
                    ENEMIES.Add(character);
                }
            }
        }
    }

    private Field GetVisibleField()
    {
        ENEMIES.Clear();
        Field[,] fields = controller.fieldData.Fields;
        int rowsCount = fields.GetUpperBound(0) + 1;
        int columnsCount = fields.GetUpperBound(1) + 1;
        for (var i = 0; i < columnsCount; i++)
        {
            for (var j = 0; j < rowsCount; j++)
            {
                Character character = fields[i, j].character;
                if (character == null) continue;
                if (character.isPlayer == isPlayer) continue;
                ENEMIES.Add(character);
            }
        }
        FIELDS.Clear();
        float minDist = Single.PositiveInfinity;
        for (var i = 0; i < columnsCount; i++)
        {
            for (var j = 0; j < rowsCount; j++)
            {
                bool visible = false;
                float dist = Math.Abs(field.x - i) + Math.Abs(field.y - j);
                if (dist > minDist) continue;
                for (var k = 0; k < ENEMIES.Count; k++)
                {
                    Field field = ENEMIES[k]._field;
                    _pathStraightGenerator.CheckPath(field.x, field.y, controller.fieldData);
                    if (_pathStraightGenerator.FieldVisibilityType == FieldVisibilityType.Visible)
                    {
                        visible = true;
                        break;
                    }
                }
                if (!visible) continue;
                if (dist < minDist)
                {
                    FIELDS.Clear();
                }
                FIELDS.Add(fields[i, j]);
            }
        }
        return FIELDS[Random.Range(0, FIELDS.Count)];
    }

    public void AI(Action endTurn)
    {
        if (stunnedSteps <= 0)
        {
            switch (clazz)
            {
                case CharacterClass.OOZE_MELEE:
                {
                    GetNearestEnemies();
                    if (ENEMIES.Count == 0)
                    {
                        controller.fieldData.ActiveSkill = null;
                        Field fieldNearestEnemy = GetNearWeakEnemy(false);
                        fieldNearestEnemy.OnAI();
                        GetNearestEnemies();
                    }

                    if (ENEMIES.Count > 0)
                    {
                        controller.fieldData.ActiveSkill = stats.skills[0];
                        Field fieldNearestEnemy = GetNearWeakEnemy(true);
                        fieldNearestEnemy.OnAI();
                    }
                    break;
                }
                case CharacterClass.OOZE_RANGED:
                {
                    GetVisibleEnemies();
                    if (ENEMIES.Count == 0)
                    {
                        controller.fieldData.ActiveSkill = null;
                        GetVisibleField().OnAI();
                        GetVisibleEnemies();
                    }

                    if (ENEMIES.Count > 0)
                    {
                        controller.fieldData.ActiveSkill = stats.skills[0];
                        Character enemy = ENEMIES[0];
                        for (int i = 1; i < ENEMIES.Count; i++)
                        {
                            Character character = ENEMIES[i];
                            if (character.stats.currentHealth < enemy.stats.currentHealth)
                            {
                                enemy = character;
                            }
                        }
                        enemy._field.OnAI();
                    }
                    break;
            }
            }
        }
        endTurn.Invoke();
    }

    public void OnSkillApplied()
    {
        Debug.Log("Applied");
        Attacked?.Invoke();
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
        
        int damage = Random.Range(minDamage, maxDamage);
        if (Random.Range(0, 100) < crit)
        {
            damage += damage;
        }

        int currentArmor = stats.protection - penetration;

        int hit = damage - currentArmor;

        if (hit < 0)
        {
            damage = minDamage / 10;
        }

        stats.currentHealth -= damage;

        HPText text = Instantiate(damageText).GetComponentInChildren<HPText>();
        text.transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                text.transform.position.z);
        text.text.text = $"-{damage}";

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
