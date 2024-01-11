using System;
using System.Collections.Generic;
using System.Linq;
using HorrorJam.Audio;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Debug = FMOD.Debug;

public class SettingPanel : MonoBehaviour
{

    AudioSetttingData audioSetttingData;

    [Header("Audio Setting")] [TabGroup("Audio Setting")] [SerializeField]
    Slider masterVolumeSlider;

    [TabGroup("Audio Setting")] [SerializeField]
    Slider musicVolumeSlider;

    [TabGroup("Audio Setting")] [SerializeField]
    Slider sfxVolumeSlider;

    [TabGroup("Mouse Setting")] [SerializeField]
    Slider mouseSensitivitySlider;

    [TabGroup("Display Setting")] [SerializeField]
    TMP_Dropdown displayResolution;
    [SerializeField] TMP_Dropdown screenMode;
    [SerializeField] ScreenLineLoading applyBlock;
    [SerializeField] Button_MainMenu applybutton;
    [SerializeField] private Toggle vsyncboolean;
    private List<Resolution> resolutionsList = new List<Resolution>();
    private List<FullScreenMode> fullscreenmodeList = new List<FullScreenMode>();
    private List<String> fullscreenmodeListTxt = new List<string>();
    private List<String> resolutionListTxt = new List<string>();


    [PropertySpace] [TabGroup("HeaderBar Button Manager")] [field: SerializeField]
    List<SettingButtonEvent> settingButtons = new List<SettingButtonEvent>();

    [SerializeField] Button_MainMenu backButton;

    public Action OnBack;

    private Dictionary<SettingButtonEvent, UnityEvent<string>> buttonDict =
        new Dictionary<SettingButtonEvent, UnityEvent<string>>();


    private void Awake()
    {
        InitScreenSetting();
    }

    void Start()
    {
        LoadMasterVolume();
        InitListener();
        SetUpSettingButton();
        
        mouseSensitivitySlider.onValueChanged?.Invoke(mouseSensitivitySlider.value);
        OnChangePanel(settingButtons[0].eventName);
    }

