using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Controller controller;
    [SerializeField] private UiManagerGame uiManagerGame;

    private List<Action> actionGameList = new List<Action>();
    private static int countAction;

    private void Start()
    {
        actionGameList = new List<Action>
        {
            EndRound,
            EndGame
        };

        controller.onOutPlayers += CallEndAction;
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnDestroy()
    {
        controller.onOutPlayers -= CallEndAction;
    }

    private void CallEndAction()
    {
        actionGameList[countAction]?.Invoke();
        countAction++;
    }

    private void OnSceneChanged(Scene current, Scene next)
    {
        if (next.name != "Game")
        {
            countAction = 0;
        }
    }
    
    private void EndRound()
    {
        uiManagerGame.ShowPanel(UiPanelNames.EndRoundPanel);
    }

    private void EndGame()
    {
        countAction = 0;
        uiManagerGame.ShowPanel(UiPanelNames.EndGamePanel);
    }
}
