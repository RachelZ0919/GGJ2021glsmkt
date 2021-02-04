using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

[Serializable]
public struct Dialogue
{
    public int setIndex;
    public string content;
}

public class DialogueShower : MonoBehaviour
{
    public DialogueSetData setData;
    public float wordShowInter = 0.05f;

    [HideInInspector] public List<Dialogue> dialogues;

    private AudioSource audio;
    private Text text;
    private GameObject dialogueObject;
    private Image raycastImage;
    private Image backgroundImage;
    
    private bool dialogueEnd;
    private int dialogueIndex;
    private int wordIndex;
    private float lastShowTime;

    public delegate void DialogueEnd();
    public DialogueEnd OnDialogueEnd;
    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        text = GetComponentInChildren<Text>();
        dialogueObject = transform.GetChild(0).gameObject;
        raycastImage = GetComponent<Image>();
        backgroundImage = dialogueObject.GetComponentInChildren<Image>();
    }

    private void Start()
    {
        dialogueEnd = true;
        raycastImage.raycastTarget = false;
        dialogueObject.SetActive(false);
        StartDialogue();
    }

    private void Update()
    {
        if(!dialogueEnd && Time.time - lastShowTime >= wordShowInter)
        {
            text.text += dialogues[dialogueIndex].content[wordIndex++];
            if (audio != null) audio.Play();

            if(wordIndex >= dialogues[dialogueIndex].content.Length)
            {
                dialogueEnd = true;
            }
            else
            {
                lastShowTime = Time.time;
            }
        }
    }

    public void NextDialogue()
    {
        if (!dialogueEnd)
        {
            dialogueEnd = true;
            text.text = dialogues[dialogueIndex].content;
        }
        else
        {
            if(dialogueIndex == dialogues.Count - 1)
            {
                raycastImage.raycastTarget = false;
                dialogueObject.SetActive(false);
                OnDialogueEnd?.Invoke();
            }
            else
            {
                dialogueEnd = false;
                wordIndex = 0;
                text.text = "";
                lastShowTime = Time.time - wordShowInter;
                dialogueIndex++;

                int index = dialogues[dialogueIndex].setIndex;
                backgroundImage.sprite = setData.dialogueSets[index].backgroundImage;
                text.color = setData.dialogueSets[index].textColor;
            }
        }
    }

    public void StartDialogue()
    {
        raycastImage.raycastTarget = true;
        dialogueObject.SetActive(true);
        dialogueEnd = false;

        dialogueIndex = wordIndex = 0;

        int index = dialogues[dialogueIndex].setIndex;
        backgroundImage.sprite = setData.dialogueSets[index].backgroundImage;
        text.color = setData.dialogueSets[index].textColor;

        lastShowTime = Time.time - wordShowInter;
        text.text = "";
    }
}
