using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DataEndGamePanel : DataPanel
{
    public Button ButtonEnd;
    public Text TextEndGame;
}

public class EndGamePanel : Panel
{
    private DataEndGamePanel dataEndGamePanel;

    public EndGamePanel(DataEndGamePanel dataEndGamePanel) : base(dataEndGamePanel)
    {
        this.dataEndGamePanel = dataEndGamePanel;
    }

    public override void ShowPanel()
    {
        dataEndGamePanel.TextEndGame.text = TextDataKeeper.TextDataDict["EndGame"];
        dataEndGamePanel.ButtonEnd.gameObject.SetActive(true);
        dataEndGamePanel.ButtonEnd.GetComponentInChildren<Text>().text = "Конец";
        dataEndGamePanel.ButtonEnd.onClick.RemoveAllListeners();
        dataEndGamePanel.ButtonEnd.onClick.AddListener(() => SceneManager.LoadScene("Menu"));
        base.ShowPanel();
    }

    public override void HidePanel()
    {
        dataEndGamePanel.ButtonEnd.gameObject.SetActive(false);
        base.HidePanel();
    }
}