    void InitListener()
    {
        masterVolumeSlider.onValueChanged.AddListener(ChangeMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(ChangeSFXVolume);
        mouseSensitivitySlider.onValueChanged.AddListener(MouseSensitivtyUpdate);
        backButton.AddListener(() => OnBack?.Invoke());
        applybutton.AddListener(ApplyButton);
    }

    void InitScreenSetting()
    {
        screenMode.ClearOptions();
        var oof = Enum.GetValues(typeof(FullScreenMode));
        fullscreenmodeList.AddRange(oof.OfType<FullScreenMode>().ToList() );
        fullscreenmodeListTxt = fullscreenmodeList.Select(i => i.ToString()).ToList();
        screenMode.AddOptions(fullscreenmodeListTxt);
        screenMode.value = fullscreenmodeList.FindIndex(x => x == Screen.fullScreenMode);
        
        
        resolutionsList.AddRange(Screen.resolutions);
        resolutionsList = resolutionsList.Where(x => x.refreshRateRatio.ToString() == Screen.currentResolution.refreshRateRatio.ToString() ).ToList();
        resolutionListTxt = resolutionsList.Select(i =>i.width.ToString() + "x" + i.height.ToString() ).ToList();
        displayResolution.ClearOptions();
        displayResolution.AddOptions(resolutionListTxt);
        displayResolution.value = resolutionsList.FindIndex(x =>
            (x.height == Screen.currentResolution.height && x.width == Screen.currentResolution.width));
      
        QualitySettings.vSyncCount =  0;
        displayResolution.onValueChanged.AddListener((int temp)=>OnresChanged(displayResolution.value)); 
        screenMode.onValueChanged.AddListener((int temp2)=>OnscreenModeChanged(screenMode.value)); 
    }

    public void RefreshSetting()
    {
        applyBlock.InstantFill(1);
        displayResolution.value = resolutionsList.FindIndex(x =>
            (x.height == Screen.currentResolution.height && x.width == Screen.currentResolution.width));
        screenMode.value = fullscreenmodeList.FindIndex(x => x == Screen.fullScreenMode);
        QualitySettings.vSyncCount =  QualitySettings.vSyncCount;
    }
    

    private bool resChanged, screenModeChanged;
    public void OnresChanged(int value)
    {
        resChanged = resolutionsList[displayResolution.value].ToString() != Screen.currentResolution.ToString();
        if (resChanged||screenModeChanged)
        {
         applyBlock.FillLine(0);
         applyBlock.GetComponent<Image>().raycastTarget = false;
        }
        else
        {
            applyBlock.GetComponent<Image>().raycastTarget = true;
            applyBlock.InstantFill(1);
        }
        
    }

    public void OnscreenModeChanged(int value)
    {
        screenModeChanged = fullscreenmodeList[screenMode.value].ToString() != Screen.fullScreenMode.ToString();
        if (resChanged || screenModeChanged)
        {
            applyBlock.FillLine(0);
            applyBlock.GetComponent<Image>().raycastTarget = false;
        }
        else
        {
            applyBlock.GetComponent<Image>().raycastTarget = true;
            applyBlock.InstantFill(1);
        }
    }

    public void ApplyButton()
    {
        Screen.SetResolution(resolutionsList[displayResolution.value].width,resolutionsList[displayResolution.value].height,fullscreenmodeList[screenMode.value]);
       
    }

    public void VsyncToggle()
    {
        int vsyncCount = vsyncboolean.isOn ? 1 : 0;
        QualitySettings.vSyncCount = vsyncCount;
    }

    void SetUpSettingButton()
    {
        foreach (var VARIABLE in settingButtons)
        {
            buttonDict.Add(VARIABLE,VARIABLE.buttonEvent);
            VARIABLE.button.AddEvent(VARIABLE.buttonEvent);
            VARIABLE.button.SetButtonState(true);
        }
        settingButtons[0].button.SetButtonState(false);
    }
    
    
    #region SFX_SETTING
    void ChangeMasterVolume(float value)
    {
        audioSetttingData.masterVolume = value;
        AudioManager.Instance.ChangeMasterVolume(audioSetttingData.masterVolume);
        SaveMasterVolume();
    }
    
    void ChangeMusicVolume(float value)
    {
        audioSetttingData.musicVolume = value;
        AudioManager.Instance.ChangeMusicVolume(audioSetttingData.musicVolume);
        SaveMasterVolume();
    }

    void ChangeSFXVolume(float value)
    {
        audioSetttingData.sfxVolume = value;
        AudioManager.Instance.ChangeSFXVolume(audioSetttingData.sfxVolume);
        SaveMasterVolume();
    }

    void SaveMasterVolume()
    {
        audioSetttingData.Save();
    }

    void LoadMasterVolume()
    {
        audioSetttingData = AudioSetttingData.Load();

        SetSaveVolumeToSlider();
    }

    void SetSaveVolumeToSlider()
    {
        if(audioSetttingData == null)
        {
            masterVolumeSlider.value = 1f;
            musicVolumeSlider.value = 1f;
            sfxVolumeSlider.value = 1f;
        }
        else
        {
            masterVolumeSlider.value = audioSetttingData.masterVolume;
            musicVolumeSlider.value = audioSetttingData.musicVolume;
            sfxVolumeSlider.value = audioSetttingData.sfxVolume;
        }
    }
    

    #endregion
    
    #region MouseSensitivity_Setting

    public void OnChangePanel(String buttonName)
    {
        foreach (var VARIABLE in settingButtons)
        {
            if (VARIABLE.eventName == buttonName)
            {
                VARIABLE.button.SetButtonState(false);
                VARIABLE.currentPanel.interactable = true;
                VARIABLE.currentPanel.alpha = 1;
                VARIABLE.currentPanel.blocksRaycasts = true;
            }
            else
            {
                VARIABLE.button.SetButtonState(true);
                VARIABLE.currentPanel.interactable = false;
                VARIABLE.currentPanel.alpha = 0;
                VARIABLE.currentPanel.blocksRaycasts = false;
            }
           
        }
    }
    public void MouseSensitivtyUpdate(float value)
    {
        MouseSettingController.Instance.SetMouseSen(value);
    }

    #endregion
}
