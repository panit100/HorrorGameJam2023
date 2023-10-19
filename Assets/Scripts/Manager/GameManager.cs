using System.Collections;
using System.Collections.Generic;
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
            OnStartGame();
        #endif

        #if !UNITY_EDITOR
            LoadCoreScene();
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
                InputSystemManager.Instance.ToggleUIControl(true);
                InputSystemManager.Instance.ToggleInGameControl(false);
                break;
            case GameStage.Playing:
                LockCursor(true);

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
                //TODO: Load MainMenu after cutscene
                break;
            case GameStage.Cutscene:
                LockCursor(true);
                InputSystemManager.Instance.TogglePlayerControl(false);
                InputSystemManager.Instance.ToggleUIControl(false);
                InputSystemManager.Instance.ToggleInGameControl(true);
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
        //TODO: Run Cutscene when die
        isPause = true;
        OnChangeGameStage(GameStage.GameOver);
    }
    //TODO: Start Game Cutscene
    public void OnStartGame()
    {
        TimeManager.Instance.SetCurrentTime();

        //TODO: Run Cutscene when startGame

        //Lock Keyboard and Mouse
        LockCursor(true);
        InputSystemManager.Instance.TogglePlayerControl(false);
        InputSystemManager.Instance.ToggleUIControl(false);
        InputSystemManager.Instance.ToggleInGameControl(true);
        
        //TODO: When end cutscene change stage to Playing
    }


    //TODO: End Game Cutscene
    public void OnEndGame()
    {
        //TODO: Run Cutscene when endGame

        //Lock Keyboard and Mouse
        LockCursor(true);
        InputSystemManager.Instance.TogglePlayerControl(false);
        InputSystemManager.Instance.ToggleUIControl(false);
        InputSystemManager.Instance.ToggleInGameControl(false);
    }

    void ShowSkipCutsceneUI()
    {
        //TODO: Skip Cutscene
        if(gameStage != GameStage.Cutscene)
            return;
        
        //TODO: Show skip button
    }   

    public void OnSkipCutScene()
    {
        //TODO: Skip Cutscene

        OnChangeGameStage(GameStage.Playing);
    }

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

    void LoadCoreScene()
    {
        SceneManager.LoadScene(SceneController.Instance.SCENE_CORE, LoadSceneMode.Additive);
        StartCoroutine(GoToSceneMainMenu());
    }

    IEnumerator GoToSceneMainMenu()
    {
        yield return new WaitUntil(() => SceneController.Instance != null);
        GameManager.Instance.OnChangeGameStage(GameStage.MainMenu);
        SceneController.Instance.OnLoadSceneAsync(SceneController.Instance.SCENE_MAINMENU, null, null);
    }

    void OnDestroy() 
    {
        RemoveInputListener();
    }
}
