using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainObjectiveItem : InteractObject
{
    Scanable scanable;

    private void Start() {
        if(TryGetComponent<Scanable>(out Scanable _scanable))
            scanable = _scanable;
        else
            scanable = null;
    }   

    public override void OnInteract()
    {
        print("1");

        if(scanable != null)
            if(!scanable.AlreadyScan)
                return;

        print("2");
        
        CheckObjective();
    }
    void CheckObjective()
    {
        MainObjectiveManager.Instance.GetCheckObjective(this);
    }

}
