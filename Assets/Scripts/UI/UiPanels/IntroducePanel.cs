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
    private DataIntroducePanel dataIntroducePanel;

    public IntroducePanel(DataIntroducePanel dataIntroducePanel) : base(dataIntroducePanel)
    {
        this.dataIntroducePanel = dataIntroducePanel;
        if(TextDataKeeper.TextDataDict.ContainsKey("IntroduceText"))
            this.dataIntroducePanel.TextIntroduce.text = TextDataKeeper.TextDataDict["IntroduceText"];
        this.dataIntroducePanel.PlayButton.onClick.AddListener(() => SceneManager.LoadScene("Game"));
    }
}