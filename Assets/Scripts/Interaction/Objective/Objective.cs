using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : MonoBehaviour
{
    protected void CheckObjective()
    {
        MainObjectiveManager.Instance.GetCheckObjective(this);
    }

}
