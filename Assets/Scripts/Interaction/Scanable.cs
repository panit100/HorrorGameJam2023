using DG.Tweening;
using Eenemy_FSM;
using HorrorJam.Audio;
using Sirenix.OdinInspector;
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
    public UnityAction onDeactiveScanComplete;

    bool isScanning = false;
    public bool IsScanning => isScanning;

    float scanCompletedTime;
    public float ScanCompletedTime => scanCompletedTime;

    public void OnActiveScan()
    {
        if(alreadyScan)
            return;

        if(!isObjective() && !isFakeEnemy() && !isEnemy())
            return;
        
        print("Active Scan");

        isScanning = true;
        scanTween.Kill();
        scanTween = DOTween.To(() => scanProgress, x => scanProgress = x,100f,scanDuration).SetEase(scanEase).OnComplete(OnScanComplete);
        onActiveScan?.Invoke();
    }

    public void OnDeactiveScan()
    {
        if(alreadyScan)
            return;
            
        isScanning = false;
        scanTween.Kill();
        scanTween = DOTween.To(() => scanProgress, x => scanProgress = x,0f,.5f).SetEase(scanEase).OnComplete(OnDeactiveScanComplete);
        onDeactiveScan?.Invoke();
    }

    public void OnDeactiveScanWithDuration(float duration)
    {
        scanTween.Kill();
        scanTween = DOTween.To(() => scanProgress, x => scanProgress = x,0f,duration).SetEase(scanEase).OnComplete(OnDeactiveScanComplete);
        onDeactiveScan?.Invoke();
    }

    public void ForceDeactiveScane()
    {
        OnDeactiveScanWithDuration(.5f);
    }

    void OnScanComplete()
    {
        alreadyScan = true;

        if(alreadyScan)
        {
            // AudioManager.Instance.PlayOneShot("scanComplete");
            scanCompletedTime = Time.time;
            onScanComplete?.Invoke();
        }
    }

    void OnDeactiveScanComplete()
    {
        isScanning = false;
        onDeactiveScanComplete?.Invoke();
    }

    [Button]
    public void ResetProgress()
    {
        alreadyScan = false;
        scanProgress = 0;
    }

    public bool isObjective()
    {
        if(GetComponent<Objective>() != null && GetComponent<Objective>().CheckObjective())
            return true;
        else
            return false;
    }

    public bool isFakeEnemy()
    {
        // if(GetComponentInParent<FakeEnemy>() != null)
        //     return true;
        // else
            return false;
    }

    public bool isEnemy()
    {
        if(GetComponentInParent<Enemy>() != null)
            return true;
        else
            return false;
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        GUI.color = scanProgress != 0 ? Color.red : Color.green;
        Handles.Label(transform.position + new Vector3(0,2.15f,0), $"Scan Progress : {scanProgress}");
    }
#endif
}
