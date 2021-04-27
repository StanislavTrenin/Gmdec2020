using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManagerGame : UiManager
{
    [Header("Дополнительные данные для панелек")] [SerializeField]
    private DataGamePanel dataGamePanel;

    [SerializeField] private DataEndGamePanel dataEndGamePanel;
    [SerializeField] private DataPausePanel dataPausePanel;
    [SerializeField] private DataEndRoundPanel dataEndRoundPanel;

    private Panel EndGamePanel;
    private Panel GamePanel;
    private Panel PausePanel;
    private Panel EndRoundPanel;

    protected override void Start()
    {
        EndGamePanel = new EndGamePanel(dataEndGamePanel);
        GamePanel = new GamePanel(dataGamePanel);
        PausePanel = new PausePanel(dataPausePanel);
        EndRoundPanel = new EndRoundPanel(dataEndRoundPanel);

        InitPanelDictionary();

        base.Start();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    private void Update()
    {
        SetVisiblePausePanel();
    }

    protected override void InitPanelDictionary()
    {
        dataPanelDict = new Dictionary<UiPanelNames, Panel>
        {
            {UiPanelNames.GamePanel, GamePanel},
            {UiPanelNames.EndGamePanel, EndGamePanel},
            {UiPanelNames.PausePanel, PausePanel},
            {UiPanelNames.EndRoundPanel, EndRoundPanel}
        };
    }

    private void SetVisiblePausePanel()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (dataPausePanel.IsShown == false)
            {
                PausePanel.ShowPanel();
            }
            else
            {
                PausePanel.HidePanel();
            }
        }
    }
}
