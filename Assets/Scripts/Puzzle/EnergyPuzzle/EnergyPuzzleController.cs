using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENERGYTYPE
{
    LIGHT,
    DOOR,
}

public class EnergyPuzzleController : MonoBehaviour
{
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
    

    void AddEnergy(ENERGYTYPE energyType)
    {
        if(unUseEnergy == 0)
            return;

        switch(energyType)
        {
            case ENERGYTYPE.DOOR:
                doorEnergyConfig.AddEnergy();
                if(doorEnergyConfig.IsEnergyReachTargetEnergy())
                {
                    ToggleDoor(true);
                }
                break;
            case ENERGYTYPE.LIGHT:
                lightEnergyConfig.AddEnergy();
                if(lightEnergyConfig.IsEnergyReachTargetEnergy())
                {
                    ToggleLight(true);
                }
                break;
        }
        unUseEnergy--;
    }

    void RemoveEnergy(ENERGYTYPE energyType)
    {
        if(unUseEnergy >= maxEnergy)
            return;

        switch(energyType)
        {
            case ENERGYTYPE.DOOR:
                doorEnergyConfig.RemoveEnergy();
                if(!doorEnergyConfig.IsEnergyReachTargetEnergy())
                {
                    ToggleDoor(false);
                }
                break;
            case ENERGYTYPE.LIGHT:
                lightEnergyConfig.RemoveEnergy();
                if(!lightEnergyConfig.IsEnergyReachTargetEnergy())
                {
                    ToggleLight(false);
                }
                break;
        }
        unUseEnergy++;
    }

    void ToggleDoor(bool toggle)
    {
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
}

[Serializable]
public class EnergyConfig
{
    public ENERGYTYPE energyType;
    public int maxEnergy;
    public int currentEnergy;
    public int targetEnergy;

    public void AddEnergy()
    {
        if(currentEnergy < maxEnergy)
        {
            currentEnergy++;
        }
    }

    public void RemoveEnergy()
    {
        if(currentEnergy > 0)
        {
            currentEnergy--;
        }
    }

    public bool IsEnergyReachTargetEnergy()
    {
        return currentEnergy >= targetEnergy;
    }
}