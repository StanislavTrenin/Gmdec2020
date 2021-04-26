using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public Skill skill;
    [NonSerialized] public Controller controller;
    [SerializeField] private Text text;
    
    public void OnClick()
    {
        controller.OnSkillButton(skill);
    }

    public void UpdateText()
    {
        text.text = skill.name;
    }
    
}
