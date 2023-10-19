using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    // [SerializeField] Button backButton;
    // [SerializeField] Slider masterVolumeSlider;

    // CanvasGroup canvasGroup;
    // private void Awake()
    // {
    //     canvasGroup = GetComponent<CanvasGroup>();

    //     backButton.onClick.AddListener(OnClickBackButton);
    //     masterVolumeSlider.onValueChanged.AddListener(delegate { ChangeMasterVolume(); });
    //     SetMasterVoulme();
    //     gameObject.SetActive(false);
    // }
    // void OnClickBackButton()
    // {
    //     gameObject.SetActive(false);
    // }
    
    // void SetMasterVoulme()
    // {
    //     if (!PlayerPrefs.HasKey("HorrorGameJamMasterVolume"))
    //     {
    //         PlayerPrefs.SetFloat("HorrorGameJamMasterVolume", 0.5f);
    //         LoadMasterVolume();
    //     }
    //     else
    //     {
    //         LoadMasterVolume();
    //     }
    // }

    // void ChangeMasterVolume()
    // {
    //     AudioListener.volume = masterVolumeSlider.value;
    //     SaveMasterVolume();
    // }
    // void SaveMasterVolume()
    // {
    //     PlayerPrefs.SetFloat("HorrorGameJamMasterVolume", masterVolumeSlider.value);
    // }
    // void LoadMasterVolume()
    // {
    //     PlayerPrefs.GetFloat("HorrorGameJamMasterVolume");
    //     masterVolumeSlider.value = PlayerPrefs.GetFloat("HorrorGameJamMasterVolume");
    // }
}
