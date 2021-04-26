using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManagerGame : UiManager
{
    [Header("Дополнительные данные для панелек")]
    [SerializeField] private DataGamePanel dataGamePanel;
    [SerializeField] private DataEndGamePanel dataEndGamePanel;

    public EndGamePanel EndGamePanel;
    public Panel GamePanel;
    
    protected override void Start()
    {
        EndGamePanel = new EndGamePanel(dataEndGamePanel);
        GamePanel = new GamePanel(dataGamePanel);

        InitPanelDictionary();
        
        base.Start();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected override void InitPanelDictionary()
    {
        dataPanelDict = new Dictionary<UiPanelNames, Panel>
        {
            {UiPanelNames.GamePanel, GamePanel},
            {UiPanelNames.EndGamePanel, EndGamePanel}
        };
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            ShowPanel(UiPanelNames.EndGamePanel);
        }
    }
}
