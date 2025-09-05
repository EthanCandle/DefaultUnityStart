using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public LevelTransitionManager levelTransition;
    public PlayerDebugStatsGlobalManager playerStatManager;

    public GameObject confirmationPopup, levelSelectObject;
    public CanvasGroup mainMenuCG, pauseMenuCG;


    public Settings settingsScript;

    public bool isInOptions, isInDebugStore, isInLevelSelect;

    public InputManager _input;
    public ConfirmationPopUP areYouSureScript;
    public ConfirmationPopUpManager popUpManager;

    public ReselectDefaultButton reselectButtonScript;

    public Sound summonSound, deSummonSound;

    public Button levelSelectDefaultButton, mainMenuDefaultButton;

    public Button optionsButton;

    private void Start()
    {
        levelTransition = LevelTransitionManager.instance;
        playerStatManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<PlayerDebugStatsGlobalManager>();
        reselectButtonScript = gameObject.GetComponent<ReselectDefaultButton>();
    }


    public void PlayGameNew () // new game
    {
        ReselectDefaultButton.instance.ClosedMenuGoToGamePlay();

        // change this to be 
        levelTransition.MoveToDifferentLevel(1);
    }

    public void PlayGameContinue()
    {
        ReselectDefaultButton.instance.ClosedMenuGoToGamePlay();

        // change this to be 
        levelTransition.MoveToDifferentLevel(PlayerDebugStatsGlobalManager.Instance.DataGetLevelCount());
    }

    public void ResetLevel(Button button)
    {
        // called by pause button

        // if in main run do nothing
        SpawnConfirmationPopup(ResetLevelLogic, pauseMenuCG, button);
    }

    public void ResetLevelLogic()
    {
        levelTransition.MoveToDifferentLevel(SceneManager.GetActiveScene().buildIndex);
    }
    public void BackToMainMenu(Button button)
    {
        // called by pause menu button
        // called by the button
        SpawnConfirmationPopup(BackToMainMenuLogic, pauseMenuCG, button);
    }

    public void BackToMainMenuLogic()
    {
        levelTransition.MoveToDifferentLevel(0);
    }

    public void PlayLevel(int levelToPlay)
    {
        ReselectDefaultButton.instance.ClosedMenuGoToGamePlay();

        // called by pause menu button
        levelTransition.MoveToDifferentLevel(levelToPlay);
    }



    public void QuitGame (Button button)
    {
        // called by the button
        SpawnConfirmationPopup(QuitGameLogic, mainMenuCG, button);
    }

    public void QuitGameLogic()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }


    public void SpawnConfirmationPopup(Action onYes, CanvasGroup canvasGroup, Button button)
    {
        // action is the function itself
        GameObject popUp = Instantiate(confirmationPopup, confirmationPopup.transform.position, confirmationPopup.transform.rotation);
        ConfirmationPopUP popUpScript = popUp.GetComponent<ConfirmationPopUP>();
        popUpScript.Show(onYes, canvasGroup,reselectButtonScript);
        ReselectDefaultButton.instance.SetPreviousButton(button);
        areYouSureScript = popUpScript;
    }


    // Update is called once per frame
    void Update()
    {
        if(_input == null)
        {
            return;
        }
        if (_input.pause || _input.goBack)
        {
            _input.pause = false;
            _input.goBack = false;

            if(DestroyAreYouSure())
            {
               
            }
            else if (settingsScript != null && settingsScript.isInOptions)
            {
                settingsScript.DesummonOptionsMenu();
            }


        }
    }

    public bool DestroyAreYouSure()
    {
        if (areYouSureScript != null)
        {
            areYouSureScript.CloseOnNo();
            return true;
        }
        return false;
    }

    public void SummonOptionsMenu(Button button)
    {
        Settings.instance.SummonOptionsMenu(button, mainMenuCG);
    }

    public void DeleteMainData(Button button)
    {
        print("Delete data");
        SpawnConfirmationPopup(DeleteMainDataLogic, mainMenuCG, button);
    }

    public void DeleteMainDataLogic()
    {
        print("Yes Delete data");
    }
}
