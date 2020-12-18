using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterAction : MonoBehaviour
{
    [SerializeField][Range(1, 10)] private int speedCharacter;
    [SerializeField] private int countStepsCharacter;
    
    private FieldData fieldData;
    private Vector2 prevPosition;
    private List<Vector2> characterPositions = new List<Vector2>();
    
    private int currentStep;
    private bool isNextField;
    
    private void Update()
    {
        Moving();
    }
    
    public void EnableMove(List<Vector2> characterPositions, FieldData fieldData)
    {
        currentStep = 0;
        isNextField = true;

        this.fieldData = fieldData;
        
        try
        {
            if (this.characterPositions.Count <= 0 ||
                this.characterPositions[this.characterPositions.Count - 1] !=
                characterPositions[characterPositions.Count - 1])
            {
                this.characterPositions = characterPositions;
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            
        }
    }

    private void Moving()
    {
        if(characterPositions.Count <= 0 || currentStep >= countStepsCharacter) return;
        
        if (isNextField)
        {
            prevPosition = fieldData.ActiveCharacter.transform.position;
            isNextField = false;
        }

        fieldData.ActiveCharacter.transform.position = Vector2.MoveTowards(
            fieldData.ActiveCharacter.transform.position, characterPositions[0], Time.deltaTime * speedCharacter);

        if (Vector2.Distance(fieldData.ActiveCharacter.transform.position, characterPositions[0]) < 0.1f)
        {
            fieldData.ActiveCharacter.transform.position = characterPositions[0];
            UpdateCharacterField(characterPositions[0], prevPosition);
            characterPositions.RemoveAt(0);
            isNextField = true;
            currentStep++;
        }
    }

    private void UpdateCharacterField(Vector2 actualPosition, Vector2 prevPosition)
    {
        var direction = (actualPosition - prevPosition) / 4;
        fieldData.ActiveCharacter.field = fieldData.Fields[
                fieldData.ActiveCharacter.field.x + (int) direction.x, 
                fieldData.ActiveCharacter.field.y - (int) direction.y];
        fieldData.ActiveCharacter.field.character = fieldData.ActiveCharacter;
    }
}
