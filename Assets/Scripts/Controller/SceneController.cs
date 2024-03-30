using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneController : PersistentSingleton<SceneController>
{
    public string SCENE_MAINMENU { get { return "Scene_MainMenu"; } }
    public string SCENE_CUTSCENE { get { return "Scene_Cutscene"; } }
    public string SCENE_MAIN { get { return "Scene_Main"; } }
    public string SCENE_CORE { get { return "Scene_Core"; } }
    public string SCENE_FAKELOADER
    {
        get { return "Scene_FakeLoader"; } //wait 2-3 sec then start load scene async
    }

    public float loadingProgress { get; private set; }

    protected override void InitAfterAwake()
    {

    }

    public void OnLoadSceneAsync(string sceneName, UnityEvent beforeSwitchScene = null, UnityEvent afterSwitchScene = null)
    {
        StartCoroutine(LoadSceneAsync(sceneName, beforeSwitchScene, afterSwitchScene));
    }

    IEnumerator LoadSceneAsync(string sceneName, UnityEvent beforeSwitchScene = null, UnityEvent afterSwitchScene = null)
    {
        beforeSwitchScene?.Invoke();

        yield return new WaitForEndOfFrame();

        var asyncOparation = SceneManager.LoadSceneAsync(sceneName);

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

        afterSwitchScene?.Invoke();
    }

    IEnumerator LoadSubSceneAsync(string sceneName, UnityEvent beforeSwitchScene = null, UnityEvent afterSwitchScene = null)
    {
        yield return null;
    }
}
