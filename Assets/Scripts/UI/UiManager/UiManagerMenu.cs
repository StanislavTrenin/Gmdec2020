using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManagerMenu : UiManager
{
    [SerializeField] private DataMenuPanel dataMenuPanel;
    
    public Panel MenuPanel;

    protected override void Start()
    {
        MenuPanel = new MenuPanel(dataMenuPanel);
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
            {UiPanelNames.MenuPanel, MenuPanel}
        };
    }
}
