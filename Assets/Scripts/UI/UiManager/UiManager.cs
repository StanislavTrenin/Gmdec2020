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
    
    protected virtual void Start()
    {
        foreach (var data in dataPanelDict)
        {
            data.Value.onSetShowPanel += ShowPanel;
            data.Value.onSetHidePanel += HidePanel;
        }
    }

    protected virtual void OnDestroy()
    {
        foreach (var data in dataPanelDict)
        {
            data.Value.onSetShowPanel -= ShowPanel;
            data.Value.onSetHidePanel -= HidePanel;
        }
    }
    
    protected abstract void InitPanelDictionary();
    

    public void ShowPanel(UiPanelNames uiPanelName)
    {
        if(dataPanelDict.ContainsKey(uiPanelName))
            dataPanelDict[uiPanelName].ShowPanel();
    }

    public void HidePanel(UiPanelNames uiPanelName)
    {
        if(dataPanelDict.ContainsKey(uiPanelName))
            dataPanelDict[uiPanelName].HidePanel();
    }
}
