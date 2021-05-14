using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour, IPointerClickHandler
{
    public Skill skill;
    public static Skill StaticSkill;
    [NonSerialized] public Controller controller;
    [SerializeField] private Text text;
    [NonSerialized] public UiManager UiManager;
    public static Vector2 SkillButtonPosition;

    public void OnClick()
    {
        controller.OnSkillButton(skill);
    }

    public void UpdateText()
    {
        text.text = skill.name;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            StaticSkill = skill;
            SkillButtonPosition = transform.position;
            UiManager.ShowPanel(UiPanelNames.SkillPanel);
        }
    }
}
