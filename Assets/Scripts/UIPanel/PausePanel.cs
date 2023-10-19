using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    // [SerializeField] CanvasGroup pausePanel;
    // [SerializeField] SettingPanel settingPanel;
    
    [SerializeField] Button resumeButton;
    // [SerializeField] Button settingButton;
    [SerializeField] Button quitButton;

    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        resumeButton.onClick.AddListener(OnClickResumeButton);
        // settingButton.onClick.AddListener(OnClickSettingButton);
        quitButton.onClick.AddListener(OnClickQuitButton);
        EnablePausePanel(false);
    }

    void OnClickResumeButton()
    {
        GameManager.Instance.OnPause();
    }

    // void OnClickSettingButton()
    // {
    //     pausePanel.alpha = 0;
    //     settingPanel.alpha = 1;
    // }

    void OnClickQuitButton()
    {
        StartCoroutine(GameManager.Instance.GoToSceneMainMenu());
    }

    public void EnablePausePanel(bool enable)
    {
        if(enable)
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts =true;
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts =false;
        }
    }
}
