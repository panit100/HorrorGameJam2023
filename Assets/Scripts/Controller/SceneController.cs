using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    public string SCENE_MAINMENU { get { return "Scene_MainMenu"; } }
    public string SCENE_CUTSCENE {get { return "Scene_Cutscene"; } }
    public string SCENE_MAIN { get { return "Scene_Main"; } }
    public string SCENE_CORE { get { return "Scene_Core"; } }
    public string SCENE_FAKELOADER { get { return "Scene_FakeLoader"; }
    }

    public float loadingProgress { get; private set; }
    public Scene loadedSceneBefore;
    [ShowInInspector][ReadOnly] public string scenename
    {
        get { return loadedSceneBefore.name; }
        set {return;}
    } 

    bool gameplaySceneLoaded = false;
    public bool GameplaySceneLoaded { set { gameplaySceneLoaded = value; } get { return gameplaySceneLoaded; } }

    protected override void InitAfterAwake()
    {
            
#if !UNITY_EDITOR
        LoadCoreScene();
#endif
    }

    public void OnLoadSceneAsync(string sceneName, Action beforeSwitchScene = null, Action afterSwitchScene = null)
    {
        StartCoroutine(LoadSceneAsync(sceneName, beforeSwitchScene, afterSwitchScene));
    }

    IEnumerator LoadSceneAsync(string sceneName, Action beforeSwitchScene = null, Action afterSwitchScene = null)
    {
        beforeSwitchScene?.Invoke();

        yield return new WaitForEndOfFrame();

        var asyncOparation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        asyncOparation.allowSceneActivation = false;

        while (!asyncOparation.isDone)
        {

            loadingProgress = Mathf.Clamp01(asyncOparation.progress / 0.9f);
            print("Scene progress : " + loadingProgress);

            if (loadingProgress >= 0.9f)
            {
                asyncOparation.allowSceneActivation = true;
            }

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForEndOfFrame();

         var loadedScene = SceneManager.GetSceneByName(sceneName);
         for (int i =SceneManager.sceneCount-1 ; i >= 0; i--)
         {
             if (SceneManager.GetSceneByBuildIndex(i).name == sceneName)
             {
                 loadedScene = SceneManager.GetSceneByBuildIndex(i);
                 break;
             }
         }
         
        if (loadedScene.isLoaded)
        {
            SceneManager.SetActiveScene(loadedScene);
        }

        if (loadedSceneBefore.IsValid())
            SceneManager.UnloadSceneAsync(loadedSceneBefore);

        loadedSceneBefore = loadedScene;

        yield return new WaitForEndOfFrame();

        afterSwitchScene?.Invoke();
    }

    void LoadCoreScene()
    {
        StartCoroutine(GameManager.Instance.GoToSceneMainMenu());
    }

    
}
