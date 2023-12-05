using System;
using System.Collections.Generic;
using HorrorJam.Audio;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Debug = FMOD.Debug;

public class SettingPanel : MonoBehaviour
{
    
    AudioSetttingData audioSetttingData;
    [Header("Audio Setting")]
    [TabGroup("Audio Setting")]
    [SerializeField] Slider masterVolumeSlider;
    [TabGroup("Audio Setting")]
    [SerializeField] Slider musicVolumeSlider;
    [TabGroup("Audio Setting")]
    [SerializeField] Slider sfxVolumeSlider;
    
    [TabGroup("Mouse Setting")]
    [SerializeField] Slider mouseSensitivitySlider;
    
    [PropertySpace] [TabGroup("HeaderBar Button Manager")] 
    [field :SerializeField] List<SettingButtonEvent> settingButtons = new List<SettingButtonEvent>(); 
    
    [SerializeField] Button backButton;

    public Action OnBack;

    private Dictionary<SettingButtonEvent, UnityEvent<string>> buttonDict =
        new Dictionary<SettingButtonEvent,UnityEvent<string>>();
    

    void Start()
    {
        LoadMasterVolume();
        SetUpSettingButton();
        
        masterVolumeSlider.onValueChanged.AddListener(ChangeMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(ChangeSFXVolume);
        mouseSensitivitySlider.onValueChanged.AddListener(MouseSensitivtyUpdate);
        backButton.onClick.AddListener(() => OnBack?.Invoke());
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
