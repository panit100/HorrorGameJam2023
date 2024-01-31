using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class MainMenuAnimationEventController : MonoBehaviour
{
    [SerializeField] private Camera TVreactCamLayer1;
    [SerializeField] private Animator animatorLayer0;
    [SerializeField] private CanvasScaler mainCanvas;
   // [SerializeField] private PixelArtFilter pixelate;
    [SerializeField] private ScreenLineLoading loadline;
    [SerializeField] private Image crtpic;
    [SerializeField] private Animator pixelLoad;



    [SerializeField] private PostProcessVolume volume_1st;
    [SerializeField] private PostProcessVolume Volume_2nd;
    private Rect tempcamv2rec;
    private Vector4 rect;
    // HardCode


    void InitCRT()
    {
        volume_1st.profile = Instantiate(volume_1st.profile);
        Volume_2nd.profile = Instantiate(Volume_2nd.profile);
    }
    void Start()
    {
       InitCRT();
        tempcamv2rec = TVreactCamLayer1.rect;
        OnstartMainMenuTV();
    }

    [Button]
    public void OnstartMainMenuTV()
    {
        pixelLoad.Play("PixelscreenScan_Default",-1,0);
        // Vector2 rectWH = new Vector2(TVreactCamLayer1.rect.width, TVreactCamLayer1.rect.height);
        // Vector2 rectMinMax = new Vector2(TVreactCamLayer1.rect.x, TVreactCamLayer1.rect.y);
        // rect = new Vector4(rectWH.x, rectWH.y,rectMinMax.x,rectMinMax.y);
        // DOTween.To(() => rect, x => rect = x, new Vector4(1, 1, 0, 0), 0.075f).SetEase(Ease.OutQuad)
        //     .OnUpdate(() =>
        //     {
        //         TVreactCamLayer1.rect = new Rect(new Vector2(rect.w, rect.z), new Vector2(rect.x, rect.y));
        //     });
        TVreactCamLayer1.rect = tempcamv2rec;
       animatorLayer0.enabled = true;
        animatorLayer0.Rebind();
        animatorLayer0.Update(0f);
        //crtpost.curvature = 1;
        volume_1st.profile.GetSetting<EffectCRT>().curvature.Override(1f); 
        Volume_2nd.profile.GetSetting<UnityEngine.Rendering.PostProcessing.Bloom>().intensity.value = 1.18f;
     
        mainCanvas.matchWidthOrHeight = 1f;
        pixelLoad.Play("OnloadPixel",-1,0);
         DOTween.To(() => Volume_2nd.profile.GetSetting<UnityEngine.Rendering.PostProcessing.Bloom>().intensity, x => Volume_2nd.profile.GetSetting<UnityEngine.Rendering.PostProcessing.Bloom>().intensity.value = x ,0.1f,1f).SetEase(Ease.OutQuint);
         DOTween.To(() =>  volume_1st.profile.GetSetting<EffectCRT>().curvature  , x => volume_1st.profile.GetSetting<EffectCRT>().curvature.value  = x, 5f, 1f).SetEase(Ease.OutElastic);
         DOTween.To(() => mainCanvas.matchWidthOrHeight, x => mainCanvas.matchWidthOrHeight = x, 0, 1f).SetEase(Ease.OutElastic).OnComplete(() =>
                 DOTween.To(() => TVreactCamLayer1.rect.y,
                         x => TVreactCamLayer1.rect = new Rect(new Vector2(0, x), Vector2.one), -1, 0.5f)
                     .SetEase(Ease.OutElastic)).OnComplete(() =>
             {
                 TVreactCamLayer1.rect = tempcamv2rec;
                 animatorLayer0.enabled = false;
             });
            
         ;
    }

    public void OnShutdownMainMenuTV(UnityAction quit)
    {
        animatorLayer0.enabled = true;
        animatorLayer0.Play("Reverse");
        DOTween.To(() => Volume_2nd.profile.GetSetting<UnityEngine.Rendering.PostProcessing.Bloom>().intensity, x => Volume_2nd.profile.GetSetting<UnityEngine.Rendering.PostProcessing.Bloom>().intensity.value = x ,1.18f,0.5f).SetEase(Ease.InExpo);
        DOTween.To(() => volume_1st.profile.GetSetting<EffectCRT>().curvature, x => volume_1st.profile.GetSetting<EffectCRT>().curvature.value = x, 1f, 0.5f).SetEase(Ease.InExpo);
        DOTween.To(() => mainCanvas.matchWidthOrHeight, x => mainCanvas.matchWidthOrHeight = x, 1, 0.5f).SetEase(Ease.InExpo).OnComplete(() =>
                DOTween.To(() => TVreactCamLayer1.rect.y,
                        x => TVreactCamLayer1.rect = new Rect(new Vector2(0, x), Vector2.one), 0, 0.5f)
                    .SetEase(Ease.InExpo)).OnComplete((() => {quit?.Invoke();}));
    }

    [Button]
    public void OnPanelLoadingAnim()
    {
       //  loadline.afterevent += () => DOTween.To(() => pixelate.downSamples, x => pixelate.downSamples = x, 0, 0.25f)
       //      .SetEase(Ease.Linear)
       //      .OnComplete(() =>
       //      {
       //          GameManager.Instance.LockCursor(false);
       //      }
       // );
        loadline.InstantFill(1);
        pixelLoad.Play("OnloadPixel",-1,0);
        loadline.FillLine(0);
    }
    
}
