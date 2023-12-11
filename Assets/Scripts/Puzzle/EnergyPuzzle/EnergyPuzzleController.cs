using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENERGYTYPE
{
    LIGHT,
    DOOR,
    BRIDGE,
}

public class EnergyPuzzleController : MonoBehaviour, InteractObject
{
    [Header("Canvas")]
    [SerializeField] EnergyPuzzleCanvas energyPuzzleCanvas;

    [Header("EnergyConfig")]
    //unuse battery
    [SerializeField] int maxEnergy;
    [SerializeField] int unUseEnergy;

    //max battery
    [Header("Light Config")]
    [SerializeField] EnergyConfig lightEnergyConfig;
    [SerializeField] List<Light> lights = new List<Light>();

    [Header("Door")]
    [SerializeField] EnergyConfig doorEnergyConfig;
    [SerializeField] Door door;

    [Header("Bridge")]
    [SerializeField] EnergyConfig bridgeEnergyConfig;
    // [SerializeField] Door door; //Bridge
    
    bool isActive = false;
    
    public bool AddEnergy(ENERGYTYPE energyType)
    {
        if(unUseEnergy == 0)
            return false;

        switch(energyType)
        {
            case ENERGYTYPE.DOOR:
                if(!doorEnergyConfig.AddEnergy())
                    return false;

                if(doorEnergyConfig.IsEnergyReachTargetEnergy())
                {
                    ToggleDoor(true);
                }
                break;
            case ENERGYTYPE.LIGHT:
                if(!lightEnergyConfig.AddEnergy())
                    return false;

                if(lightEnergyConfig.IsEnergyReachTargetEnergy())
                {
                    ToggleLight(true);
                }
                break;
            case ENERGYTYPE.BRIDGE :
                if(!bridgeEnergyConfig.AddEnergy())
                    return false;

                if(bridgeEnergyConfig.IsEnergyReachTargetEnergy())
                {
                    ToggleBridge(true);
                }
                break;
        }
        unUseEnergy--;
        return true;
    }

    public bool RemoveEnergy(ENERGYTYPE energyType)
    {
        if(unUseEnergy >= maxEnergy)
            return false;

        switch(energyType)
        {
            case ENERGYTYPE.DOOR:
                if(!doorEnergyConfig.RemoveEnergy())
                    return false;

                if(!doorEnergyConfig.IsEnergyReachTargetEnergy())
                {
                    ToggleDoor(false);
                }
                break;
            case ENERGYTYPE.LIGHT:
                if(!lightEnergyConfig.RemoveEnergy())
                    return false;

                if(!lightEnergyConfig.IsEnergyReachTargetEnergy())
                {
                    ToggleLight(false);
                }
                break;
            case ENERGYTYPE.BRIDGE :
                if(!bridgeEnergyConfig.RemoveEnergy())
                    return false;

                if(!bridgeEnergyConfig.IsEnergyReachTargetEnergy())
                {
                    ToggleBridge(false);
                }
                break;
        }
        unUseEnergy++;
        return true;
    }

    public void AddUnUseEnergy()
    {
        maxEnergy++;
        unUseEnergy++;
    }

    void ToggleDoor(bool toggle)
    {
        if(door == null)
            return;

        if(toggle)
            door.OpenDoorWithSound();
        else
            door.CloseDoorWithSound();
    }

    void ToggleLight(bool toggle)
    {
        foreach(var n in lights)
        {
            n.enabled = toggle;
        }
    }

    void ToggleBridge(bool toggle)
    {
        //Active bridge
    }

    public void OnInteract()
    {
        if(isActive)
            return;

        
        energyPuzzleCanvas.CreateEnergyAllSlot(maxEnergy,unUseEnergy,
                                                lightEnergyConfig.maxEnergy,lightEnergyConfig.currentEnergy,
                                                    doorEnergyConfig.maxEnergy,doorEnergyConfig.currentEnergy,
                                                        bridgeEnergyConfig.maxEnergy,bridgeEnergyConfig.currentEnergy);

        energyPuzzleCanvas.ShowCanvas(this);
        isActive = true;
        PlayerManager.Instance.OnChangePlayerState(PlayerState.puzzle);

        energyPuzzleCanvas.onHideCanvas += OnExitPuzzle;
    }

    public void OnExitPuzzle()
    {
        if(!isActive)
            return;
        
        if(PlayerManager.Instance.PlayerState != PlayerState.puzzle)
            return;

        isActive = false;
        PlayerManager.Instance.OnChangePlayerState(PlayerState.Move);
    }
}

[Serializable]
public class EnergyConfig
{
    public ENERGYTYPE energyType;
    public int maxEnergy;
    public int currentEnergy;
    public int targetEnergy;

    public bool AddEnergy()
    {
        if(currentEnergy < maxEnergy)
        {
            currentEnergy++;
            return true;
        }
        else
            return false;
    }

    public bool RemoveEnergy()
    {
        if(currentEnergy > 0)
        {
            currentEnergy--;
            return true;
        }
        else
            return false;
    }

    public bool IsEnergyReachTargetEnergy()
    {
        return currentEnergy >= targetEnergy;
    }
}
