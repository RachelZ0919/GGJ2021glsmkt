using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
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
    public float wordShowInter = 0.05f;
    public bool disappearWhenEnd = true;
    public bool playOnStart = false;
    public GameObject[] dialogueObjects;

    [HideInInspector] public List<Dialogue> dialogues;

    private AudioSource audio;
    private Image raycastImage;
    private GameObject nextIcon;

    private Text text;
    private bool dialogueEnd;
    private int dialogueIndex;
    private int wordIndex;
    private float lastShowTime;
    private bool allEnd;

    public UnityEvent OnDialogueEnd = new UnityEvent();

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        text = GetComponentInChildren<Text>();
        raycastImage = GetComponent<Image>();
        nextIcon = transform.Find("NextIcon").gameObject;
    }

    private void Start()
    {
        for(int i = 0; i < dialogueObjects.Length; i++)
        {
            dialogueObjects[i].SetActive(false);
        }
        nextIcon.SetActive(false);

        dialogueEnd = true;
        raycastImage.raycastTarget = false;
        if (playOnStart) StartDialogue();
    }

    private void Update()
    {
        if(!dialogueEnd && Time.time - lastShowTime >= wordShowInter)
        {
            text.text += dialogues[dialogueIndex].content[wordIndex++];
            if (audio != null) audio.Play();

            if(wordIndex >= dialogues[dialogueIndex].content.Length)
            {
                nextIcon.SetActive(true);
                nextIcon.transform.parent = dialogueObjects[dialogues[dialogueIndex].setIndex].transform.Find("Next");
                nextIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
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
        if (allEnd)
        {
            return;
        }

        if (!dialogueEnd)
        {
            dialogueEnd = true;
            text.text = dialogues[dialogueIndex].content;
            nextIcon.SetActive(true);
            nextIcon.transform.parent = dialogueObjects[dialogues[dialogueIndex].setIndex].transform.Find("Next");
            nextIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        }
        else
        {
            if(dialogueIndex == dialogues.Count - 1)
            {
                raycastImage.raycastTarget = false;
                nextIcon.SetActive(false);

                if (disappearWhenEnd)
                {
                    int index = dialogues[dialogueIndex].setIndex;
                    dialogueObjects[index].SetActive(false);
                }

                OnDialogueEnd?.Invoke();
                allEnd = true;
            }
            else
            {
                //重设播放参数
                dialogueEnd = false;
                wordIndex = 0;
                lastShowTime = Time.time - wordShowInter;

                //关掉上一个object
                int lastIndex = dialogues[dialogueIndex].setIndex;
                dialogueObjects[lastIndex].SetActive(false);

                //更新下一个object
                dialogueIndex++;
                int index = dialogues[dialogueIndex].setIndex;
                dialogueObjects[index].SetActive(true);
                text = dialogueObjects[index].GetComponentInChildren<Text>();
                text.text = "";

                nextIcon.SetActive(false);
            }
        }
    }

    public void StartDialogue()
    {
        raycastImage.raycastTarget = true;
        dialogueEnd = false;

        dialogueIndex = wordIndex = 0;

        int index = dialogues[dialogueIndex].setIndex;
        dialogueObjects[index].SetActive(true);
        text = dialogueObjects[index].GetComponentInChildren<Text>();

        lastShowTime = Time.time - wordShowInter;
        text.text = "";

        allEnd = false;
    }
}
