using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    public delegate void ApplySkill(Character character);
    
    private int stepsToReload;
    private int maxStepsToReload;
    private SkillAim skillAim;
    private ApplySkill skill;
    private string _name;

    public string name => _name;

    public Skill(int maxStepsToReload, SkillAim skillAim, ApplySkill skill, string name)
    {
        this.maxStepsToReload = maxStepsToReload;
        this.skillAim = skillAim;
        this.skill = skill;
        _name = name;
    }
    
    public bool isReady()
    {
        return stepsToReload <= 0;
    }

    public bool canApply(Character source, Character destination)
    {
        switch (skillAim)
        {
            case SkillAim.SELF:
                if (source != destination) return false;
                break;
            case SkillAim.ENEMY:
                if (source.isPlayer == destination.isPlayer) return false;
                break;
            case SkillAim.ALLY:
                if (source.isPlayer != destination.isPlayer) return false;
                break;
        }

        return true;
    }

    public void Apply(Character character)
    {
        character.OnSkillApplied();
        skill(character);
        stepsToReload = maxStepsToReload;
    }

    public void UpdateStep()
    {
        stepsToReload--;
    }
}
