using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum EquipmentType
{
    Scanner,
    Camera,
}

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] EquipmentType currentEquipment;
    [SerializeField] List<Equipment> equipment = new List<Equipment>();

    void Start()
    {
        AddInputListener();
    }

    void AddInputListener()
    {
        InputSystemManager.Instance.onUseEquipment += OnUseEquipment;
        InputSystemManager.Instance.onUseScanner += SwitchEquipment;
        InputSystemManager.Instance.onUseCamera += SwitchEquipment;
    }

    void RemoveInputListener()
    {
        InputSystemManager.Instance.onUseEquipment -= OnUseEquipment;
        InputSystemManager.Instance.onUseScanner -= SwitchEquipment;
        InputSystemManager.Instance.onUseCamera -= SwitchEquipment;
        
    }

    void OnUseEquipment()
    {
        equipment.Find(n => n.equipmentType == currentEquipment).OnUse();
        //TODO สร้างตัวแปร 2 ตัวไว้เก็บ gameobject โมเดล เเล้วเช็ค if currentEquipment เเล้วปิดโมเดลอันที่ไม่ใช้
    }

    void OnDestroy() 
    {
        RemoveInputListener();
    }

    public void AddOnUseCameraAction(UnityAction action)
    {
        CameraEquipment cameraEquipment = equipment.Find(n => n.GetComponent<CameraEquipment>() != null).GetComponent<CameraEquipment>();
        cameraEquipment.onUse.AddListener(action);
    }

    void SwitchEquipment(int index)
    {
        currentEquipment = equipment[index].equipmentType;
    }
}
