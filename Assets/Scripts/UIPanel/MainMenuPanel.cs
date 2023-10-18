using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] Button startButton;
    [SerializeField] Button settingButton;
    [SerializeField] GameObject settingPanel;
    [SerializeField] Button howToPlayButton;
    [SerializeField] GameObject howToPlayPanel;
    [SerializeField] Button quitButton;

    private void Awake()
    {
        startButton.onClick.AddListener(OnClickStartButton);
        settingButton.onClick.AddListener(OnClickSettingButton);
        howToPlayButton.onClick.AddListener(OnClickHowToPlayButton);
        quitButton.onClick.AddListener(OnClickQuitButton);
    }

    void OnClickStartButton()
    {

    }

    void OnClickSettingButton()
    {
        settingPanel.SetActive(true);
    }

    void OnClickHowToPlayButton()
    {
        howToPlayPanel.SetActive(true);
    }
    void OnClickQuitButton()
    {
        Application.Quit();
    }
}
