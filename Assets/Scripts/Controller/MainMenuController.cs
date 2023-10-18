using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum MainMenuStage
{
    MainMenu,
    Setting,
}

public class MainMenuController : MonoBehaviour
{
    MainMenuStage mainMenuStage = MainMenuStage.MainMenu;

    void Start()
    {
        GameManager.Instance.OnChangeGameStage(GameStage.MainMenu);
    }

    public void OnStartGame()
    {
        if(mainMenuStage != MainMenuStage.MainMenu)
            return;

        SceneController.Instance.GoToSceneGame();
    }

    public void OnBack()
    {
        if(mainMenuStage != MainMenuStage.Setting)
            return;

        mainMenuStage = MainMenuStage.MainMenu;
    }

    public void OnSetting()
    {
        if(mainMenuStage != MainMenuStage.MainMenu)
            return;

        mainMenuStage = MainMenuStage.Setting;
    }

    public void OnExit()
    {
        if(mainMenuStage != MainMenuStage.MainMenu)
            return;
    }
}
