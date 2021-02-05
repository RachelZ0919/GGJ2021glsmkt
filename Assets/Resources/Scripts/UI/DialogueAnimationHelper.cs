using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAnimationHelper : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayDialogue(string dialogueName)
    {
        DialogueShower shower = GameObject.Find(dialogueName).GetComponent<DialogueShower>();
        shower.StartDialogue();
    }

    public void AnimationEnd()
    {
        animator.SetTrigger("AnimEnd");
    }

    public void GoToScene(int i)
    {
        SceneLoader.instance.LoadScene(i);
    }
}
