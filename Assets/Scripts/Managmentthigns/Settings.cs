using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    // this is the options manager
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

    bool ignoreNextMusicChange = false, ignoreNextSFXChange = false, ignoreNextSensitivityChange = false;

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

        SetStartStuff();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetVariables()
    {
        audioManager = AudioManager.instance;
    }


    public void SetStartStuff()
    {
        SetSliderOnStartSFX();
        SetSliderOnStartMusic();
        SetSliderOnStartController();

        isMutedMusic = audioManager.audioDataLocal.isMusicMuted;
        isMutedSFX = audioManager.audioDataLocal.isSFXMuted;
        SetBothImages();
    }


    // these set slider are called only by script
    public void SetSliderOnStartSFX()
    {
        // call this whenever this thing is set active (need to see if theres a set active start_
        // makes it  so the slier starts on the correct value when it is made
        ignoreNextSFXChange = true;
        volumeSliderSFX.value = audioManager.audioDataLocal.sfxVolume;
        ignoreNextSFXChange = false;
    }
    public void SetSliderOnStartMusic()
    {
        // call this whenever this thing is set active (need to see if theres a set active start_
        // makes it  so the slier starts on the correct value when it is made
      //  print("Set slide on music");
        ignoreNextMusicChange = true;
        volumeSliderMusic.value = audioManager.audioDataLocal.musicVolume;

        ignoreNextMusicChange = false;

    }
    public void SetSliderOnStartController()
    {
        // call this whenever this thing is set active (need to see if theres a set active start_
        // makes it  so the slier starts on the correct value when it is made
      // print("Set slide on controller");
        controllerSensitivitySlider.value = audioManager.audioDataLocal.controllerSensitivity;

    }


    // these change function only called by changing the value of hte sliders
    public void ChangeVolumeSFX(Slider slider)
    {
        // print((int)volumeSlider.value);
        if (ignoreNextSFXChange)
        {
            print("Blocked sfx");
            ignoreNextSFXChange = false;
            return;
        }
       // print("UnBlocked sfx");
        audioManager.SetVolumeSFX((int)slider.value);
        UnMuteSFX(); // just to remove the mute symbol
    }
    public void ChangeVolumeMusic(Slider slider)
    {
        // print((int)volumeSlider.value);
        if (ignoreNextMusicChange)
        {
            print("Blocked music");
            ignoreNextMusicChange = false;
            return;
        }
        //print("UnBlocked music");
        audioManager.SetVolumeMusic((int)slider.value);
        UnMuteMusic(); // just to remove the mute symbol
    }

    public void ChangeControllerSensitivity(Slider slider)
    {
        // print((int)volumeSlider.value);
        if (ignoreNextSensitivityChange)
        {
            ignoreNextSensitivityChange = false;
            return;
        }
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

    public void SetSFXImages()
    {
        if (isMutedSFX)
        {
            muteObjectSFX.SetActive(true);
            unMuteObjectSFX.SetActive(false);

        }
        else
        {
            muteObjectSFX.SetActive(false);
            unMuteObjectSFX.SetActive(true);
        }
    }

    public void MuteSFX(bool shouldMute = true)
    {
        isMutedSFX = true;
        // called by button
        if (shouldMute)
        {
            print("Muting it");
            audioManager.MuteVolumeSFX();
        }
        SetSFXImages();


    }

    public void UnMuteSFX(bool shouldMute = true)
    {
        isMutedSFX = false;
        // called by button
        if (shouldMute)
            audioManager.UnMuteVolumeSFX();
        SetSFXImages();

    }


    public void SetMusicImages()
    {
        if (isMutedMusic)
        {

            muteObjectMusic.SetActive(true);
            unMuteObjectMusic.SetActive(false);

        }
        else
        {
            muteObjectMusic.SetActive(false);
            unMuteObjectMusic.SetActive(true);
        }
    }
    public void MuteMusic()
    {
        isMutedMusic = true;
        // called by button
        audioManager.MuteVolumeMusic();
        SetMusicImages();
    }

    public void UnMuteMusic()
    {
        isMutedMusic = false;
        // called by button
        audioManager.UnMuteVolumeMusic();
        SetMusicImages();
    }

    public void UnMuteBothSounds() {
        muteObjectMusic.SetActive(false);
        unMuteObjectMusic.SetActive(true);
        muteObjectSFX.SetActive(false);
        unMuteObjectSFX.SetActive(true);
    }

    public void SetBothImages()
    {
        SetMusicImages();
        SetSFXImages();
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
