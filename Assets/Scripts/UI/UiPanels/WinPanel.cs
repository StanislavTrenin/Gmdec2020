using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


[System.Serializable]
public class DataWinPanel : DataPanel
{
    public string TextWin;
}

public class WinPanel : PanelEndGameDecorator
{
    private DataWinPanel dataWinPanel;
    public WinPanel(Panel panel, DataEndGamePanel endGamePanel, DataWinPanel dataWinPanel) : 
        base(panel, endGamePanel)
    {
        try
        {
            this.dataWinPanel = dataWinPanel;
        }
        catch (Exception e)
        {
            // ignored
        }
    }
    
    public override void ShowPanel()
    {
        dataEndGamePanel.TxtOther.text = dataWinPanel.TextWin;
        panel.ShowPanel();
    }
}


