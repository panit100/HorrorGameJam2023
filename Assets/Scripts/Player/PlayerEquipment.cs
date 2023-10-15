using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    void RemoveInputListener()
    {
        InputSystemManager.Instance.onUseEquipment -= OnUseEquipment;
    }

    void OnUseEquipment()
    {
        equipment.Find(n => n.equipmentType == currentEquipment).OnUse();
    }

    void OnDestroy() 
    {
        RemoveInputListener();
    }
}
