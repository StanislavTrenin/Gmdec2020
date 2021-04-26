using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DataLosePanel : DataPanel
{
    public string TextLose;
}

public class LosePanel : PanelEndGameDecorator
{
    private DataLosePanel dataLosePanel;
    public LosePanel(Panel panel, DataEndGamePanel endGamePanel, DataLosePanel dataLosePanel) : 
        base(panel, endGamePanel)
    {
        try
        {
            this.dataLosePanel = dataLosePanel;
        }
        catch (Exception e)
        {
            // ignored
        }
    }
  
    public override void ShowPanel()
    {
        dataEndGamePanel.TxtOther.text = dataLosePanel.TextLose;
        panel.ShowPanel();
    }

    private void Restart()
    {
        SceneManager.LoadScene("Game");
    }
}

