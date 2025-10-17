using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class SettingsManager : MonoBehaviour
{
    public GameObject pauseMenu;

    public GameManager gm; 
    public InputManager _input;
    public Settings settingsScript;

    public ConfirmationPopUpManager popUpManager;
    public bool isPaused = false, canPause = false;

    public MainMenu mainMenuScript;
    public Sound summonSound, deSummonSound;


    public Button pauseMenuDefaultButton;
    public bool unPaused;



    public CanvasGroup canvasGroup;

    //public static SettingsManager instance;
    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //        return;
    //    }
    //    DontDestroyOnLoad(gameObject);

    //}

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.instance;
        _input = InputManager.instance;
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        // pause input or if we are already paused and they press back button)
        if (InputManager.instance.pause || (isPaused && _input.goBack))
        {
            return;
            if (!canPause)
            {
                return;
            }
            gm._input.pause = false;
            gm._input.goBack = false;

            // if in pause menu then remove it
            if (settingsScript != null && settingsScript.isInOptions)
            {
                settingsScript.DesummonOptionsMenu();
            }
            // else remove the pause menu
            else
            {
                ChangePauseMenuState();
            }

        }

    }

    public void ChangePauseMenuState()
    {
        if (!isPaused)
        {
            OpenPauseMenu();
            isPaused = true;
        }
        else if (mainMenuScript.DestroyAreYouSure())
        {

        }
        else if (Settings.instance.isInOptions)
        {
            Settings.instance.DesummonOptionsMenu();
        }
        else
        {

            ClosePauseMenu();
            isPaused = false;

        }
    }

    public void SummonOptionsMenu(Button button)
    {
        Settings.instance.SummonOptionsMenu(button, canvasGroup);
    }


    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
        gm.TurnOnMouse();
        gm.TurnOffTime();
        gm.TurnOffPlayerMovement();

        FindObjectOfType<AudioManager>().PlaySoundInstantiate(summonSound);

        ReselectDefaultButton.instance.SetPreviousButton(pauseMenuDefaultButton);
        ReselectDefaultButton.instance.SetButton(pauseMenuDefaultButton);
        ReselectDefaultButton.instance.OpenedMenuPausedButtons();
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        gm.TurnOffMouse();
        gm.TurnOnTime();
        gm.TurnOnPlayerMovement();

        print("closed pause menu");
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(deSummonSound);
        StartCoroutine(DelayFrame());

        // causes button selector to stop
        ReselectDefaultButton.instance.ClosedMenuGoToGamePlay();
    }

    public void PlayerOnPauseMenuOpen()
    {
        // what game specific things need to happen when player pauses


    }

    public void PlayerOnPauseMenuClosed()
    {

    }

    public IEnumerator DelayFrame()
    {
        unPaused = true;
        yield return null;
        print("closed pause menu null");
        unPaused = false;
    }

    public void SetPausing(bool state)
    {
        canPause = state;

    }

    public void TurnOnPausing()
    {
        SetPausing(true);
    }

    public void TurnOffPausing()
    {
        SetPausing(false);
    }

}

