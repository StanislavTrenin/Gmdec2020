using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DataEndRoundPanel : DataPanel
{
    public Button ButtonYes;
    public Button ButtonNo;
    public Text TextEndRound;
}

public class EndRoundPanel : Panel
{
    private DataEndRoundPanel dataEndRoundPanel;

    public EndRoundPanel(DataEndRoundPanel dataEndRoundPanel) : base(dataEndRoundPanel)
    {
        this.dataEndRoundPanel = dataEndRoundPanel;
        this.dataEndRoundPanel.ButtonNo.onClick.AddListener(() => SceneManager.LoadScene("Game"));
        this.dataEndRoundPanel.ButtonYes.onClick.AddListener(() => SceneManager.LoadScene("Game"));

        try
        {
            this.dataEndRoundPanel.TextEndRound.text = TextDataKeeper.TextDataDict?["EndRound"];
        }
        catch (KeyNotFoundException)
        {
        }

    }

    public override void ShowPanel()
    {
        dataEndRoundPanel.ButtonNo.gameObject.SetActive(true);
        dataEndRoundPanel.ButtonYes.gameObject.SetActive(true);
        base.ShowPanel();
    }

    public override void HidePanel()
    {
        dataEndRoundPanel.ButtonNo.gameObject.SetActive(false);
        dataEndRoundPanel.ButtonYes.gameObject.SetActive(false);
        base.HidePanel();
    }
}