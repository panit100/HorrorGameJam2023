using System;
using System.Collections;
using DG.Tweening;
using HorrorJam.AI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public enum GameStage
{
    MainMenu,
    Playing,
    Pause,
    Cutscene,
    GameOver,
    Tutorial
}

public class GameManager : Singleton<GameManager>,IAstronosisDebug
{
    [SerializeField] GameStage gameStage = GameStage.MainMenu;

    [Indent,SerializeField,ReadOnly] bool isPause = false;

    public GameStage GameStage => gameStage;
    public bool IsPause => isPause;

    public Action onPause;
    
    protected override void InitAfterAwake()
    {
        AddInputListener();
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
        gameStage = _gameStage;

        switch(gameStage)
        {
            case GameStage.MainMenu:
                LockCursor(false);
                InputSystemManager.Instance.TogglePlayerControl(false);
                InputSystemManager.Instance.ToggleInGameControl(false);
                InputSystemManager.Instance.ToggleInGameUIControl(false);
                InputSystemManager.Instance.ToggleCutsceneControl(false);

                break;
            case GameStage.Playing:
                isPause = false;
                InputSystemManager.Instance.ToggleInGameControl(true);
                InputSystemManager.Instance.ToggleInGameUIControl(true);
                InputSystemManager.Instance.ToggleCutsceneControl(false);

                PlayerManager.Instance.OnChangePlayerState(PlayerState.Move);
                break;
            case GameStage.Pause:
                isPause = true;
                LockCursor(false);

                InputSystemManager.Instance.TogglePlayerControl(false);
                InputSystemManager.Instance.ToggleInGameControl(true);
                InputSystemManager.Instance.ToggleInGameUIControl(false);
                InputSystemManager.Instance.ToggleCutsceneControl(false);

                PlayerManager.Instance.PlayerEquipment.GetScanner().ForceSetIsPress(false);

                onPause?.Invoke();
                break;
            case GameStage.GameOver:
                LockCursor(false);
                InputSystemManager.Instance.TogglePlayerControl(false);
                InputSystemManager.Instance.ToggleInGameControl(false);
                InputSystemManager.Instance.ToggleInGameUIControl(false);
                InputSystemManager.Instance.ToggleCutsceneControl(false);
                PlayerManager.Instance.PlayerEquipment.GetScanner().ForceSetIsPress(false);
              //  StartCoroutine(GoToSceneMainMenu());
                break;
            case GameStage.Cutscene:
                LockCursor(true);
                InputSystemManager.Instance.TogglePlayerControl(false);
                InputSystemManager.Instance.ToggleInGameControl(false);
                InputSystemManager.Instance.ToggleInGameUIControl(false);
                InputSystemManager.Instance.ToggleCutsceneControl(true);

                break;
            case GameStage.Tutorial:
                LockCursor(false);
                PlayerManager.Instance.PlayerEquipment.GetScanner().ForceSetIsPress(false);
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

        UnityEvent afterLoadScene = new UnityEvent();
        afterLoadScene.AddListener(OnStartGame);
        
        SceneController.Instance.OnLoadSceneAsync(SceneController.Instance.SCENE_MAIN, null, afterLoadScene);
    }
    public IEnumerator ReTryGameScene()
    {
        yield return new WaitUntil(() => SceneController.Instance != null);

        UnityEvent afterLoadScene = new UnityEvent();
        afterLoadScene.AddListener(() => { StartCoroutine(GoToSceneGame());});

        SceneController.Instance.OnLoadSceneAsync(SceneController.Instance.SCENE_FAKELOADER, null,afterLoadScene);
    }

    [Button]
    public void OnStartGame()
    {
        TimeManager.Instance.SetCurrentTime();
        MainObjectiveManager.Instance.SetupObjective();
    }

    public IEnumerator GoToCutscene(string cutsceneID)
    {
        yield return new WaitUntil(() => SceneController.Instance != null);
        OnChangeGameStage(GameStage.Cutscene);

        UnityEvent afterLoadScene = new UnityEvent();
        afterLoadScene.AddListener(() => {OnStartCutscene(cutsceneID);});

        SceneController.Instance.OnLoadSceneAsync(SceneController.Instance.SCENE_CUTSCENE, null, afterLoadScene);
    }

    public void OnStartCutscene(String CutsceneID)
    {
        CutsceneManager.Instance.initCutscene();
        CutsceneManager.Instance.Playcutscene(CutsceneID);
    }
    
}
