using System;
using HorrorJam.Audio;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Button backButton;

    public Action OnBack;

    void Start()
    {
        // backButton.onClick.AddListener(() => {OnBack?.Invoke(); AudioManager.Instance.PlayOneShot("clickUI");});
        masterVolumeSlider.onValueChanged.AddListener(ChangeMasterVolume);
        LoadMasterVolume();
    }

    void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
        SaveMasterVolume();
    }

    void SaveMasterVolume()
    {
        PlayerPrefs.SetFloat("HorrorGameJamMasterVolume", masterVolumeSlider.value);
    }

    void LoadMasterVolume()
    {
        PlayerPrefs.GetFloat("HorrorGameJamMasterVolume");
        masterVolumeSlider.value = PlayerPrefs.GetFloat("HorrorGameJamMasterVolume",0.5f);
    }
}
