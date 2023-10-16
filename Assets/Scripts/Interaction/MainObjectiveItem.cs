using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainObjectiveItem : InteractObject
{
    public override void OnInteract()
    {
        print("Interact");
        CheckObjective();
    }
    void CheckObjective()
    {
        MainObjectiveManager.Instance.GetCheckObjective(gameObject);
    }

}
