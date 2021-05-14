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
    [SerializeField] private DataRestartPanel dataRestartPanel;
    [SerializeField] private DataStatsPanel dataStatsPanel;
    [SerializeField] private DataSkillPanel dataSkillPanel;

    private Panel EndGamePanel;
    private Panel GamePanel;
    private Panel PausePanel;
    private Panel EndRoundPanel;
    private Panel RestartPanel;
    private Panel StatsPanel;
    private Panel SkillPanel;

    protected override void Start()
    {
        EndGamePanel = new EndGamePanel(dataEndGamePanel);
        GamePanel = new GamePanel(dataGamePanel);
        PausePanel = new PausePanel(dataPausePanel);
        EndRoundPanel = new EndRoundPanel(dataEndRoundPanel);
        RestartPanel = new RestartPanel(dataRestartPanel);
        StatsPanel = new StatsPanel(dataStatsPanel);
        SkillPanel = new SkillPanel(dataSkillPanel);
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
            {UiPanelNames.EndRoundPanel, EndRoundPanel},
            {UiPanelNames.RestartPanel, RestartPanel},
            {UiPanelNames.StatsPanel, StatsPanel},
            {UiPanelNames.SkillPanel, SkillPanel}
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
