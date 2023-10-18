using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public enum GameStage
{
    MainMenu,
    Playing,
    Pause,
    GameOver,
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameStage gameStage = GameStage.MainMenu;

    public GameStage GameStage => gameStage;

    [Indent,SerializeField,ReadOnly] bool isPause = false;
    public bool IsPause => isPause;
    protected override void InitAfterAwake()
    {

    }

    void Start()
    {
        AddInputListener();
        OnChangeGameStage(GameStage.Playing); //TODO: Remove when finish game
    }

    void AddInputListener()
    {
        InputSystemManager.Instance.onPause += OnPause;
    }

    void RemoveInputListener()
    {
        InputSystemManager.Instance.onPause -= OnPause;
    }

    public void OnChangeGameStage(GameStage _gameStage)
    {
        print(_gameStage.ToString());

        gameStage = _gameStage;

        switch(gameStage)
        {
            case GameStage.MainMenu:
                LockCursor(false);
                InputSystemManager.Instance.TogglePlayerControl(false);
                InputSystemManager.Instance.ToggleUIControl(true);
                InputSystemManager.Instance.ToggleInGameControl(false);
                break;
            case GameStage.Playing:
                LockCursor(true);
                //TODO: Run Cutscene
                TimeManager.Instance.SetCurrentTime();

                InputSystemManager.Instance.TogglePlayerControl(true);
                InputSystemManager.Instance.ToggleUIControl(false);
                InputSystemManager.Instance.ToggleInGameControl(true);
                break;
            case GameStage.Pause:
                isPause = true;
                LockCursor(false);

                InputSystemManager.Instance.TogglePlayerControl(false);
                InputSystemManager.Instance.ToggleUIControl(true);
                InputSystemManager.Instance.ToggleInGameControl(true);
                break;
            case GameStage.GameOver:
                LockCursor(false);
                InputSystemManager.Instance.TogglePlayerControl(false);
                InputSystemManager.Instance.ToggleUIControl(true);
                InputSystemManager.Instance.ToggleInGameControl(false);

                //TODO: Play Die Cutscene
                break;
        }
    }

    public void OnPause()
    {
        if(gameStage == GameStage.Playing)
            OnChangeGameStage(GameStage.Pause);
        else if(gameStage == GameStage.Pause)
            OnChangeGameStage(GameStage.Playing);
    }

    public void OnDie()
    {
        //TODO: Run Cutscene
        Debug.LogWarning("Game Over!!! Noob!");
        isPause = true;
        OnChangeGameStage(GameStage.GameOver);
    }

    //TODO: End Game Cutscene

    void LockCursor(bool toggle)
    {
        if(toggle)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void OnDestroy() 
    {
        RemoveInputListener();
    }
}
