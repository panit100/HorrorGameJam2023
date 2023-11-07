using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UI;
using HorrorJam.AI;
using System;
using System.Linq;
using HorrorJam.Audio;




#if UNITY_EDITOR
using UnityEditor;
#endif

public class ScannerEquipment : Equipment
{
#region Setting
    [Header("Scanner Setting")]

    [Range(0f, 100f)]
    [SerializeField] float batteryAmout = 100f;
    [SerializeField] float batteryConsumeAmount = 1.5f;
    [SerializeField] float batteryRefillAmount = 3.5f;

    [SerializeField] float scanRange = 10f;
    [Range(0, 180)]
    [SerializeField] float ScanDeg = 45;
    float fovRad => ScanDeg * Mathf.Deg2Rad;
    float angThresh => Mathf.Cos(fovRad / 2);

    [SerializeField] ScannerCanvas scannerCanvas;
#endregion

#region  Animation
    [Header("For animation")] 
    [SerializeField]private float AnimDuration;
    [SerializeField]private Vector3 initpos;
    [SerializeField]private Vector3 endpos;
    [SerializeField]private Vector3 initrot;
    [SerializeField]private Vector3 endrot;
    [SerializeField] private GameObject MeshGroup;
    [SerializeField] private GameObject animationRoot;
    [SerializeField] private Image Brightness;
    private Tween Onhold;
#endregion
    
    bool isScanning = false;
    Collider[] objectInRange;
    Scanable scanningObject;
    List<Scanable> scanningEnemies = new List<Scanable>();

    public Scanable ScanningObject => scanningObject;

    public Action OnBatteryEmpty;
    
    void Start()
    {
        equipmentType = EquipmentType.Scanner;
    }

    public override void OnUse()
    {
        base.OnUse();

        if(isPress && batteryAmout > 0)
        {
            OnScan();
        }
        else
        {
            OnUnscan();
        }    
    }

    public override void HoldAnim()
    {
        if(!Application.isPlaying)
            return;

        MeshGroup.SetActive(true);
        Onhold.Kill();
        animationRoot.transform.localPosition = initpos;
        Brightness.transform.localScale = new Vector3(1,1,1);
        Onhold = animationRoot.transform.DOLocalMove(endpos, AnimDuration).SetEase(Ease.OutExpo).OnComplete(()=> Brightness.transform.DOScale(new Vector3(Brightness.transform.localScale.x,0, Brightness.transform.localScale.z),AnimDuration*0.5f).SetEase(Ease.OutExpo));
        animationRoot.transform.localRotation = Quaternion.Euler(initrot);
        animationRoot.transform.DOLocalRotate(endrot,AnimDuration).SetEase(Ease.OutExpo);
    }
    
    public override void PutAnim()
    {
        if(!Application.isPlaying)
            return;

        MeshGroup.SetActive(false);
        
    }

    void Update()
    {
        objectInRange = GetObjectInScanRange();

        UpdateCanvas();

        if(isScanning)
        {
            if(batteryAmout <= 0)
            {
                OnUnscan();
                OnBatteryEmpty?.Invoke();
                return;
            }
            
            ConsumeBattery();
        }
        else
            RefillBattery();
    }

    void UpdateCanvas()
    {
        if(scannerCanvas == null)
            return;

        scannerCanvas.UpdateBattery(batteryAmout);

        if(!isScanning)
        {
            if(objectInRange.Length > 0) 
                scannerCanvas.UpdateText("Detect");
            else
                scannerCanvas.UpdateText("Not Found");
        }
        else
        {
            scannerCanvas.UpdateText("Scanning");
        }
    }

    void OnScan()
    {
        isScanning = true;
        
        AudioManager.Instance.PlayAudio("scanner","scanner");

        if(objectInRange.Length == 0)
            return;

        var closestObject = GetClosestObject();

        if(closestObject != null)
        {
            if(CylindricalSectorContains(closestObject.transform.position))
            {
                closestObject.OnActiveScan();
                if(closestObject.TryGetComponent<ScanObjective>(out ScanObjective scan))
                    scannerCanvas.SetScanner(scan);

                scanningObject = closestObject;
            }
        }

        var enemies = GetEnemy();

        foreach(var n in enemies)
        {
            if(CylindricalSectorContains(n.transform.position))
            {
                n.OnActiveScan();
                scanningEnemies.Add(n);
            }
        }
        
    }

    public void OnUnscan()
    {
        if(!isScanning)
            return;

        AudioManager.Instance.StopAudio("scanner");

        if(scanningObject != null)
        {
            scanningObject.OnDeactiveScan();
            scanningObject = null;
        }

        if(scanningEnemies.Count > 0)
        {
            foreach(var n in scanningEnemies)
                n.OnDeactiveScan();
        }

        scanningEnemies.Clear();
        
        scannerCanvas.SetScanner(null);

        isScanning = false;
    }

    Scanable GetClosestObject()
    {
        var closetObjectArray = objectInRange.OrderBy(c => (transform.position - c.transform.position).sqrMagnitude).ToArray();
        
        var closestObject = closetObjectArray.Where(closetObject => closetObject.GetComponentInParent<Enemy>() == null).ToArray();

        if(closestObject.Length == 0)
            return null;

        return closestObject[0].GetComponent<Scanable>();
    }

    Scanable[] GetEnemy()
    {
        var enemyArray = objectInRange.Where(enemy => enemy.GetComponentInParent<Enemy>() != null).ToArray();

        List<Scanable> enemies = new List<Scanable>();

        foreach(var n in enemyArray)
        {
            enemies.Add(n.GetComponent<Scanable>());
        }

        return enemies.ToArray();
    }

    Collider[] GetObjectInScanRange()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position,scanRange);
        
        List<Collider> collidersInScanRange = new List<Collider>();

        foreach(var n in colliders)
        {
            if(isScanableObjectInScanRange(n))
                collidersInScanRange.Add(n);
        }

        return collidersInScanRange.ToArray();
    }

    bool isScanableObjectInScanRange(Collider collider)
    {
        if(collider.GetComponent<Scanable>() != null)
        {
            if(CylindricalSectorContains(collider.transform.position))
            {
                if(collider.GetComponent<Scanable>().isObjective())
                    return true;

                if(collider.GetComponent<Scanable>().isFakeEnemy())
                    return true;

                if(collider.GetComponent<Scanable>().isEnemy())
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
        
        Handles.DrawWireArc(default,Vector3.up,vLeft,ScanDeg,scanRange);

        Gizmos.DrawLine(default,vLeft);
        Gizmos.DrawLine(default,vRight);
    }   
#endif
}
