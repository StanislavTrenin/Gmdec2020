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
    private enum State
    {
        INIT,
        NO,
        YES,
        YES_NO,
        YES_YES
    }
    
    private DataEndRoundPanel dataEndRoundPanel;
    private State state;

    public EndRoundPanel(DataEndRoundPanel dataEndRoundPanel) : base(dataEndRoundPanel)
    {
        state = State.INIT;
        this.dataEndRoundPanel = dataEndRoundPanel;
        this.dataEndRoundPanel.ButtonNo.onClick.AddListener(No);
        this.dataEndRoundPanel.ButtonYes.onClick.AddListener(Yes);

        try
        {
            this.dataEndRoundPanel.TextEndRound.text = TextDataKeeper.TextDataDict?["Dialog1"];
        }
        catch (KeyNotFoundException)
        {
        }

    }

    private void Yes()
    {
        switch (state)
        {
            case State.INIT:
            {
                state = State.NO;
                dataEndRoundPanel.TextEndRound.text = TextDataKeeper.TextDataDict?["Dialog1No"];
                dataEndRoundPanel.ButtonNo.GetComponentInChildren<Text>().text = "Go";
                dataEndRoundPanel.ButtonYes.GetComponentInChildren<Text>().text = "Go";
                GameManager.currentBuff = GameManager.Buff.FAIL;
                break;
            }
            case State.YES:
            {
                state = State.YES_NO;
                dataEndRoundPanel.TextEndRound.text = TextDataKeeper.TextDataDict?["Dialog1YesNo"];
                dataEndRoundPanel.ButtonNo.GetComponentInChildren<Text>().text = "Go";
                dataEndRoundPanel.ButtonYes.GetComponentInChildren<Text>().text = "Go";
                GameManager.currentBuff = GameManager.Buff.WIN;
                break;
            }
            case State.NO:
            case State.YES_NO:
            case State.YES_YES:
            {
                SceneManager.LoadScene("Game");
                break;
            }
        }
    }

    private void No()
    {
        switch (state)
        {
            case State.INIT:
            {
                state = State.YES;
                dataEndRoundPanel.TextEndRound.text = TextDataKeeper.TextDataDict?["Dialog1Yes"];
                dataEndRoundPanel.ButtonYes.GetComponentInChildren<Text>().text = "1. Выстрелить в генератор";
                dataEndRoundPanel.ButtonNo.GetComponentInChildren<Text>().text = "2. Осмотреть помещение внимательнее";
                break;
            }
            case State.YES:
            {
                state = State.YES_YES;
                dataEndRoundPanel.TextEndRound.text = TextDataKeeper.TextDataDict?["Dialog1YesYes"];
                dataEndRoundPanel.ButtonNo.GetComponentInChildren<Text>().text = "Go";
                dataEndRoundPanel.ButtonYes.GetComponentInChildren<Text>().text = "Go";
                GameManager.currentBuff = GameManager.Buff.NO;
                break;
            }
            case State.NO:
            case State.YES_NO:
            case State.YES_YES:
            {
                SceneManager.LoadScene("Game");
                break;
            }
        }
    }

    public override void ShowPanel()
    {
        dataEndRoundPanel.ButtonYes.GetComponentInChildren<Text>().text = "1. Двинуться по центральному коридору";
        dataEndRoundPanel.ButtonYes.gameObject.SetActive(true);
        dataEndRoundPanel.ButtonNo.GetComponentInChildren<Text>().text = "2. Повернуть в боковой проход";
        dataEndRoundPanel.ButtonNo.gameObject.SetActive(true);
        if(TextDataKeeper.TextDataDict.ContainsKey("Dialog1"))
            dataEndRoundPanel.TextEndRound.text = TextDataKeeper.TextDataDict["Dialog1"];
        base.ShowPanel();
    }

    public override void HidePanel()
    {
        dataEndRoundPanel.ButtonNo.gameObject.SetActive(false);
        dataEndRoundPanel.ButtonYes.gameObject.SetActive(false);
        base.HidePanel();
    }
}