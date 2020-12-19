using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FieldSelectionVisual : MonoBehaviour
{
    [SerializeField] private Text textTypeSelection;

    private void Start()
    {
        FieldSelector.onSelectField += VisualizeSelection;
    }

    private void OnDestroy()
    {
        FieldSelector.onSelectField -= VisualizeSelection;
    }

    private void VisualizeSelection(string text)
    {
        textTypeSelection.text = text;
    }
}


public class FieldSelector
{
    private FieldData fieldData;
    private PathGeneratorVisual pathGeneratorVisual;
    public static Action<string> onSelectField;
    
    public FieldSelector(FieldData fieldData, PathGeneratorVisual pathGeneratorVisual)
    {
        this.fieldData = fieldData;
        this.pathGeneratorVisual = pathGeneratorVisual;
    }
    
    public void OnSelectField(int finishX, int finishY, PointerEventData.InputButton inputButton)
    {
        switch (inputButton)
        {
            case PointerEventData.InputButton.Left:
                ActionOnField(finishX, finishY);
                break;
            case PointerEventData.InputButton.Right:
                CheckTypeField(finishX, finishY);
                break;
        }

        fieldData.PrevField = fieldData.Fields[finishX, finishY];
    }

    private void ActionOnField(int finishX, int finishY)
    {
        if (fieldData.PrevField == fieldData.Fields[finishX, finishY])
        {
            fieldData.ActiveCharacter.CharacterAction.
                EnableMove(pathGeneratorVisual.LinePositions, fieldData);
            fieldData.PrevField = null;
        }
        else
        {
            pathGeneratorVisual.PathShortestGenerator.GeneratePath(finishX, finishY, fieldData);
        }
    }
    
    private void CheckTypeField(int finishX, int finishY)
    {
        pathGeneratorVisual.PathStraightGenerator.GeneratePath(finishX, finishY, fieldData);

        switch (pathGeneratorVisual.PathStraightGenerator.FieldVisibilityType)
        {
            case FieldVisibilityType.NoVisible:
                onSelectField?.Invoke("Can't See");
                break;
            case FieldVisibilityType.PartiallyVisible:
                onSelectField?.Invoke("See in cover");
                break;
            default:
                onSelectField?.Invoke("See");
                break;
        }

    }
}
