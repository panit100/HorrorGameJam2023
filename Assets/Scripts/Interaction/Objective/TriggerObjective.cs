using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObjective : Objective
{

    void OnTriggerEnter(Collider other) 
    {
        CheckObjective();
    }
}
