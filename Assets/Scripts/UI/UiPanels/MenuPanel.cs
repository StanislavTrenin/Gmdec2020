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
    private DataMenuPanel dataMenuPanel;

    public MenuPanel(DataMenuPanel dataMenuPanel) : base(dataMenuPanel)
    {
        this.dataMenuPanel = dataMenuPanel;
        
        this.dataMenuPanel.StartButton.onClick.AddListener(() => onSetShowPanel?.Invoke(UiPanelNames.IntroducePanel));
        this.dataMenuPanel.ExitButton.onClick.AddListener(() => Application.Quit());
    }
}

