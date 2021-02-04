using UnityEngine;
using System.Collections;


public class LevelChangeTrigger : MonoBehaviour
{
    public int toLevel;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SceneLoader.instance.LoadScene(toLevel);
    }
}
