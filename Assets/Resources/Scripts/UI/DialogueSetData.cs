using UnityEngine;
using System;

[Serializable]
public struct DialogueSet
{
    public string speakerName;
    public Sprite backgroundImage;
    public Color textColor;
}
public class DialogueSetData : ScriptableObject
{
    public DialogueSet[] dialogueSets;
 }
