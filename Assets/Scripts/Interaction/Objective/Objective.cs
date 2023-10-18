using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : MonoBehaviour
{
    [SerializeField] string objectiveCode;

    protected bool CheckObjective()
    {
        return MainObjectiveManager.Instance.GetCheckObjective(objectiveCode);
    }

}
