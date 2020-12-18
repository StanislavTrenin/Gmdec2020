using System;
using System.Collections;
using System.Collections.Generic;
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

    private void VisualizeSelection(Field field)
    {
        
    }
}


public class FieldSelector
{
    private FieldData fieldData;
    private PathGenerator pathGenerator;

    public static Action<Field> onSelectField;
    
    public FieldSelector(FieldData fieldData, PathGenerator pathGenerator)
    {
        this.fieldData = fieldData;
        this.pathGenerator = pathGenerator;
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
                EnableMove(pathGenerator.LinePositions, fieldData);
            fieldData.PrevField = null;
        }
        else
        {
            pathGenerator.GenerateShortestPath(finishX, finishY, fieldData);
        }
    }
    
    private void CheckTypeField(int finishX, int finishY)
    {
        //pathGenerator.GenerateStraightPath(finishX, finishY, fieldData);
        //onSelectField?.Invoke(fieldData.Fields[finishX, finishY]);
    }
}
