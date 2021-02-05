using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstInScene : MonoBehaviour
{
    static public bool firstInScene = true;

    private void Awake()
    {
        if (firstInScene)
        {
            firstInScene = false;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
