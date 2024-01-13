using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnergyPuzzleController : MonoBehaviour, InteractObject
{
    [Header("Canvas")]
    [SerializeField] EnergyPuzzleCanvas energyPuzzleCanvas;

    [Header("EnergyConfig")]
    //unuse battery
    [SerializeField] int maxEnergy;
    [SerializeField] int unUseEnergy;

    //max battery
    [Header("Energy Config")]
    [SerializeField] List<EnergyConfig> energyConfigs = new List<EnergyConfig>();
    public List<EnergyConfig> EnergyConfigs {get { return energyConfigs;}}

    bool isActive = false;
    
    public bool AddEnergy(int index)
    {
        if(unUseEnergy == 0)
            return false;

        if(!energyConfigs[index].AddEnergy())
            return false;

        if(!energyConfigs[index].isAlreadyActive)
        {
            if(energyConfigs[index].IsEnergyReachTargetEnergy())
            {
                energyConfigs[index].OnActive();
            }
        }

        unUseEnergy--;
        return true;
    }

    public bool RemoveEnergy(int index)
    {
        if(unUseEnergy >= maxEnergy)
            return false;

        if(!energyConfigs[index].RemoveEnergy())
            return false;

        if(energyConfigs[index].isAlreadyActive)
        {
            if(!energyConfigs[index].IsEnergyReachTargetEnergy())
            {
                energyConfigs[index].OnDeactive();
            }
        }

        unUseEnergy++;
        return true;
    }

    public void AddUnUseEnergy()
    {
        maxEnergy++;
        unUseEnergy++;
    }

    public void OnInteract()
    {
        if(isActive)
            return;

        
        energyPuzzleCanvas.CreateEnergyAllSlot(maxEnergy,unUseEnergy,energyConfigs);

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

    public void DebugLog(string log)
    {
        print(log);
    }
}

[Serializable]
public class EnergyConfig
{
    public int maxEnergy;
    public int currentEnergy;
    public int targetEnergy;
    public bool isAlreadyActive = false;
    public UnityEvent eventOnActive;
    public UnityEvent eventOnDeactive;

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

    public void OnActive()
    {
        eventOnActive?.Invoke();
        isAlreadyActive = true;
    }

    public void OnDeactive()
    {
        eventOnDeactive?.Invoke();
        isAlreadyActive = false;
    }
}
