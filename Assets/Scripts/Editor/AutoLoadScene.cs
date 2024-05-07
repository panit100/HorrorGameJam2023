using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class AutoLoadScene
{
    // static AutoLoadScene()
    // {
    //     EditorSceneManager.sceneOpened += OnSceneOpened;
    // }

    // private static void OnSceneOpened(Scene scene, OpenSceneMode mode)
    // {
    //     if (EditorApplication.isPlaying)
    //     {
    //         return;
    //     }

    //     if (scene.name != "Scene_Core")
    //     {
    //         EditorSceneManager.OpenScene("Assets/Scenes/MainScene/Scene_Core.unity", OpenSceneMode.Additive);
    //     }
    // }
}
