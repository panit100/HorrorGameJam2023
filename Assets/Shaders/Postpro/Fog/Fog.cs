using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Fog : MonoBehaviour {
    [Header("Fog")]
    public Shader fogShader;
    public Color fogColor;
    
    [Range(0.0f, 1.0f)]
    public float fogDensity;
    
    [Range(0.0f, 100.0f)]
    public float fogOffset;
    
    private Material fogMat;

    void Start() {
        if (fogMat == null) {
            fogMat = new Material(fogShader);
            fogMat.hideFlags = HideFlags.HideAndDontSave;
        }

        Camera cam = GetComponent<Camera>();
        cam.depthTextureMode = cam.depthTextureMode | DepthTextureMode.Depth;
    }

    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        fogMat.SetVector("_FogColor", fogColor);
        fogMat.SetFloat("_FogDensity", fogDensity);
        fogMat.SetFloat("_FogOffset", fogOffset);
        Graphics.Blit(source, destination, fogMat);
    }

    [Button]
    void fogStart()
    {
        DOTween.To(() => CustomPostprocessingManager.Instance.fog.fogDensity, x => CustomPostprocessingManager.Instance.fog.fogDensity = x, 0.194f, 1.5f);
    }

    [Button]
    void fogend()
    {
        DOTween.To(() => CustomPostprocessingManager.Instance.fog.fogDensity, x => CustomPostprocessingManager.Instance.fog.fogDensity = x, 0f, 0.194f);
    }
}