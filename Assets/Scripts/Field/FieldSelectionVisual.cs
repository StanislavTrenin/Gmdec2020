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
    public Controller controller;
    
    public FieldSelector(FieldData fieldData, PathGeneratorVisual pathGeneratorVisual)
    {
        this.fieldData = fieldData;
        this.pathGeneratorVisual = pathGeneratorVisual;
    }
    
    public void OnSelectField(int finishX, int finishY, PointerEventData.InputButton inputButton, bool isAi)
    {
        if (fieldData.ActiveCharacter.stunnedSteps > 0) return;
        switch (inputButton)
        {
            case PointerEventData.InputButton.Left:
                if (fieldData.ActiveSkill == null)
                {
                    MoveToField(finishX, finishY, isAi);
                }
                else
                {
                    ApplySkillToField(finishX, finishY);
                }
                break;
            case PointerEventData.InputButton.Right:
                CheckTypeField(finishX, finishY);
                break;
        }

        fieldData.PrevField = fieldData.Fields[finishX, finishY];
    }

    private void ApplySkillToField(int x, int y)
    {
        Skill skill = fieldData.ActiveSkill;
        Field field = fieldData.Fields[x, y];
        Field currentField = fieldData.ActiveCharacter.field;
        fieldData.ActiveSkill = null;
        if (field.character == null) return;

        if (!skill.canApply(currentField.character, field.character)) return;

        pathGeneratorVisual.PathStraightGenerator.GeneratePath(x, y, fieldData);
        if (pathGeneratorVisual.PathStraightGenerator.FieldVisibilityType == FieldVisibilityType.NoVisible) return;

        if (Math.Sqrt(Math.Pow(field.x - currentField.x, 2) + Math.Pow(field.y - currentField.y, 2)) >
            fieldData.ActiveCharacter.stats.range) return;

        skill.Apply(field.character);
    }

    private void MoveToField(int finishX, int finishY, bool isAi)
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

            if (isAi)
            {
                fieldData.PrevField = fieldData.Fields[finishX, finishY];
                MoveToField(finishX, finishY, isAi);
            }
        }
    }
    
    public void CheckTypeField(int finishX, int finishY)
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
