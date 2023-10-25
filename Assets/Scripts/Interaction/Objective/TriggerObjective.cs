using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerObjective : Objective
{
    [SerializeField] UnityEvent unityEvent;
    
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    void OnTriggerEnter(Collider other) 
    {
        if(!other.CompareTag("Player"))
            return;

        if(CheckObjective())
        {
            MainObjectiveManager.Instance.UpdateProgress(objectiveCode);
            unityEvent?.Invoke();
            gameObject.SetActive(false);
        }

    }
}
