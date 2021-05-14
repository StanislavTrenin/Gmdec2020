using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBlockActionsVisual : MonoBehaviour
{
    [SerializeField] private GameObject endTurnObject;
    public List<GameObject> SkillButtonList = new List<GameObject>();
    
    private void Start()
    {
        CharacterAction.onMove += SetEndTurnButton;
    }

    private void OnDestroy()
    {
        CharacterAction.onMove -= SetEndTurnButton;
    }

    private void SetEndTurnButton(bool isPlayerBusy)
    {
        endTurnObject.SetActive(!isPlayerBusy);
        SkillButtonList.ForEach(x => x.SetActive(!isPlayerBusy));
    }
}
