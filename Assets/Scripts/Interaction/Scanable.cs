using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Scanable : MonoBehaviour
{
    public float scanProgress;
    [SerializeField] float scanDuration = 5f;
    [SerializeField] Ease scanEase;
    Tween scanTween;
    
    bool alreadyScan = false;

    public bool AlreadyScan => alreadyScan;

    public UnityAction onActiveScan;
    public UnityAction onDeactiveScan;
    public UnityAction onScanComplete;
    

    public void OnActiveScan()
    {
        if(alreadyScan)
            return;

        if(this.GetComponent<Objective>() != null )
            return;
        
        scanTween.Kill();
        scanTween = DOTween.To(() => scanProgress, x=> scanProgress = x,100f,scanDuration).SetEase(scanEase).OnComplete(OnScanComplete);
        onActiveScan?.Invoke();
    }

    public void OnDeactiveScan()
    {
        if(alreadyScan)
            return;
            
        scanTween.Kill();
        scanTween = DOTween.To(() => scanProgress, x=> scanProgress = x,0f,.5f).SetEase(scanEase);
        onDeactiveScan?.Invoke();
    }

    public void OnDeactiveScanWithDuration(float duration)
    {
        scanTween.Kill();
        scanTween = DOTween.To(() => scanProgress, x=> scanProgress = x,0f,duration).SetEase(scanEase);
        onDeactiveScan?.Invoke();
    }

    void OnScanComplete()
    {
        alreadyScan = true;

        if(alreadyScan)
            onScanComplete?.Invoke();
    }

    public void ResetProgress()
    {
        alreadyScan = false;
        scanProgress = 0;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        GUI.color = scanProgress != 0 ? Color.red : Color.green;
        Handles.Label(transform.position + new Vector3(0,2.15f,0), $"Scan Progress : {scanProgress}");
    }
#endif
}
