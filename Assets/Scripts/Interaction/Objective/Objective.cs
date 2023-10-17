using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : MonoBehaviour
{
    protected bool CheckObjective()
    {
        return MainObjectiveManager.Instance.GetCheckObjective(this);
    }

}
