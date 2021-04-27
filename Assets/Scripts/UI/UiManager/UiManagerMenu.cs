using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManagerMenu : UiManager
{
    [SerializeField] private DataMenuPanel dataMenuPanel;
    [SerializeField] private DataIntroducePanel dataIntroducePanel;
    
    private Panel MenuPanel;
    private Panel IntroducePanel;

    protected override void Start()
    {
        MenuPanel = new MenuPanel(dataMenuPanel);
        IntroducePanel = new IntroducePanel(dataIntroducePanel);
        
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
            {UiPanelNames.MenuPanel, MenuPanel},
            {UiPanelNames.IntroducePanel, IntroducePanel},
        };
    }
}
