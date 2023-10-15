using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScannerEquipment : Equipment
{
    [Range(0f,100f)]
    [SerializeField] float scanPrograss = 0f;
    [SerializeField] float scanDuration = 5f;
    [SerializeField] Ease scanEase;

    Tween scanTween;
    
    bool isScan = false;

    public override void OnUse()
    {
        base.OnUse();

        if(isPress && !isScan)
            OnActiveScan();
        else
            OnDeactiveScan();
    }

    void OnActiveScan()
    {
        isScan = true;

        scanTween.Kill();
        scanTween = DOTween.To(() => scanPrograss, x=> scanPrograss = x,100f,scanDuration).SetEase(scanEase).OnComplete(OnScanComplete);
    }

    void OnDeactiveScan()
    {
        scanTween.Kill();
        scanTween = DOTween.To(() => scanPrograss, x=> scanPrograss = x,0f,.5f).SetEase(scanEase).OnComplete(() => {isScan = false;});
    }

    void OnScanComplete()
    {

    }
}
