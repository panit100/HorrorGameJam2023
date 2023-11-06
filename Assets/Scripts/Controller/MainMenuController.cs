using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using HorrorJam.Audio;

public enum MainMenuStage
{
    MainMenu,
    Setting,
    HowToPlay
}

public class MainMenuController : MonoBehaviour
{
    MainMenuStage mainMenuStage = MainMenuStage.MainMenu;

    [SerializeField] CanvasGroup mainMenuGroup;
    [SerializeField] CanvasGroup settingGroup;
    [SerializeField] CanvasGroup howToPlayGroup;

    [Header("MainmenuPanel")]
    [SerializeField] Button mainMenu_startButton;
    [SerializeField] Button mainMenu_settingButton;
    [SerializeField] Button mainMenu_howToPlayButton;
    [SerializeField] Button mainMenu_quitButton;

    [Header("SettingPanel")]
    [SerializeField] Slider setting_volumeSlider;
    [SerializeField] Button setting_backButton;

    [Header("HowToPlayPanel")]
    [SerializeField] Button howToPlay_backButton;

    void Start()
    {
        AddListener();

        GameManager.Instance.OnChangeGameStage(GameStage.MainMenu);
        EnableMainMenu(true);
        EnableSetting(false);
        EnableHowToPlay(false);
    }

    void AddListener()
    {
        mainMenu_startButton.onClick.AddListener(() => {OnStartCutscene(); PlayButtonSound();});
        mainMenu_settingButton.onClick.AddListener(() => {OnSetting(); PlayButtonSound();});
        mainMenu_howToPlayButton.onClick.AddListener(() => {OnHowToPlay(); PlayButtonSound();});
        mainMenu_quitButton.onClick.AddListener(() => {OnQuit(); PlayButtonSound();});

        setting_volumeSlider.onValueChanged.AddListener((f) => {OnChangeVolume(f);});
        setting_backButton.onClick.AddListener(() => {OnBack(); PlayButtonSound();});
        
        howToPlay_backButton.onClick.AddListener(() => {OnBack(); PlayButtonSound();});
    }

    [Button]
    public void OnStartGame()
    {
        StartCoroutine(GameManager.Instance.GoToSceneGame());
    }

    public void OnStartCutscene()
    {
        StartCoroutine(GameManager.Instance.GoToCutscene("As_intro"));
    }

    public void OnBack()
    {
        mainMenuStage = MainMenuStage.MainMenu;
        EnableMainMenu(true);
        EnableSetting(false);
        EnableHowToPlay(false);
        
    }

    public void OnSetting()
    {
        mainMenuStage = MainMenuStage.Setting;
        EnableMainMenu(false);
        EnableSetting(true);
        EnableHowToPlay(false);
    }

    public void OnHowToPlay()
    {
        mainMenuStage = MainMenuStage.HowToPlay;
        EnableMainMenu(false);
        EnableSetting(false);
        EnableHowToPlay(true);
    }

    public void OnQuit()
    {
        if(mainMenuStage != MainMenuStage.MainMenu)
            return;
        Application.Quit();
    }

    void OnChangeVolume(float volume)
    {

    }

    void EnableMainMenu(bool enable)
    {
        if(enable)
        {
            mainMenuGroup.alpha = 1;
            mainMenuGroup.blocksRaycasts = true;
            mainMenuGroup.interactable = true;
        }
        else
        {
            mainMenuGroup.alpha = 0;
            mainMenuGroup.blocksRaycasts = false;
            mainMenuGroup.interactable = false;
            
        }
    }

    void EnableSetting(bool enable)
    {
        if(enable)
        {
            settingGroup.alpha = 1;
            settingGroup.blocksRaycasts = true;
            settingGroup.interactable = true;
        }
        else
        {
            settingGroup.alpha = 0;
            settingGroup.blocksRaycasts = false;
            settingGroup.interactable = false;
            
        }
    }

    void EnableHowToPlay(bool enable)
    {
        if(enable)
        {
            howToPlayGroup.alpha = 1;
            howToPlayGroup.blocksRaycasts = true;
            howToPlayGroup.interactable = true;
        }
        else
        {
            howToPlayGroup.alpha = 0;
            howToPlayGroup.blocksRaycasts = false;
            howToPlayGroup.interactable = false;
        }
    }

    void PlayButtonSound()
    {
        AudioManager.Instance.PlayOneShot("ui_click");
    }
}
