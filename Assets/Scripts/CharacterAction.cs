using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : MonoBehaviour
{
    [SerializeField][Range(1, 10)] private int speedCharacter;
    
    private bool isNextField;
    private Vector2 fieldSize;
    private Vector2 prevPosition;
    private Character character;
    private List<Vector2> characterPositions = new List<Vector2>();
    
    private void Update()
    {
        Moving();
    }
    
    public void EnableMove(Character character, List<Vector2> characterPositions, Vector2 fieldSize)
    {
        isNextField = true;
        
        this.fieldSize = fieldSize;
        this.character = character;
        this.characterPositions = characterPositions;
        
        Debug.Log(characterPositions.Count);
    }

    private void Moving()
    {
        if(characterPositions.Count <= 0) return;
        
        if (isNextField)
        {
            prevPosition = character.transform.position;
            isNextField = false;
        }

        character.transform.position = Vector2.MoveTowards(
            character.transform.position, characterPositions[0], Time.deltaTime * speedCharacter);

        if (Vector2.Distance(character.transform.position, characterPositions[0]) < 0.1f)
        {
            character.transform.position = characterPositions[0];
            UpdateCharacterField(characterPositions[0], prevPosition);
            characterPositions.RemoveAt(0);
            isNextField = true;
        }
    }

    private void UpdateCharacterField(Vector2 actualPosition, Vector2 prevPosition)
    {
        var direction = (actualPosition - prevPosition) / 4;
        character.field.x += (int) direction.x;
        character.field.y -= (int) direction.y;
    }
}
