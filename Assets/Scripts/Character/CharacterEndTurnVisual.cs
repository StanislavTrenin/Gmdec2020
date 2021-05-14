using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterEndTurnVisual : MonoBehaviour
{
    [SerializeField] private Button endTurnButton;

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
        endTurnButton.interactable = !isPlayerBusy;
    }
}
