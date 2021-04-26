using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DataGamePanel : DataPanel
{

}

public class GamePanel : Panel
{
    public GamePanel(DataGamePanel menuPanel) : base(menuPanel)
    {
    }
}

