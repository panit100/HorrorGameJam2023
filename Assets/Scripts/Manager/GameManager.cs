using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum GameStage
{
    MainMenu,
    Playing,
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
        OnChangeGameStage(GameStage.Playing);
    }

    void OnChangeGameStage(GameStage _gameStage)
    {
        gameStage = _gameStage;

        switch(gameStage)
        {
            case GameStage.MainMenu:
                break;
            case GameStage.Playing:
                TimeManager.Instance.SetCurrentTime();
                break;
        }
    }

    public void OnPause()
    {
        isPause = true;
    }

    public void OnResume()
    {
        isPause = false;
    }
}
