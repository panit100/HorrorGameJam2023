using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective : MonoBehaviour
{
    [SerializeField]protected string objectiveCode;

    public bool CheckObjective()
    {
        return MainObjectiveManager.Instance.isEqualCurrentObjective(objectiveCode);
    }

}
