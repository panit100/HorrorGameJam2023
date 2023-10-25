using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HorrorJam.AI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameStage
{
    MainMenu,
    Playing,
    OnPipboy,
    Pause,
    Cutscene,
    GameOver,
    Tutorial
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameStage gameStage = GameStage.MainMenu;

    public GameStage GameStage => gameStage;

    [Indent,SerializeField,ReadOnly] bool isPause = false;
    public bool IsPause => isPause;
    protected override void InitAfterAwake()
    {
        AddInputListener();
    }

    void Start()
    {
        #if UNITY_EDITOR
            LockCursor(true);
            TimeManager.Instance.SetCurrentTime();
            OnChangeGameStage(GameStage.Playing);
        #endif
    }

    void AddInputListener()
    {
        InputSystemManager.Instance.onPause += OnPause;
        InputSystemManager.Instance.onPause += ShowSkipCutsceneUI;
    }

    void RemoveInputListener()
    {
        InputSystemManager.Instance.onPause -= OnPause;
        InputSystemManager.Instance.onPause -= ShowSkipCutsceneUI;
    }

    public void OnChangeGameStage(GameStage _gameStage)
    {
        gameStage = _gameStage;

        switch(gameStage)
        {
            case GameStage.MainMenu:
                LockCursor(false);
                InputSystemManager.Instance.TogglePlayerControl(false);
                InputSystemManager.Instance.ToggleInGameControl(false);
                InputSystemManager.Instance.TogglePipboyControl(false);
                InputSystemManager.Instance.ToggleCutsceneControl(false);
                break;
            case GameStage.Playing:
                isPause = false;
                if(FindObjectOfType<PausePanel>() != null)
                    FindObjectOfType<PausePanel>().EnablePausePanel(false);
                LockCursor(true);

                InputSystemManager.Instance.TogglePlayerControl(true);
                InputSystemManager.Instance.ToggleInGameControl(true);
                InputSystemManager.Instance.TogglePipboyControl(true);
                InputSystemManager.Instance.ToggleCutsceneControl(false);
                break;
            case GameStage.OnPipboy:
                LockCursor(false);
                
                InputSystemManager.Instance.TogglePlayerControl(false);
                InputSystemManager.Instance.ToggleInGameControl(false);
                InputSystemManager.Instance.TogglePipboyControl(true);
                InputSystemManager.Instance.ToggleCutsceneControl(false);
                break;
            case GameStage.Pause:
                isPause = true;
                LockCursor(false);

                InputSystemManager.Instance.TogglePlayerControl(false);
                InputSystemManager.Instance.ToggleInGameControl(true);
                InputSystemManager.Instance.TogglePipboyControl(false);
                InputSystemManager.Instance.ToggleCutsceneControl(false);
                
                if(FindObjectOfType<PausePanel>() != null)
                    FindObjectOfType<PausePanel>().EnablePausePanel(true);
                break;
            case GameStage.GameOver:
                LockCursor(false);
                InputSystemManager.Instance.TogglePlayerControl(false);
                InputSystemManager.Instance.ToggleInGameControl(false);
                InputSystemManager.Instance.TogglePipboyControl(false);
                InputSystemManager.Instance.ToggleCutsceneControl(false);

                PlayerManager.Instance.PlayerCamera.transform.DORotate(new Vector3(0,0,90f),1f).OnComplete(() => {StartCoroutine(GoToSceneMainMenu());});
                break;
            case GameStage.Cutscene:
                LockCursor(true);
                InputSystemManager.Instance.TogglePlayerControl(false);
                InputSystemManager.Instance.ToggleInGameControl(false);
                InputSystemManager.Instance.TogglePipboyControl(false);
                InputSystemManager.Instance.ToggleCutsceneControl(true);
                break;
            case GameStage.Tutorial:
                LockCursor(false);
                InputSystemManager.Instance.TogglePlayerControl(false);
                InputSystemManager.Instance.ToggleInGameControl(false);
                InputSystemManager.Instance.TogglePipboyControl(false);
                InputSystemManager.Instance.ToggleCutsceneControl(false);
                break;

        }
    }

    [Button]
    public void OnPause()
    {
        if(gameStage == GameStage.Playing)
        {
            OnChangeGameStage(GameStage.Pause);
            AIManager.Instance.EnterStopEnemy();
        }
        else if(gameStage == GameStage.Pause)
        {
            OnChangeGameStage(GameStage.Playing);
            AIManager.Instance.ExitStopEnemy();
        }
    }

    [Button]
    public void OnDie()
    {
        OnChangeGameStage(GameStage.GameOver);
    }

    //TODO: Start Game Cutscene
    [Button]
    public void OnStartGame()
    {
        TimeManager.Instance.SetCurrentTime();
        GameManager.Instance.OnChangeGameStage(GameStage.Playing);
        MainObjectiveManager.Instance.SetupObjective();
    }


    public void OnStartCutscene(String CutsceneID)
    {
        CutsceneManager.Instance.initCutscene();
        CutsceneManager.Instance.Playcutscene(CutsceneID);
    }

    //TODO: End Game Cutscene

    [Button]
    public void OnEndGame()
    {
        LockCursor(true);
        InputSystemManager.Instance.TogglePlayerControl(false);
        InputSystemManager.Instance.ToggleInGameControl(false);

        //TODO: Load End game Cutscene scene
    }

    void ShowSkipCutsceneUI()
    {
        //TODO: Skip Cutscene
        if(gameStage != GameStage.Cutscene)
            return;
        
        //TODO: Show skip button
    }   

    [Button]
    public void OnSkipCutScene()
    {
        //TODO: Skip Cutscene
        //TODO: load scene game
    }

    public void LockCursor(bool toggle)
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

    public IEnumerator GoToSceneMainMenu()
    {
        yield return new WaitUntil(() => SceneController.Instance != null);
        GameManager.Instance.OnChangeGameStage(GameStage.MainMenu);
        SceneController.Instance.OnLoadSceneAsync(SceneController.Instance.SCENE_MAINMENU, null, null);
    }

    public IEnumerator GoToSceneGame()
    {
        yield return new WaitUntil(() => SceneController.Instance != null);
        SceneController.Instance.OnLoadSceneAsync(SceneController.Instance.SCENE_MAIN, null, () => {OnStartGame();});
    }

    public IEnumerator GoToCutscene(string cutsceneID)
    {
        yield return new WaitUntil(() => SceneController.Instance != null);
        GameManager.Instance.OnChangeGameStage(GameStage.Cutscene);
        SceneController.Instance.OnLoadSceneAsync(SceneController.Instance.SCENE_CUTSCENE, null, () => {OnStartCutscene(cutsceneID);});
    }
}
