using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsDataPanel : DataPanel
{
    public GameObject exitButton;
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
    public StatsPanel(DataPanel dataPanel) : base(dataPanel)
    {
    }
    
    
}
