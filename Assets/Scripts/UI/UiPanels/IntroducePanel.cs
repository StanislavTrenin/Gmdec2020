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
    public UiTextVisual UiTextVisual;
}

public class IntroducePanel : Panel
{
    private DataIntroducePanel dataIntroduceDataPanel;

    public IntroducePanel(DataIntroducePanel dataIntroduceDataPanel) : base(dataIntroduceDataPanel)
    {
        this.dataIntroduceDataPanel = dataIntroduceDataPanel;
        this.dataIntroduceDataPanel.PlayButton.interactable = false;
        this.dataIntroduceDataPanel.PlayButton.onClick.AddListener(() => SceneManager.LoadScene("Game"));
    }

    public override void ShowPanel()
    {
        base.ShowPanel();
        dataIntroduceDataPanel.UiTextVisual.StartShowText(
            TextDataKeeper.TextDataDict["Introduce"], 
            dataIntroduceDataPanel.TextIntroduce, () =>
            {
                dataIntroduceDataPanel.PlayButton.interactable = true;
            });
    }
}