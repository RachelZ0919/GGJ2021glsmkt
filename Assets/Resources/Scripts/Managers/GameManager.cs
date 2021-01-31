using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }


    public void LevelFailed()
    {
        SceneLoader.instance.LoadScene(2);
    }


    public void LevelSucceed()
    {
        SceneLoader.instance.LoadScene(3);
    }

}
