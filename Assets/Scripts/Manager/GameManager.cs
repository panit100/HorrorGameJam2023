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
    Pause,
    Cutscene,
    GameOver,
    Tutorial
}

public class GameManager : Singleton<GameManager>
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

#if UNITY_EDITOR
    void Start()
    {
            LockCursor(true);
            TimeManager.Instance.SetCurrentTime();
            OnChangeGameStage(GameStage.Playing);
    }
#endif

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

                PlayerManager.Instance.PlayerCamera.transform.DORotate(new Vector3(0,0,90f),1f).OnComplete(() => {StartCoroutine(GoToSceneMainMenu());});
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
        SceneController.Instance.OnLoadSceneAsync(SceneController.Instance.SCENE_MAIN, null, () => {OnStartGame();});
    }

    [Button]
    public void OnStartGame()
    {
        TimeManager.Instance.SetCurrentTime();
        MainObjectiveManager.Instance.SetupObjective();
        initFogsetting();
    }

    public IEnumerator GoToCutscene(string cutsceneID)
    {
        yield return new WaitUntil(() => SceneController.Instance != null);
        OnChangeGameStage(GameStage.Cutscene);
        SceneController.Instance.OnLoadSceneAsync(SceneController.Instance.SCENE_CUTSCENE, null, () => {OnStartCutscene(cutsceneID);});
    }

    public void OnStartCutscene(String CutsceneID)
    {
        CutsceneManager.Instance.initCutscene();
        CutsceneManager.Instance.Playcutscene(CutsceneID);
    }

    public void initFogsetting()
    {
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.141f,0,0);
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = 0;
    }
}
