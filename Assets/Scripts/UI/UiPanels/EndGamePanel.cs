using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DataEndGamePanel : DataPanel
{

}

public class EndGamePanel : Panel
{
    private DataEndGamePanel dataEndGamePanel;
    public EndGamePanel(DataEndGamePanel dataEndGamePanel) : base(dataEndGamePanel)
    {
        try
        {

        }
        catch (System.NullReferenceException)
        {

        }
    }

    private void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}

