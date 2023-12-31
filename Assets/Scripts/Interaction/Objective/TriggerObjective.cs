using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObjective : Objective
{

    void OnTriggerEnter(Collider other) 
    {
        if(!other.CompareTag("Player"))
            return;

        if(CheckObjective())
        {
            MainObjectiveManager.Instance.UpdateProgress(objectiveCode);
        }

    }
}
