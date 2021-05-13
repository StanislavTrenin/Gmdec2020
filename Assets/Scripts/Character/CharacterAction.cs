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
    
    private bool isNextField;

    private Character movableCharacter;
    
    private void Update()
    {
        Moving();
    }
    
    public void EnableMove(List<Vector2> characterPositions, FieldData fieldData)
    {
        if (characterPositions.Count < 2) return;
        characterPositions.RemoveAt(0);
        isNextField = true;
        
        this.fieldData = fieldData;
        movableCharacter = this.fieldData.ActiveCharacter;
        
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
        if (characterPositions.Count <= 0 || movableCharacter.currentStep >= countStepsCharacter)
        {
            if (movableCharacter != null && movableCharacter._isPlayer) {
                movableCharacter.controller.pathGeneratorVisual.ResetLinePath();
            }
            characterPositions.Clear();
            movableCharacter = null;
            return;
        }

        if (isNextField)
        {
            prevPosition = movableCharacter.transform.position;
            isNextField = false;
        }

        movableCharacter.transform.position = Vector2.MoveTowards(
            movableCharacter.transform.position, characterPositions[0], Time.deltaTime * speedCharacter);

        if (Vector2.Distance(movableCharacter.transform.position, characterPositions[0]) < 0.1f)
        {
            movableCharacter.transform.position = characterPositions[0];
            UpdateCharacterField(characterPositions[0], prevPosition);
            characterPositions.RemoveAt(0);
            isNextField = true;
            movableCharacter.currentStep++;
        }
    }

    private void UpdateCharacterField(Vector2 actualPosition, Vector2 prevPosition)
    {
        var direction = (actualPosition - prevPosition) / 4;
        movableCharacter.field = fieldData.Fields[
            movableCharacter.field.x + (int) direction.x, 
            movableCharacter.field.y - (int) direction.y];
        movableCharacter.field.character = fieldData.ActiveCharacter;
    }
}
