using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

public enum EquipmentType
{
    NONE,
    Scanner,
    Camera,
}

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] EquipmentType currentEquipment;
    [SerializeField] List<Equipment> equipment = new List<Equipment>();
    bool canUseEquipment = true;

    void Start()
    {
        AddInputListener();
        if(!equipment.IsNullOrEmpty())
            SwitchEquipment(0); //TODO: Move to OnStartGame
    }

    void AddInputListener()
    {
        InputSystemManager.Instance.onUseEquipment += OnUseEquipment;
        InputSystemManager.Instance.onUseScanner += SwitchEquipment;
        InputSystemManager.Instance.onUseCamera += SwitchEquipment;
        InputSystemManager.Instance.onUseArmConsole += OnUseArm;
    }

    void RemoveInputListener()
    {
        InputSystemManager.Instance.onUseEquipment -= OnUseEquipment;
        InputSystemManager.Instance.onUseScanner -= SwitchEquipment;
        InputSystemManager.Instance.onUseCamera -= SwitchEquipment;
        InputSystemManager.Instance.onUseArmConsole -= OnUseArm;
        
    }

    void OnUseArm()
    {
        foreach (var VARIABLE in equipment)
            VARIABLE.PutAnim();
    }

    void OnUseEquipment()
    {
        if(currentEquipment == EquipmentType.NONE)
            return;

        equipment.Find(n => n.equipmentType == currentEquipment).OnUse();
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
        if(!canUseEquipment)
            return;

        if(currentEquipment == equipment[index].equipmentType)
            return;

        Equipment tempHoldedEquipment = equipment.Find(n => n.equipmentType == currentEquipment);

        currentEquipment = equipment[index].equipmentType;
        
        if(tempHoldedEquipment != null)
            tempHoldedEquipment.PutAnim();  
        
        equipment[index].HoldAnim();
    }

    public void SwitchPipboyToEquipment()
    {
        if(currentEquipment == EquipmentType.NONE)
            return;

        equipment.Find(x => x.equipmentType == currentEquipment).HoldAnim();
    }

    public ScannerEquipment GetScanner()
    {
        return equipment.Find(x => x.equipmentType == EquipmentType.Scanner).GetComponent<ScannerEquipment>();
    }

    public CameraEquipment GetCamera()
    {
        return equipment.Find(x => x.equipmentType == EquipmentType.Camera).GetComponent<CameraEquipment>();
    }

    [Button]
    public void ToggleUseEquipment(bool toggle)
    {
        canUseEquipment = toggle;

        if(toggle)
        {
            SwitchEquipment(0);
        }
        else
        {
            Equipment tempHoldedEquipment = equipment.Find(n => n.equipmentType == currentEquipment);
            tempHoldedEquipment.PutAnim();
            currentEquipment = EquipmentType.NONE;
        }
    }
}
