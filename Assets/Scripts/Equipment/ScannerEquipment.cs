using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ScannerEquipment : Equipment
{
    // [Range(0f,100f)]
    // [SerializeField] float scanProgress = 0f;
    // [SerializeField] float scanDuration = 5f;
    // [SerializeField] Ease scanEase;

    // Tween scanTween;
    
    // bool isScan = false;

    [SerializeField] float scanRange = 10f;

    [Range(0, 180)]
    [SerializeField] float fovDeg = 45;
    float fovRad => fovDeg * Mathf.Deg2Rad;
    float angThresh => Mathf.Cos(fovRad / 2);

    List<Scanable> scanningObject = new List<Scanable>();

    public override void OnUse()
    {
        base.OnUse();

        if(isPress)
            OnScan();
        else
            OnUnscan();
    }

    void OnScan()
    {
        // isScan = true;

        // scanTween.Kill();
        // scanTween = DOTween.To(() => scanProgress, x=> scanProgress = x,100f,scanDuration).SetEase(scanEase).OnComplete(OnScanComplete);

        Collider[] scanObject = Physics.OverlapSphere(transform.position,scanRange,LayerMask.GetMask("InteractObject"));

        foreach(var n in scanObject)
        {
            if(n.GetComponent<Scanable>() != null)
            {
                if(CylindricalSectorContains(n.transform.position))
                {
                    n.GetComponent<Scanable>().OnActiveScan();
                    scanningObject.Add(n.GetComponent<Scanable>());
                }
            }
        }
    }

    void OnUnscan()
    {
        // scanTween.Kill();
        // scanTween = DOTween.To(() => scanProgress, x=> scanProgress = x,0f,.5f).SetEase(scanEase).OnComplete(() => {isScan = false;});

        foreach(var n in scanningObject)
        {
            n.OnDeactiveScan();
        }

        scanningObject.Clear();
    }

    public bool CylindricalSectorContains(Vector3 position)
    {
        //Inverse transform world to local
        Vector3 vecToTargetWorld = position - transform.position;

        Vector3 vecToTarget = transform.InverseTransformVector(vecToTargetWorld);
        Vector3 dirToTarget = vecToTarget.normalized;

        //angular check

        Vector3 flatDirToTarget = vecToTarget;
        flatDirToTarget.y = 0;
        float flatDistance = flatDirToTarget.magnitude;
        flatDirToTarget /= flatDistance; // normalizes

        if(flatDirToTarget.z < angThresh)
            return false; // outside the angular wedge

        //cylindrical radial

        if(flatDistance > scanRange)
            return false; // outside the cylinder

        return true;    
    }

#if UNITY_EDITOR
    void OnDrawGizmos() 
    {
        Gizmos.matrix = Handles.matrix = transform.localToWorldMatrix;
        Gizmos.color = Handles.color = Color.red;

        float p = angThresh;
        float x = Mathf.Sqrt(1 - p * p);

        Vector3 vLeft = new Vector3(-x,0,p) * scanRange;
        Vector3 vRight = new Vector3(x,0,p) * scanRange;
        
        Handles.DrawWireArc(default,Vector3.up,vLeft,fovDeg,scanRange);

        Gizmos.DrawLine(default,vLeft);
        Gizmos.DrawLine(default,vRight);
    }   
#endif
}
