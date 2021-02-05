using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(DialogueShower))]
public class DialogueShowerEditor : Editor
{
    private int dialogueCount;
    private List<bool> foldOut;
    private string[] dialogueNames;

    private void OnEnable()
    {
        DialogueShower shower = (DialogueShower)target;
        foldOut = new List<bool>();
        for(int i = 0; i < shower.dialogues.Count; i++)
        {
            foldOut.Add(false);
        }
        dialogueCount = shower.dialogues.Count;
        SetDictionary();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10);
        if(GUILayout.Button("Update Dialogue Set"))
        {
            SetDictionary();
        }

        GUILayout.Space(10);
        GUILayout.Label("Dialogue Setting");

        DialogueShower shower = (DialogueShower)target;
        if(shower.dialogueObjects == null || shower.dialogueObjects.Length == 0)
        {
            EditorGUILayout.HelpBox("First Set Dialogue Objects!", MessageType.Info);
        }
        else
        {
            dialogueCount = EditorGUILayout.IntField("Dialogue Count", dialogueCount);
            UpdateDialogueList();
            
            for(int i = 0; i < shower.dialogues.Count; i++)
            {
                foldOut[i] = EditorGUILayout.Foldout(foldOut[i], $"Dialogue {i}");
                if (foldOut[i])
                {
                    Dialogue dialogue = shower.dialogues[i];

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Speaker");
                    dialogue.setIndex = EditorGUILayout.Popup(shower.dialogues[i].setIndex, dialogueNames);
                    dialogue.setIndex = Mathf.Clamp(dialogue.setIndex, 0, shower.dialogueObjects.Length - 1);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Content");
                    dialogue.content = EditorGUILayout.TextField(dialogue.content);
                    GUILayout.EndHorizontal();

                    shower.dialogues[i] = dialogue;
                }
            }
        }

    }

    private void UpdateDialogueList()
    {
        DialogueShower shower = (DialogueShower)target;

        dialogueCount = Mathf.Max(dialogueCount, 0);

        if(shower.dialogues == null)
        {
            shower.dialogues = new List<Dialogue>();
        }

        if(foldOut == null)
        {
            foldOut = new List<bool>();
        }

        if (dialogueCount > shower.dialogues.Count)
        {
            int addCount = dialogueCount - shower.dialogues.Count;
            for(int i = 0; i < addCount; i++)
            {
                shower.dialogues.Add(new Dialogue());
                foldOut.Add(false);
            }
        }else if(dialogueCount < shower.dialogues.Count)
        {
            while(shower.dialogues.Count > dialogueCount)
            {
                shower.dialogues.RemoveAt(shower.dialogues.Count - 1);
                foldOut.RemoveAt(shower.dialogues.Count - 1);
            }
        }
    }

    private void SetDictionary()
    { 
        DialogueShower shower = (DialogueShower)target;
        if (shower.dialogueObjects == null) return;

        dialogueNames = new string[shower.dialogueObjects.Length];
        for (int i = 0; i < shower.dialogueObjects.Length; i++)
        {
            dialogueNames[i] = shower.dialogueObjects[i].name;
        }
    }
}
