using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoadSceneTrigger : MonoBehaviour
{
    [SerializeField] string loadSceneName;
    [SerializeField] UnityEvent eventBeforeSwitchScene;
    [SerializeField] UnityEvent eventAfterSwitchScene;

    void Start()
    {
        //GetComponent<MeshRenderer>().enabled = false;
    }

    void LoadSceneOnTrigger()
    {
        SceneController.Instance.OnLoadSceneAsync(loadSceneName,eventBeforeSwitchScene,eventAfterSwitchScene);
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            LoadSceneOnTrigger();
        }    
    }
}
