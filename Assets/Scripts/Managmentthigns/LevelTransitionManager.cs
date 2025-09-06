using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionManager : MonoBehaviour
{
    public bool shouldPlayTransition = true;
    public GameObject levelTransitionUIHolder; // thing that holds all of the ui othertahn the canvas tiself
    public Animator levelTransAnimator;
    public Sound goingInSFX, goingOutSFX, test;

    public AudioManager audioManager;

    public static LevelTransitionManager instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            instance.StartGame();
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

    }

    // Start is called before the first frame update
    void Start()
    {
        audioManager = AudioManager.instance;
        //print("Playing test sound");

        if (audioManager.inMainMenuFirstTime)
        {
            DoNotPlayTransition();
            audioManager.FadeIn();
            audioManager.inMainMenuFirstTime = false;
            return;
        }

        StartGame();

    }

    // Update is called once per frame
    void Update()
    {
      //  print(audioManager.gm.fm.debugStore.mainMenuScript.settingManagerScript.allowedToPause);

        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            if (HTMLPlatformUtil.IsEditor())
            {
                EndGame();
            }
            //   
        }
    }
    
    public void DoNotPlayTransition()
    {
        UnPauseTime();
        levelTransitionUIHolder.SetActive(false);
    }

    public void PlayTransitionOut()
    {
        print("Going Out");
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(goingOutSFX);
        PauseTime();
        levelTransitionUIHolder.SetActive(true);
        levelTransAnimator.Play("GoingOut");
        audioManager.FadeIn();

        float duration = 1;

        foreach (var clip in levelTransAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == "GoingOut")
            {
                duration = clip.length;
                break;
            }
        }

        SetSettingAllowedToPause(false); 
        StartCoroutine(DelayForStartingGame(duration, true));
    }

    public void PlayTransitionIn(int levelNum = 0)
    {

        print("Going in");
        PauseTime();
        levelTransitionUIHolder.SetActive(true);
        levelTransAnimator.Play("GoingIn");
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(goingInSFX);  
        audioManager.FadeOut();

        float duration = 1;

        foreach (var clip in levelTransAnimator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == "GoingIn")
            {
                duration = clip.length;
                break;
            }
        }
        StartCoroutine(WaitToLoadNextScene(duration, levelNum));
        SetSettingAllowedToPause(false);
    }

    IEnumerator WaitToLoadNextScene(float timeToWait, int levelToGoTo)
    {

        yield return new WaitForSecondsRealtime(timeToWait);
        LoadLevel(levelToGoTo);
    }

    IEnumerator DelayForStartingGame(float timeToWait, bool state)
    {
        
        yield return new WaitForSecondsRealtime(timeToWait);
        SetSettingAllowedToPause(state);
    }

    public void SetSettingAllowedToPause(bool state)
    {
        //print("Is gm null?");
        if(GameManager.instance == null)
        {
            print("GM IS NULL");

        }
       // print($"Pausing state: {state}");
        GameManager.instance.SetPausing(state);
    }

    public void StartGame()
    {
        PlayTransitionOut();
    }

    public void EndGame()
    {
        print("End game");
        // set level time

        // called when hitting a end trigger by player so its at the last level

        audioManager.gm.playerController.HasWonLevel();


        PlayTransitionIn(SceneManager.GetActiveScene().buildIndex + 1);


    }

    public void MoveToDifferentLevel(int levelNum)
    {
        PlayTransitionIn(levelNum);


    }

    public void UnPauseTime()
    {
        Time.timeScale = 1;
    }

    public void PauseTime()
    {
        Time.timeScale = 0;
    }

    public void LoadLevel(int levelNum)
    {

        SceneManager.LoadScene(levelNum);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
