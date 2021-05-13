using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class DataStatsPanel : DataPanel
{
    [NonSerialized] public CharacterStats characterStats;
    public Button exitButton;
    public Text HpText;
    public Text InitiativeText;
    public Text ProtectionText;
    public Text EvasionText;
    public Text RangeText;
    public Text CriticalStrikeChanceText;
    public Text MinDamageText;
    public Text MaxDamageText;
    public Text PunchingText;
    public Text AgilityText;
    public Text AccuracyText;
}

public class StatsPanel : Panel
{
    private CharacterStats characterStats;
    private DataStatsPanel dataStatsPanel;
    
    public StatsPanel(DataStatsPanel dataStatsPanel) : base(dataStatsPanel)
    {
        this.dataStatsPanel = dataStatsPanel;
        this.dataStatsPanel.exitButton.onClick.AddListener(HidePanel);
    }

    public override void ShowPanel()
    {
        SetCharacterStats();
        base.ShowPanel();
    }

    private void SetCharacterStats()
    {
        characterStats = dataStatsPanel.characterStats;
        dataStatsPanel.HpText.text = characterStats.currentHealth + " / " + characterStats.health;
        dataStatsPanel.InitiativeText.text = characterStats.initiative.ToString();
        dataStatsPanel.ProtectionText.text = characterStats.protection.ToString();
        dataStatsPanel.EvasionText.text = characterStats.evasion.ToString();
        dataStatsPanel.RangeText.text = characterStats.range.ToString();
        dataStatsPanel.CriticalStrikeChanceText.text = characterStats.criticalStrikeChance.ToString();
        dataStatsPanel.MinDamageText.text = characterStats.minDamage.ToString();
        dataStatsPanel.MaxDamageText.text = characterStats.maxDamage.ToString();
        dataStatsPanel.PunchingText.text = characterStats.punching.ToString();
        dataStatsPanel.AgilityText.text = characterStats.agility.ToString();
        dataStatsPanel.AccuracyText.text = characterStats.accuracy.ToString();
    }
}
