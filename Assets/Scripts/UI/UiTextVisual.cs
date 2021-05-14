using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UiTextVisual : MonoBehaviour
{
    [SerializeField] private int maxSymbols;
    [SerializeField] private float delayChanging;
    private Action onShowedText;
    private Queue<string> textQueue = new Queue<string>();
    private Text textObject;
    
    public void StartShowText(string text, Text textObject, Action onShowedText)
    {
        this.onShowedText = onShowedText;
        this.textObject = textObject;
        ProcessText(text);
        ChangeText();
    }
    
    private void ProcessText(string text)
    {
        var textArray = text.Split('.');
        textQueue.Clear();
        
        foreach (var textSentence in textArray)
        {
            textQueue.Enqueue(textSentence);
        }
    }

    private void ChangeText()
    {
        StartCoroutine(TextChanging());
    }

    private IEnumerator TextChanging()
    {
        StringBuilder textBuilder = new StringBuilder();
        var colorAlpha = 1f;
        
        while (textQueue.Count > 0)
        {
            textBuilder.Append(textQueue.Dequeue());
           
            if (textBuilder.ToString().Length > maxSymbols)
            {
                textObject.text = textBuilder.ToString();
                colorAlpha = 0;
            
                while (textObject.color.a < 1)
                {
                    textObject.color = new Color(1, 1, 1, colorAlpha);
                    colorAlpha += 0.05f;
                    yield return new WaitForEndOfFrame();
                }

                yield return new WaitForSeconds(delayChanging);

                if (textQueue.Count > 0)
                {
                    colorAlpha = 1;

                    while (textObject.color.a > 0)
                    {
                        textObject.color = new Color(1, 1, 1, colorAlpha);
                        colorAlpha -= 0.05f;
                        yield return new WaitForEndOfFrame();
                    }
                }

                textBuilder.Clear();
            }

            yield return null;
        }
        onShowedText?.Invoke();
    }
}
