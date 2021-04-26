using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class CharacterStats
{
    public int health;
    public int initiative;
    public int protection;
    public int evasion;
    public int range;
    public int criticalStrikeChance;
    public int minDamage;
    public int maxDamage;
    public int punching;
    public int agility;
    public int accuracy;
    public List<Skill> skills = new List<Skill>();

    public bool aimedShot = false;

    public CharacterStats(CharacterClass clazz, int level)
    {
        if (level < 1 || level > 3)
        {
            throw new Exception("Level can be only 1-3");
        }
        skills.Add(new Skill(1, SkillAim.ENEMY, character =>
        {
            int chance = accuracy - evasion;
            for (int i = 0; i < agility; i++)
            {
                if (Random.value * 100 < chance) continue;
                character.Hit(aimedShot ? maxDamage : minDamage, maxDamage, criticalStrikeChance, punching);
                if (character == null) break;
            }

            aimedShot = false;
        }, "Атака"));
        switch (clazz)
        {
            case CharacterClass.MELEE_FIGHTER:
                skills.Add(new Skill(3, SkillAim.ENEMY, character => { character.stunnedSteps += 1; }, "Удар по кумполу"));
                switch (level)
                {
                    case 1:
                    {
                        health = 60;
                        initiative = 12;
                        protection = 15;
                        evasion = 12;
                        range = 2;
                        criticalStrikeChance = 15;
                        minDamage = 21;
                        maxDamage = 25;
                        punching = 18;
                        agility = 2;
                        accuracy = 75;
                        break;
                    }
                    case 2:
                    {
                        health = 80;
                        initiative = 12;
                        protection = 15;
                        evasion = 12;
                        range = 2;
                        criticalStrikeChance = 15;
                        minDamage = 26;
                        maxDamage = 30;
                        punching = 18;
                        agility = 2;
                        accuracy = 75;
                        break;
                    }
                    case 3:
                    {
                        health = 100;
                        initiative = 12;
                        protection = 15;
                        evasion = 14;
                        range = 2;
                        criticalStrikeChance = 16;
                        minDamage = 32;
                        maxDamage = 36;
                        punching = 20;
                        agility = 2;
                        accuracy = 75;
                        break;
                    }
                }
                break;
            case CharacterClass.RANGED_FIGHTER:
                skills.Add(new Skill(2, SkillAim.SELF, character => { character.stats.aimedShot = true; }, "Прицельный выстрел"));
                switch (level)
                {
                    case 1:
                    {
                        health = 45;
                        initiative = 10;
                        protection = 10;
                        evasion = 17;
                        range = 12;
                        criticalStrikeChance = 22;
                        minDamage = 25;
                        maxDamage = 31;
                        punching = 23;
                        agility = 3;
                        accuracy = 60;
                        break;
                    }
                    case 2:
                    {
                        health = 55;
                        initiative = 10;
                        protection = 10;
                        evasion = 17;
                        range = 12;
                        criticalStrikeChance = 22;
                        minDamage = 32;
                        maxDamage = 38;
                        punching = 23;
                        agility = 3;
                        accuracy = 60;
                        break;
                    }
                    case 3:
                    {
                        health = 65;
                        initiative = 10;
                        protection = 10;
                        evasion = 19;
                        range = 12;
                        criticalStrikeChance = 23;
                        minDamage = 38;
                        maxDamage = 43;
                        punching = 25;
                        agility = 3;
                        accuracy = 62;
                        break;
                    }
                }
                break;
            case CharacterClass.TANK:
                skills.Add(new Skill(2, SkillAim.SELF, character => { character.isMagnitAttack = true; }, "Как за каменной стеной"));
                switch (level)
                {
                    case 1:
                    {
                        health = 85;
                        initiative = 7;
                        protection = 22;
                        evasion = 8;
                        range = 1;
                        criticalStrikeChance = 10;
                        minDamage = 18;
                        maxDamage = 20;
                        punching = 15;
                        agility = 3;
                        accuracy = 80;
                        break;
                    }
                    case 2:
                    {
                        health = 110;
                        initiative = 7;
                        protection = 22;
                        evasion = 8;
                        range = 1;
                        criticalStrikeChance = 10;
                        minDamage = 20;
                        maxDamage = 23;
                        punching = 15;
                        agility = 3;
                        accuracy = 80;
                        break;
                    }
                    case 3:
                    {
                        health = 135;
                        initiative = 7;
                        protection = 23;
                        evasion = 8;
                        range = 1;
                        criticalStrikeChance = 10;
                        minDamage = 23;
                        maxDamage = 25;
                        punching = 16;
                        agility = 3;
                        accuracy = 80;
                        break;
                    }
                }
                break;
            case CharacterClass.SUPPORT:
                switch (level)
                {
                    case 1:
                    {
                        health = 55;
                        initiative = 8;
                        protection = 18;
                        evasion = 9;
                        range = 2;
                        criticalStrikeChance = 10;
                        minDamage = 15;
                        maxDamage = 17;
                        punching = 10;
                        agility = 4;
                        accuracy = 80;
                        break;
                    }
                    case 2:
                    {
                        health = 70;
                        initiative = 8;
                        protection = 18;
                        evasion = 9;
                        range = 2;
                        criticalStrikeChance = 10;
                        minDamage = 17;
                        maxDamage = 20;
                        punching = 10;
                        agility = 4;
                        accuracy = 80;
                        break;
                    }
                    case 3:
                    {
                        health = 85;
                        initiative = 8;
                        protection = 18;
                        evasion = 10;
                        range = 2;
                        criticalStrikeChance = 11;
                        minDamage = 20;
                        maxDamage = 22;
                        punching = 10;
                        agility = 4;
                        accuracy = 80;
                        break;
                    }
                }
                break;
            case CharacterClass.OOZE_MELEE:
                switch (level)
                {
                    case 1:
                    {
                        health = 60;
                        initiative = 10;
                        protection = 5;
                        evasion = 10;
                        range = 1;
                        criticalStrikeChance = 10;
                        minDamage = 18;
                        maxDamage = 20;
                        punching = 15;
                        agility = 2;
                        accuracy = 75;
                        break;
                    }
                    case 2:
                    {
                        health = 80;
                        initiative = 10;
                        protection = 7;
                        evasion = 10;
                        range = 1;
                        criticalStrikeChance = 10;
                        minDamage = 20;
                        maxDamage = 22;
                        punching = 15;
                        agility = 2;
                        accuracy = 75;
                        break;
                    }
                    case 3:
                    {
                        health = 100;
                        initiative = 10;
                        protection = 9;
                        evasion = 8;
                        range = 1;
                        criticalStrikeChance = 10;
                        minDamage = 22;
                        maxDamage = 24;
                        punching = 15;
                        agility = 2;
                        accuracy = 75;
                        break;
                    }
                }
                break;
            case CharacterClass.OOZE_RANGED:
                switch (level)
                {
                    case 1:
                    {
                        health = 50;
                        initiative = 9;
                        protection = 5;
                        evasion = 10;
                        range = 9;
                        criticalStrikeChance = 10;
                        minDamage = 15;
                        maxDamage = 17;
                        punching = 20;
                        agility = 3;
                        accuracy = 60;
                        break;
                    }
                    case 2:
                    {
                        health = 65;
                        initiative = 9;
                        protection = 6;
                        evasion = 10;
                        range = 9;
                        criticalStrikeChance = 10;
                        minDamage = 18;
                        maxDamage = 20;
                        punching = 20;
                        agility = 3;
                        accuracy = 60;
                        break;
                    }
                    case 3:
                    {
                        health = 80;
                        initiative = 9;
                        protection = 7;
                        evasion = 8;
                        range = 9;
                        criticalStrikeChance = 15;
                        minDamage = 20;
                        maxDamage = 22;
                        punching = 25;
                        agility = 3;
                        accuracy = 60;
                        break;
                    }
                }
                break;
        }
    }
}
