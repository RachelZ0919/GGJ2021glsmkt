using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;

    private GameObject pauseMenu;
    private AudioSource backgroundMusic;
    private bool isPlayingMusic;
    private float playTime;
    private bool gamePaused;

    public delegate void GamePause();
    public GamePause OnGamePause;
    public GamePause OnGameResume;

    public UnityEvent OnNarativeStart = new UnityEvent();
    public UnityEvent OnNarrativeEnd = new UnityEvent();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
            if (pauseMenu != null) pauseMenu.SetActive(false);
            GameObject bgmObject = GameObject.FindGameObjectWithTag("BGM");
            if (bgmObject != null) backgroundMusic = bgmObject.GetComponent<AudioSource>();
            gamePaused = false;
        }
        else
        {
            Destroy(this);
        }
    }


    public void LevelFailed()
    {
        SceneLoader.instance.LoadScene(4);
    }


    public void LevelSucceed()
    {
        OnNarativeStart?.Invoke();
    }

    public void ToEndScene()
    {
        OnNarrativeEnd?.Invoke();
    }

    private void StopGame()
    {
        if (pauseMenu != null) pauseMenu.SetActive(true);
        if (backgroundMusic != null)
        {
            isPlayingMusic = backgroundMusic.isPlaying;
            playTime = backgroundMusic.time;
            backgroundMusic.Stop();
        }
        OnGamePause?.Invoke();
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (backgroundMusic != null)
        {
            if (isPlayingMusic)
            {
                backgroundMusic.Play();
                backgroundMusic.time = playTime;
            }
        }
        OnGameResume?.Invoke();
        Time.timeScale = 1;
    }

    public void PauseOrResumeGame()
    {
        if (gamePaused)
        {
            ResumeGame();
        }
        else
        {
            StopGame();
        }
        gamePaused = !gamePaused;
    }

    public void ExitGame()
    {
        SceneLoader.instance.LoadScene(0);
    }

    public void RestartGame()
    {
        SceneLoader.instance.ReloadScene();
    }
}
