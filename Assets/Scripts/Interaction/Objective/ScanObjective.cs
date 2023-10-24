using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scanable))]
public class ScanObjective : Objective
{
    Scanable scanable;

    private void Start() {
        if(TryGetComponent<Scanable>(out Scanable _scanable))
        {
            scanable = _scanable;
            scanable.onScanComplete += OnScanFinish;
        }
        else
        {
            scanable = null;
        }
    }   

    public void OnScanFinish()
    {
        if(!scanable.AlreadyScan)
            return;
        
        if(CheckObjective())
        {
            MainObjectiveManager.Instance.UpdateProgress(objectiveCode);
            gameObject.SetActive(false);
        }
    }
}
