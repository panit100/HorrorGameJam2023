using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenuAnimationEventController : MonoBehaviour
{
    [SerializeField] private Camera TVreactCamLayer1;
    [SerializeField] private CRT crtpost;
    [SerializeField] private Bloom bloom;
    [SerializeField] private Animator animatorLayer0;
    [SerializeField] private VHSPostProcessEffect _vhsPostProcessEffect;
    [SerializeField] private CanvasScaler mainCanvas;
    [SerializeField] private PixelArtFilter pixelate;
    [SerializeField] private ScreenLineLoading loadline;
    [SerializeField] private Image crtpic;

    private Rect tempcamv2rec;
    private Vector4 rect;
    // HardCode


    void Start()
    {
        tempcamv2rec = TVreactCamLayer1.rect;
        OnstartMainMenuTV();
    }

    [Button]
    public void OnstartMainMenuTV()
    {
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
        crtpost.curvature = 1;
        bloom.bloomIntensity = 1.18f;
        _vhsPostProcessEffect.Intensity = 0.55f;
        mainCanvas.matchWidthOrHeight = 0f;
         DOTween.To(() => bloom.bloomIntensity, x => bloom.bloomIntensity = x ,0,1f).SetEase(Ease.OutQuint);
         DOTween.To(() => crtpost.curvature, x => crtpost.curvature = x, 5f, 1f).SetEase(Ease.OutElastic);
         DOTween.To(() => mainCanvas.matchWidthOrHeight, x => mainCanvas.matchWidthOrHeight = x, 1, 1f).SetEase(Ease.OutElastic);
         DOTween.To(() => _vhsPostProcessEffect.Intensity, x => _vhsPostProcessEffect.Intensity = x, 1f, 1f).SetEase(Ease.OutElastic,0.01f)
             .OnComplete(() =>
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
        DOTween.To(() => bloom.bloomIntensity, x => bloom.bloomIntensity = x ,1.18f,0.5f).SetEase(Ease.InExpo);
        DOTween.To(() => crtpost.curvature, x => crtpost.curvature = x, 1f, 0.5f).SetEase(Ease.InExpo);
        DOTween.To(() => mainCanvas.matchWidthOrHeight, x => mainCanvas.matchWidthOrHeight = x, 0, 0.5f).SetEase(Ease.InExpo);
        DOTween.To(() => _vhsPostProcessEffect.Intensity, x => _vhsPostProcessEffect.Intensity = x, 0.4f, 0.5f).SetEase(Ease.InExpo)
            .OnComplete(() =>
                DOTween.To(() => TVreactCamLayer1.rect.y,
                        x => TVreactCamLayer1.rect = new Rect(new Vector2(0, x), Vector2.one), 0, 0.5f)
                    .SetEase(Ease.InExpo)).OnComplete((() => {quit?.Invoke();}));
    }

    [Button]
    public void OnPanelLoadingAnim()
    {
        GameManager.Instance.LockCursor(true);
        pixelate.downSamples = 5;
        crtpic.enabled = false;
        loadline.afterevemt += () => DOTween.Sequence().AppendInterval(0.1f).AppendCallback(()=> crtpic.enabled = true);
        loadline.afterevemt += () => DOTween.To(() => pixelate.downSamples, x => pixelate.downSamples = x, 0, 0.25f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                GameManager.Instance.LockCursor(false);
            }
       );
        loadline.InstantFill(1);
        loadline.FillLine(0);
    }
    
}
