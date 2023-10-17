using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

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
    [Range(0f, 100f)]
    [SerializeField] float batteryAmout = 100f;
    [SerializeField] float batteryConsumeAmount = 1.5f;
    [SerializeField] float batteryRefillAmount = 3.5f;

    [SerializeField] float scanRange = 10f;
    [Range(0, 180)]
    [SerializeField] float fovDeg = 45;
    float fovRad => fovDeg * Mathf.Deg2Rad;
    float angThresh => Mathf.Cos(fovRad / 2);

    List<Scanable> scanningObject = new List<Scanable>();

    bool isScanning = false;

    void Start()
    {
        equipmentType = EquipmentType.Scanner;
    }

    public override void OnUse()
    {
        base.OnUse();

        if(isPress && batteryAmout > 0)
            OnScan();
        else
            OnUnscan();
    }

    void Update()
    {
        if(isScanning && batteryAmout <= 0)
            OnUnscan();

        if(isScanning)
            ConsumeBattery();
        else
            RefillBattery();
    }

    void OnScan()
    {
        isScanning = true;
        
        Collider[] scanObject = Physics.OverlapSphere(transform.position,scanRange);

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
        if(!isScanning)
            return;

        foreach(var n in scanningObject)
        {
            n.OnDeactiveScan();
        }

        scanningObject.Clear();

        isScanning = false;
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

    void ConsumeBattery()
    {
        if(batteryAmout <= 0)
        {
            batteryAmout = 0;
            return;
        }

        batteryAmout -= batteryConsumeAmount * Time.deltaTime;
    }

    void RefillBattery()
    {
        if(batteryAmout >= 100)
        {
            batteryAmout = 100;
            return;
        }

        batteryAmout += batteryRefillAmount * Time.deltaTime;
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
