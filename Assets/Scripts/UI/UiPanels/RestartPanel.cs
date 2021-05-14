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
    }

    public override void ShowPanel()
    {
        dataRestartPanel.ButtonRestart.onClick.RemoveAllListeners();
        dataRestartPanel.ButtonRestart.onClick.AddListener(() => SceneManager.LoadScene("Game"));
        dataRestartPanel.TextRestart.text = TextDataKeeper.TextDataDict["Restart"];
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