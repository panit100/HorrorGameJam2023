using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Objective : MonoBehaviour
{
    [SerializeField]protected string objectiveCode;
    [SerializeField]protected UnityEvent unityEvent;

    public bool CheckObjective()
    {
        return MainObjectiveManager.Instance.isEqualCurrentObjective(objectiveCode);
    }
}
