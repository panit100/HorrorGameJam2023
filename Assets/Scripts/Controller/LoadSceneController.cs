using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneController : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GameManager.Instance.GoToSceneMainMenu());
    }


}
