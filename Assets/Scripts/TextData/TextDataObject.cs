using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TextData
{
    public string KeyText;
    public string Text;
}


[CreateAssetMenu(fileName = "TextData", menuName = "ScriptableObjects/TextData")]
public class TextDataObject : ScriptableObject
{
    [SerializeField] public List<TextData> TextDataList = new List<TextData>();
}
