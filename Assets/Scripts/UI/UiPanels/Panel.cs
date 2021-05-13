using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DataPanel
{
    public UiPanelNames NamePanel;
    public GameObject PanelObject;
    [NonSerialized] public bool IsShown;
}

public abstract class Panel
{
    public Action<UiPanelNames> onSetShowPanel;
    public Action<UiPanelNames> onSetHidePanel;
    
    protected GameObject panelObject;
    protected DataPanel panel;

    public virtual void ShowPanel()
    {
        panelObject.SetActive(true);
        panel.IsShown = true;
    }

    public virtual void HidePanel()
    {
        panelObject.SetActive(false);
        panel.IsShown = false;
    }

    protected Panel(DataPanel panel)
    {
        this.panel = panel;
        this.panelObject = panel.PanelObject;
    }
}
