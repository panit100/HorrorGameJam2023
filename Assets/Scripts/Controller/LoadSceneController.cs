using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneController : MonoBehaviour
{
    void Start()
    {
#if UNITY_EDITOR
        if(SceneManager.sceneCount == 1 )
            StartCoroutine(SceneController.Instance.GoToSceneMainMenu());
#endif
    }

    
}
