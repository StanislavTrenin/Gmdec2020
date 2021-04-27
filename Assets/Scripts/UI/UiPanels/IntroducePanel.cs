using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class DataIntroducePanel : DataPanel
{
    public Button PlayButton;
    public Text TextIntroduce;
}

public class IntroducePanel : Panel
{
    private DataIntroducePanel dataIntroduceDataPanel;

    public IntroducePanel(DataIntroducePanel dataIntroduceDataPanel) : base(dataIntroduceDataPanel)
    {
        this.dataIntroduceDataPanel = dataIntroduceDataPanel;

        try
        {
            this.dataIntroduceDataPanel.TextIntroduce.text = TextDataKeeper.TextDataDict["Introduce"];
        }
        catch (KeyNotFoundException)
        {
        }

        this.dataIntroduceDataPanel.PlayButton.onClick.AddListener(() => SceneManager.LoadScene("Game"));
    }
}