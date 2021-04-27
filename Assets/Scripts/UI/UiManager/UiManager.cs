using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public abstract class UiManager : MonoBehaviour
{
    protected Dictionary<UiPanelNames, Panel> dataPanelDict = new Dictionary<UiPanelNames, Panel>();

    public Action<UiPanelNames> onShowPanel;
    public Action<UiPanelNames> onHidePanel;
    
    protected virtual void Start()
    {
        foreach (var data in dataPanelDict)
        {
            data.Value.onSetShowPanel += OnShowPanel;
            data.Value.onSetHidePanel += OnHidePanel;
        }
    }

    protected virtual void OnDestroy()
    {
        foreach (var data in dataPanelDict)
        {
            data.Value.onSetShowPanel -= OnShowPanel;
            data.Value.onSetHidePanel -= OnHidePanel;
        }
    }
    
    protected abstract void InitPanelDictionary();

    private void OnShowPanel(UiPanelNames uiPanelName)
    {
        if (dataPanelDict.ContainsKey(uiPanelName))
        {
            dataPanelDict[uiPanelName].ShowPanel();
            onShowPanel?.Invoke(uiPanelName);
        }
    }

    private void OnHidePanel(UiPanelNames uiPanelName)
    {
        if(dataPanelDict.ContainsKey(uiPanelName))
        {
            dataPanelDict[uiPanelName].HidePanel();
            onHidePanel?.Invoke(uiPanelName);
        }
    }
    
    public void ShowPanel(UiPanelNames uiPanelName)
    {
        if (dataPanelDict.ContainsKey(uiPanelName))
            dataPanelDict[uiPanelName].ShowPanel();
    }

    public void HidePanel(UiPanelNames uiPanelName)
    {
        if(dataPanelDict.ContainsKey(uiPanelName))
            dataPanelDict[uiPanelName].HidePanel();
    }
}
