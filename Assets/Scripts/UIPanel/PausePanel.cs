using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [SerializeField] Button resumeButton;
    [SerializeField] Button settingButton;
    [SerializeField] GameObject settingPanel;
    [SerializeField] Button quitButton;

    private void Awake()
    {
        resumeButton.onClick.AddListener(OnClickResumeButton);
        settingButton.onClick.AddListener(OnClickSettingButton);
        quitButton.onClick.AddListener(OnClickQuitButton);
        gameObject.SetActive(false);
    }

    void OnClickResumeButton()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    void OnClickSettingButton()
    {
        settingPanel.SetActive(true);
    }

    void OnClickQuitButton()
    {
        SceneController.Instance.OnLoadSceneAsync("Scene_MainMenu");
    }

    public void OnClickPauseButton()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }
}
