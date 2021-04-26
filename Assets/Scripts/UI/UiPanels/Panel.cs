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
}

public abstract class Panel
{
    public Action<UiPanelNames> onSetShowPanel;
    public Action<UiPanelNames> onSetHidePanel;
    
    protected GameObject panelObject;
    protected DataPanel dataPanel;
    
    public virtual void ShowPanel() => panelObject.SetActive(true);
    public virtual void HidePanel() => panelObject.SetActive(false);

    protected Panel(DataPanel dataPanel)
    {
        this.dataPanel = dataPanel;
        this.panelObject = dataPanel.PanelObject;
    }
}
