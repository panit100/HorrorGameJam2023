using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneController : MonoBehaviour
{
    void Start()
    {
#if UNITY_EDITOR
        if(SceneManager.sceneCount == 1 )
            StartCoroutine(GameManager.Instance.GoToSceneMainMenu());
#endif
    }

    
}
