using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GameStage
{
    MainMenu,
    Playing,
    GameOver,
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameStage gameStage = GameStage.MainMenu;

    public GameStage GameStage => gameStage;

    bool isPause = false;
    public bool IsPause => isPause;
    protected override void InitAfterAwake()
    {

    }

    void Start()
    {
        AddInputListener();
        OnChangeGameStage(GameStage.Playing);
    }

    void AddInputListener()
    {
        InputSystemManager.Instance.onPause += OnPause;
        InputSystemManager.Instance.onPause += OnResume;
    }

    void RemoveInputListener()
    {
        InputSystemManager.Instance.onPause -= OnPause;
        InputSystemManager.Instance.onPause -= OnResume;
    }

    public void OnChangeGameStage(GameStage _gameStage)
    {
        gameStage = _gameStage;

        switch(gameStage)
        {
            case GameStage.MainMenu:
                InputSystemManager.Instance.TogglePlayerControl(false);
                InputSystemManager.Instance.ToggleUIControl(true);
                InputSystemManager.Instance.ToggleInGameControl(false);
                break;
            case GameStage.Playing:
                TimeManager.Instance.SetCurrentTime();

                InputSystemManager.Instance.TogglePlayerControl(true);
                InputSystemManager.Instance.ToggleUIControl(false);
                InputSystemManager.Instance.ToggleInGameControl(true);
                break;
            case GameStage.GameOver:
                InputSystemManager.Instance.TogglePlayerControl(false);
                InputSystemManager.Instance.ToggleUIControl(true);
                InputSystemManager.Instance.ToggleInGameControl(false);

                //TODO: Play Die Cutscene
                break;
        }
    }

    public void OnPause()
    {
        if(isPause)
            return;

        isPause = true;
    }

    public void OnResume()
    {
        if(!isPause)
            return;

        isPause = false;
    }

    public void OnDie()
    {
        Debug.LogWarning("Game Over!!! Noob!");
        isPause = true;
        OnChangeGameStage(GameStage.GameOver);
    }

    void OnDestroy() 
    {
        RemoveInputListener();
    }
}
