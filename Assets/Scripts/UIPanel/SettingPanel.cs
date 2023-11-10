using System;
using HorrorJam.Audio;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    AudioSetttingData audioSetttingData;

    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] Button backButton;

    public Action OnBack;
    

    void Start()
    {
        LoadMasterVolume();

        masterVolumeSlider.onValueChanged.AddListener(ChangeMasterVolume);
        musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(ChangeSFXVolume);
        backButton.onClick.AddListener(() => OnBack?.Invoke());
    }

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
}
