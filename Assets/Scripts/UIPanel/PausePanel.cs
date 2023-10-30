using System.Collections;
using System.Collections.Generic;
using Hellmade.Sound;
using HorrorJam.Audio;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [Header("Canvas Group")]
    [SerializeField] CanvasGroup pausePanelCanvas;
    [SerializeField] CanvasGroup settingPanelCanvas;
    
    [Header("Button")]
    [SerializeField] Button resumeButton;
    [SerializeField] Button settingButton;
    [SerializeField] Button quitButton;

    CanvasGroup canvasGroup;
    SettingPanel settingPanel;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        settingPanel = GetComponentInChildren<SettingPanel>();
    }

    void Start()
    {
        GameManager.Instance.onPause += OnPause;
        settingPanel.OnBack += OpenPausePanel;

        resumeButton.onClick.AddListener(OnClickResumeButton);
        settingButton.onClick.AddListener(OnClickSettingButton);
        quitButton.onClick.AddListener(OnClickQuitButton);

        EnablePausePanel(false);
    }

    void OnClickResumeButton()
    {
        AudioManager.Instance.PlayOneShot("clickUI");
        EnablePausePanel(false);
        GameManager.Instance.OnPause();
    }

    void OnClickSettingButton()
    {
        AudioManager.Instance.PlayOneShot("clickUI");
        OpenSettingPanel();
    }

    void OnClickQuitButton()
    {
        AudioManager.Instance.PlayOneShot("clickUI");
        StartCoroutine(GameManager.Instance.GoToSceneMainMenu());
    }

    void EnablePausePanel(bool enable)
    {
        if(enable)
        {
            AudioManager.Instance.PlayOneShot("windowEntry");

            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts =true;

            OpenPausePanel();
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts =false;

            CloseAllPanel();
        }
    }

    void OnPause()
    {
        EnablePausePanel(true);
    }

    void OpenPausePanel()
    {
        pausePanelCanvas.alpha = 1;
        pausePanelCanvas.blocksRaycasts = true;

        settingPanelCanvas.alpha = 0;
        settingPanelCanvas.blocksRaycasts = false;
    }

    void OpenSettingPanel()
    {
        pausePanelCanvas.alpha = 0;
        pausePanelCanvas.blocksRaycasts = false;
        
        settingPanelCanvas.alpha = 1;
        settingPanelCanvas.blocksRaycasts = true;
    }

    void CloseAllPanel()
    {
        AudioManager.Instance.PlayOneShot("windowEntry");
        
        pausePanelCanvas.alpha = 0;
        pausePanelCanvas.blocksRaycasts = false;
        
        settingPanelCanvas.alpha = 0;
        settingPanelCanvas.blocksRaycasts = false;
    }
}
