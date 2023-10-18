using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Scanable))]
public class ScanObjective : Objective, InteractObject
{
    Scanable scanable;

    private void Start() {
        if(TryGetComponent<Scanable>(out Scanable _scanable))
            scanable = _scanable;
        else
            scanable = null;
    }   

    public void OnInteract()
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
