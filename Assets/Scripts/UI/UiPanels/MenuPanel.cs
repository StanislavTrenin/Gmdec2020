using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DataMenuPanel : DataPanel
{

}

public class MenuPanel : Panel
{
    private DataMenuPanel dataMenuPanel;

    public MenuPanel(DataMenuPanel dataMenuPanel) : base(dataMenuPanel)
    {
        this.dataMenuPanel = dataMenuPanel;
    }
    
}

