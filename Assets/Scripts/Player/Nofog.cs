using UnityEngine;

public class Nofog : MonoBehaviour {

    bool doWeHaveFogInScene;

    void Start() {
        doWeHaveFogInScene = RenderSettings.fog;
    }

    void OnPreRender() {
        RenderSettings.fog = false;
    }
    
    void OnPostRender() {
        RenderSettings.fog = doWeHaveFogInScene;
    }
}
