using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DataRestartPanel : DataPanel
{
    public Button ButtonRestart;
    public Text TextRestart;
}

public class RestartPanel : Panel
{
    private DataRestartPanel dataRestartPanel;

    public RestartPanel(DataRestartPanel dataRestartPanel) : base(dataRestartPanel)
    {
        this.dataRestartPanel = dataRestartPanel;
        this.dataRestartPanel.ButtonRestart.onClick.AddListener(() => SceneManager.LoadScene("Game"));

        try
        {
            this.dataRestartPanel.TextRestart.text = TextDataKeeper.TextDataDict?["Restart"];
        }
        catch (KeyNotFoundException)
        {
        }

    }

    public override void ShowPanel()
    {
        dataRestartPanel.ButtonRestart.gameObject.SetActive(true);
        dataRestartPanel.ButtonRestart.GetComponentInChildren<Text>().text = "Начать заново";
        base.ShowPanel();
    }

    public override void HidePanel()
    {
        dataRestartPanel.ButtonRestart.gameObject.SetActive(false);
        base.HidePanel();
    }
}