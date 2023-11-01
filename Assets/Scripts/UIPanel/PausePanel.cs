using HorrorJam.Audio;
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
        settingPanel = settingPanelCanvas.GetComponent<SettingPanel>();
    }

    void Start()
    {
        AddListener();

        EnablePauseCanvas(false);
    }

    void AddListener()
    {
        GameManager.Instance.onPause += OnPause;
        settingPanel.OnBack += OpenPausePanel;

        resumeButton.onClick.AddListener(OnClickResumeButton);
        settingButton.onClick.AddListener(OnClickSettingButton);
        quitButton.onClick.AddListener(OnClickQuitButton);
    }

    void RemoveListener()
    {
        GameManager.Instance.onPause -= OnPause;
        settingPanel.OnBack -= OpenPausePanel;

        resumeButton.onClick.RemoveListener(OnClickResumeButton);
        settingButton.onClick.RemoveListener(OnClickSettingButton);
        quitButton.onClick.RemoveListener(OnClickQuitButton);
    }

    void OnClickResumeButton()
    {
        AudioManager.Instance.PlayOneShot("clickUI");
        EnablePauseCanvas(false);
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

    //Show PauseCanvas
    void EnablePauseCanvas(bool enable)
    {
        if(enable)
        {
            AudioManager.Instance.PlayOneShot("windowEntry");

            canvasGroup.alpha = 1;
            canvasGroup.interactable =true;
            canvasGroup.blocksRaycasts = true;

            OpenPausePanel();
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable =false;
            canvasGroup.blocksRaycasts = false;

            CloseAllPanel();
        }
    }

    //Show pause panel
    void OpenPausePanel()
    {
        TogglePausePanel(true);
        ToggleSettingPanel(false);
    }

    //Show Setting panel
    void OpenSettingPanel()
    {
        TogglePausePanel(false);
        ToggleSettingPanel(true);
    }

    //Hide All panal when pause
    void CloseAllPanel()
    {
        AudioManager.Instance.PlayOneShot("windowEntry");
        
        TogglePausePanel(false);
        ToggleSettingPanel(false);
    }

    //Show/Hide Pause Panel in game when pause
    void TogglePausePanel(bool toggle)
    {
        if(toggle)
        {
            pausePanelCanvas.alpha = 1;
            pausePanelCanvas.interactable = true;
            pausePanelCanvas.blocksRaycasts = true;
        }
        else
        {
            pausePanelCanvas.alpha = 0;
            pausePanelCanvas.interactable = false;
            pausePanelCanvas.blocksRaycasts = false;
        }
    }

    //Show/Hide Pause Panel in game when pause
    void ToggleSettingPanel(bool toggle)
    {
        if(toggle)
        {
            settingPanelCanvas.alpha = 1;
            settingPanelCanvas.interactable = true;
            settingPanelCanvas.blocksRaycasts = true;
        }
        else
        {
            settingPanelCanvas.alpha = 0;
            settingPanelCanvas.interactable = false;
            settingPanelCanvas.blocksRaycasts = false;
        }
    }
    
    void OnPause()
    {
        EnablePauseCanvas(true);
    }

    void OnDestroy()
    {
        RemoveListener();
    }
}
