using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDataKeeper : MonoBehaviour
{
    [SerializeField] private TextDataObject textDataObject;
    
    public static Dictionary<string, string> TextDataDict = new Dictionary<string, string>();

    private void Awake()
    {
        TextDataDict.Clear();
        
        foreach (var textData in textDataObject.TextDataList)
        {
            TextDataDict.Add(textData.KeyText, textData.Text);
        }
    }
}
