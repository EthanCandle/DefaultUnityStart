using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public enum GameState { Platformer, Typing }
public class GameManager : MonoBehaviour
{
    public GameState gameStateCurrent = GameState.Platformer;

    public PlayerController playerController;

    public static GameManager instance;
    public InputManager _input;
    public PlayerInput playerInputScript;

    public SettingsManager settingsManagerScript;

    //public GameManager gm;  gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            print("Set gm instance");
            instance = this;
        }
        else
        {
            GetLocalScripts();
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        GetLocalScripts();
    }

    private void Start()
    {
        // only called by the first gm since the rest will die by this point
        GetGlobalScripts();
 
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
           // SwitchGames();
        }
    }

    public IEnumerator DelayFunctionCall(Action functionToCall)
    {
        yield return null;
        functionToCall?.Invoke();

    }
    public void GetGlobalScripts()
    {
        _input = InputManager.instance;

    }
    public void GetLocalScripts()
    {
        settingsManagerScript = GameObject.FindGameObjectWithTag("PlayerMenu").GetComponent<SettingsManager>();
        // playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

    }


    public void TurnOffPlayerMovement()
    {
        if(playerController == null)
        {
            return;
        }
        playerController.LosePlayerControl();
    }

    public void TurnOnPlayerMovement()
    {
        if (playerController == null)
        {
            return;
        }
        playerController.GainPlayerControl();
    }

    public void TurnOffPlayerJump()
    {
        playerController._input.jump = false;
    }
    public void TurnOffMouse()
    {
        _input.TurnOffMouse();
    }

    public void TurnOnMouse()
    {
        _input.TurnOnMouse();
    }

    public void TurnOffTime()
    {
        Time.timeScale = 0;
    }

    public void TurnOnTime()
    {
        Time.timeScale = 1;
    }

    public void TurnOffCameraControl()
    {
        playerController.LoseCameraControl();
    }
    public void TurnOnCameraControl()
    {
        playerController.GainCameraControl();
    }

    public void SetPausing(bool state)
    {
        settingsManagerScript.SetPausing(state);
    }

}
