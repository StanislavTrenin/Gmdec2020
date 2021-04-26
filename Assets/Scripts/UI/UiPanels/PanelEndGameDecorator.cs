using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public abstract class PanelEndGameDecorator : Panel
{
    protected DataEndGamePanel dataEndGamePanel;
    protected Panel panel;
    public PanelEndGameDecorator(Panel panel, DataEndGamePanel dataEndGamePanel): base(dataEndGamePanel)
    {
        this.dataEndGamePanel = dataEndGamePanel;
        this.panel = panel;
    }
}

