using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DataMenuPanel : DataPanel
{
    public Button StartButton;
    public Button ExitButton;
}

public class MenuPanel : Panel
{
    private DataMenuPanel dataMenuDataPanel;

    public MenuPanel(DataMenuPanel dataMenuDataPanel) : base(dataMenuDataPanel)
    {
        this.dataMenuDataPanel = dataMenuDataPanel;
        
        this.dataMenuDataPanel.StartButton.onClick.AddListener(() => onSetShowPanel?.Invoke(UiPanelNames.IntroducePanel));
        this.dataMenuDataPanel.ExitButton.onClick.AddListener(Application.Quit);
    }
}

