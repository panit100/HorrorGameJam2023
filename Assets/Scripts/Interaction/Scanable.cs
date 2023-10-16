using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Scanable : MonoBehaviour
{
    public float scanProgress;
    [SerializeField] float scanDuration = 5f;
    [SerializeField] Ease scanEase;
    Tween scanTween;
    
    bool alreadyScan = false;

    public void OnActiveScan()
    {
        if(alreadyScan)
            return;

        scanTween.Kill();
        scanTween = DOTween.To(() => scanProgress, x=> scanProgress = x,100f,scanDuration).SetEase(scanEase).OnComplete(OnScanComplete);
    }

    public void OnDeactiveScan()
    {
        if(alreadyScan)
            return;
            
        scanTween.Kill();
        scanTween = DOTween.To(() => scanProgress, x=> scanProgress = x,0f,.5f).SetEase(scanEase);
    }

    void OnScanComplete()
    {
        alreadyScan = true;
    }
}
