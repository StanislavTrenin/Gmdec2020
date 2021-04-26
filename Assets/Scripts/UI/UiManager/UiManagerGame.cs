using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManagerGame : UiManager
{
    [Header("Дополнительные данные для панелек")]
    [SerializeField] private DataGamePanel dataGamePanel;
    [SerializeField] private DataEndGamePanel dataEndGamePanel;
    [SerializeField] private DataLosePanel dataLosePanel;
    [SerializeField] private DataWinPanel dataWinPanel;

    public Panel WinPanel;
    public Panel LosePanel;
    public Panel GamePanel;
    
    protected override void Start()
    {
        Panel endGamePanel = new EndGamePanel(dataEndGamePanel);
        GamePanel = new GamePanel(dataGamePanel);
        LosePanel = new LosePanel(endGamePanel, dataEndGamePanel, dataLosePanel);
        WinPanel = new WinPanel(endGamePanel, dataEndGamePanel, dataWinPanel);
       
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
            {UiPanelNames.LosePanel, LosePanel},
            {UiPanelNames.WinPanel, WinPanel}
        };
    }
}
