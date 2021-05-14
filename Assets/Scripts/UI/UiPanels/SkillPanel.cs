using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[Serializable]
public class DataSkillPanel : DataPanel
{
    public Text TextDescriptionSkill;
    public RectTransform PanelRect;
    public Animation PanelAnim;
}

public class SkillPanel : Panel
{
    private DataSkillPanel dataSkillPanel;
    
    public SkillPanel(DataSkillPanel dataSkillPanel) : base(dataSkillPanel)
    {
        this.dataSkillPanel = dataSkillPanel;
    }

    public override void ShowPanel()
    {
        dataSkillPanel.PanelAnim.Play();
        dataSkillPanel.TextDescriptionSkill.text = SkillButton.StaticSkill.description;
        dataSkillPanel.PanelObject.transform.position = new Vector2(
            SkillButton.SkillButtonPosition.x - dataSkillPanel.PanelRect.sizeDelta.x,
            SkillButton.SkillButtonPosition.y + dataSkillPanel.PanelRect.sizeDelta.y);
        base.ShowPanel();
    }
}
