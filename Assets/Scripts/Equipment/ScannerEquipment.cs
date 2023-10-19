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

    [SerializeField] ScannerCanvas scannerCanvas;

    List<Scanable> scanningObject = new List<Scanable>();

    bool isScanning = false;
    Collider[] objectInRange;

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
        objectInRange = GetObjectInRange();

        if(isScanning && batteryAmout <= 0)
            OnUnscan();

        if(isScanning)
            ConsumeBattery();
        else
            RefillBattery();

        if(scannerCanvas != null)
        {
            scannerCanvas.UpdateBattery(batteryAmout);

            if(!isScanning)
            {
                if(isScanableObjectInRange()) 
                    scannerCanvas.UpdateText("Detect");
                else
                    scannerCanvas.UpdateText("Not Found");
            }
            else
            {
                scannerCanvas.UpdateText("Scanning");
            }
            
        }


    }

    void OnScan()
    {
        isScanning = true;
        
        foreach(var n in objectInRange)
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

    Collider[] GetObjectInRange()
    {
        return Physics.OverlapSphere(PlayerManager.Instance.transform.position,scanRange);
    }

    bool isScanableObjectInRange()
    {
        foreach(var n in objectInRange)
        {
            if(n.GetComponent<Scanable>() != null)
            {
                return true;
            }
        }

        return false;
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
        // Gizmos.matrix = Handles.matrix = transform.localToWorldMatrix;
        // Gizmos.color = Handles.color = Color.red;

        // float p = angThresh;
        // float x = Mathf.Sqrt(1 - p * p);

        // Vector3 vLeft = new Vector3(-x,0,p) * scanRange;
        // Vector3 vRight = new Vector3(x,0,p) * scanRange;
        
        // Handles.DrawWireArc(transform.InverseTransformVector(PlayerManager.Instance.transform.position),Vector3.up,vLeft,fovDeg,scanRange);

        // Gizmos.DrawLine(transform.InverseTransformVector(PlayerManager.Instance.transform.position),vLeft);
        // Gizmos.DrawLine(transform.InverseTransformVector(PlayerManager.Instance.transform.position),vRight);
    }   
#endif
}
