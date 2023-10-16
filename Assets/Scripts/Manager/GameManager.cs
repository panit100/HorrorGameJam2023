using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStage
{
    MainMenu,
    Playing,
    Pause,
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameStage gameStage = GameStage.MainMenu;

    public GameStage GameStage => gameStage;

    protected override void InitAfterAwake()
    {

    }

    public void SetGameStage(GameStage _gameStage)
    {
        gameStage = _gameStage;
    }
}
