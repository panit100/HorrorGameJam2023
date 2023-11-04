using UnityEngine;
using Sirenix.OdinInspector;

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
    

    void Start()
    {
        GameManager.Instance.OnChangeGameStage(GameStage.MainMenu);
        EnableMainMenu(true);
        EnableSetting(false);
        EnableHowToPlay(false);
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

    public void OnExit()
    {
        if(mainMenuStage != MainMenuStage.MainMenu)
            return;
        Application.Quit();
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
}
