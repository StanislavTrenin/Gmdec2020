using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class DataPausePanel : DataPanel
{
    public Button ButtonBack;
    public Button ButtonExit;
}

public class PausePanel : Panel
{
    private DataPausePanel dataPauseDataPanel;
    
    public PausePanel(DataPausePanel dataPauseDataPanel) : base(dataPauseDataPanel)
    {
        this.dataPauseDataPanel = dataPauseDataPanel;
        this.dataPauseDataPanel.ButtonBack.onClick.AddListener(HidePanel);
        this.dataPauseDataPanel.ButtonExit.onClick.AddListener(() => SceneManager.LoadScene("Menu"));
    }
}
