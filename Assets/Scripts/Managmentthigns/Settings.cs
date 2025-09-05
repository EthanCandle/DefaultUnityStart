using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static Settings instance;
    public bool isInOptions = false;
    public AudioManager audioManager;
    public int volume;
    public Slider volumeSliderSFX, volumeSliderMusic, controllerSensitivitySlider;

    public bool isMutedSFX = false, isMutedMusic = false, isMutedParticles, isMutedShaders, isMutedMainMenuCandy;
    public GameObject muteObjectSFX, unMuteObjectSFX, muteObjectMusic, unMuteObjectMusic;
    public GameObject muteObjectParticles, unMuteObjectParticles, muteObjectShaders, unMuteObjectShaders, muteObjectCandy, unMuteObjectCandy;
    public Animator optionsAnimator;
    public CanvasGroup menuToDeactiveOnSummon, canvasGroupLocal;
    public Sound summonSound, deSummonSound;
    public ReselectDefaultButton reselectButtonScript;

    public Button settingsDefaultButton;
    public GameManager gm;

    public Button quitButtonToSet;

    bool ignoreNextMusicChange = false, ignoreNextSFXChange = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);


    }
    // Start is called before the first frame update
    void Start()
    {

        audioManager = AudioManager.instance;

        if (audioManager.audioDataLocal.isMusicMuted)
        {
            print("Music muted");
            MuteMusic();
            audioManager.CallDelayFrameMusic();
        }
        if (audioManager.audioDataLocal.isSFXMuted)
        {
            print("Music muted");
            MuteSFX();
            audioManager.CallDelayFrameSFX();
        }

        CallDelayStartStuff();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetVariables()
    {
        audioManager = AudioManager.instance;
    }

    public void CallDelayStartStuff()
    {
        audioManager = AudioManager.instance;
        StartCoroutine(DelayStartStuff());
    }

    public IEnumerator DelayStartStuff()
    {
        yield return null;
        SetSliderOnStartSFX();
        SetSliderOnStartMusic();
        SetSliderOnStartController();
    }
    public void SetSliderOnStartSFX()
    {
        // call this whenever this thing is set active (need to see if theres a set active start_
        // makes it  so the slier starts on the correct value when it is made
        volumeSliderSFX.value = audioManager.audioDataLocal.sfxVolume;
    }
    public void SetSliderOnStartMusic()
    {
        // call this whenever this thing is set active (need to see if theres a set active start_
        // makes it  so the slier starts on the correct value when it is made
        print("Set slide on music");
        volumeSliderMusic.value = audioManager.audioDataLocal.musicVolume;
    }
    public void SetSliderOnStartController()
    {
        // call this whenever this thing is set active (need to see if theres a set active start_
        // makes it  so the slier starts on the correct value when it is made
        print("Set slide on controller");
        controllerSensitivitySlider.value = audioManager.audioDataLocal.controllerSensitivity;
    }

    public void ChangeVolumeSFX(Slider slider)
    {
        // print((int)volumeSlider.value);
        if (ignoreNextMusicChange)
        {

        }
        audioManager.SetVolumeSFX((int)slider.value);
        UnMuteSFX(); // just to remove the mute symbol
    }
    public void ChangeVolumeMusic(Slider slider)
    {
        // print((int)volumeSlider.value);

        audioManager.SetVolumeMusic((int)slider.value);
        UnMuteMusic(); // just to remove the mute symbol
    }

    public void ChangeControllerSensitivity(Slider slider)
    {
        // print((int)volumeSlider.value);

        audioManager.SetControllerSensitivity((float)slider.value);
    }

    public void ChangeMuteSFX()
    {
        if (isMutedSFX)
        {
            UnMuteSFX();
        }
        else
        {
            MuteSFX();
        }        
    }
    public void ChangeMuteMusic()
    {

        if (isMutedMusic)
        {
            UnMuteMusic();
        }
        else
        {
            MuteMusic();
        }
    }   

    public void MuteSFX()
    {
        isMutedSFX = true;
        // called by button
        audioManager.MuteVolumeSFX();

        if (muteObjectSFX != null)
            muteObjectSFX.SetActive(true);

        if (unMuteObjectSFX!= null)
            unMuteObjectSFX.SetActive(false);
    }

    public void UnMuteSFX()
    {
        isMutedSFX = false;
        // called by button
        audioManager.UnMuteVolumeSFX();
        muteObjectSFX.SetActive(false);
        unMuteObjectSFX.SetActive(true);

    }

    public void MuteMusic()
    {
        isMutedMusic = true;
        // called by button
        audioManager.MuteVolumeMusic();
        if(muteObjectMusic != null)
        muteObjectMusic.SetActive(true);

        if(unMuteObjectMusic != null)
        unMuteObjectMusic.SetActive(false);
    }

    public void UnMuteMusic()
    {
        isMutedMusic = false;
        // called by button
        audioManager.UnMuteVolumeMusic();
        muteObjectMusic.SetActive(false);
        unMuteObjectMusic.SetActive(true);

    }

    public void UnMuteBothSounds() {
        muteObjectMusic.SetActive(false);
        unMuteObjectMusic.SetActive(true);
        muteObjectSFX.SetActive(false);
        unMuteObjectSFX.SetActive(true);
    }



    public void ToggleOptionsMenu(Button button)
    {
        if (isInOptions)
        {
            DesummonOptionsMenu();
        }
        else
        {
            print("in toggle");
            //SummonOptionsMenu(button);
        }
    }

    // called by buttons, 
    public void SummonOptionsMenu(Button button, CanvasGroup cg)
    {
        if (!isInOptions)
        {
            optionsAnimator.SetTrigger("Move");
        }
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(summonSound);
        // called by options button
        isInOptions = true;
        optionsAnimator.SetTrigger("Move");
        menuToDeactiveOnSummon = cg;
        menuToDeactiveOnSummon.interactable = false;
        canvasGroupLocal.interactable = true;
       // ReselectButton();

        ReselectDefaultButton.instance.SetPreviousButton(button);
        ReselectDefaultButton.instance.SetButton(settingsDefaultButton);
    }

    public void ReselectButton()
    {
        StartCoroutine(DelayFrame());
    }

    public IEnumerator DelayFrame()
    {
        yield return null;
        reselectButtonScript.SelectRandomButton();
    }
    public void DesummonOptionsMenu()
    {
        if (isInOptions)
        {
            optionsAnimator.SetTrigger("Move");
        }
        print("options menu pause");
        FindObjectOfType<AudioManager>().PlaySoundInstantiate(deSummonSound);
        // called by back button
        isInOptions = false;

        menuToDeactiveOnSummon.interactable = true;
        canvasGroupLocal.interactable = false;

        ReselectDefaultButton.instance.GoBackToPreviousButton();
    }

    public void ResetSetting()
    {
        audioManager.DeleteData();
    }

}
