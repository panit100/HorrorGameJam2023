using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ObjectLoader : MonoBehaviour
{
    enum LOAD_STATE
    {
        EMPTY,
        CREATEING_OBJECT,
        FINISH,
    }

    enum SHOW_STATE
    {
        HIDE,
        START_SHOW_OBJECT,
        SHOWING_OBJECT,
        FINSIH_SHOW_OBJECT,
        FINISH,
    }

    LOAD_STATE loadState = LOAD_STATE.EMPTY;
    SHOW_STATE showState = SHOW_STATE.HIDE;
    int objectCount;

    [SerializeField] List<LoadObjectConfig> loadObjectConfigs = new List<LoadObjectConfig>();

    bool visibleObject = false;

    void Update()
    {
        LoadAllObject();
        ShowAllLoadedObject();
    }

    void LoadAllObject()
    {
        if(loadState == LOAD_STATE.EMPTY)
        {
            objectCount = 0;
            loadState = LOAD_STATE.CREATEING_OBJECT;
        }

        if(loadState == LOAD_STATE.CREATEING_OBJECT)
        {
            if(objectCount < loadObjectConfigs.Count)
            {
                CreateObject();
                objectCount++;
            }
            else
            {
                loadState = LOAD_STATE.FINISH;
            }
        }
    }

    void CreateObject()
    {
        LoadObjectConfig config = loadObjectConfigs[objectCount];
        GameObject tempObject = Instantiate(config.loadObjectPrefab,config.position,Quaternion.Euler(config.rotation),transform);
        loadObjectConfigs[objectCount].loadObject = tempObject;
        tempObject.SetActive(false);
    }

    [Button]
    public void OnShowLoadedObject()
    {
        visibleObject = true;
    }

    void ShowAllLoadedObject()
    {
        if(!visibleObject)
            return;

        if(loadState != LOAD_STATE.FINISH)
            return;

        if(showState == SHOW_STATE.HIDE)
        {
            objectCount = 0;
            showState = SHOW_STATE.START_SHOW_OBJECT;
        }

        if(showState == SHOW_STATE.START_SHOW_OBJECT)
        {
            StartCoroutine(ShowLoadedObject());
        }

        if(showState == SHOW_STATE.FINSIH_SHOW_OBJECT)
        {
            objectCount++;
            if(objectCount < loadObjectConfigs.Count)
            {
                showState = SHOW_STATE.START_SHOW_OBJECT;
            }
            else
            {
                showState = SHOW_STATE.FINISH;
            }
        }   
    }

    IEnumerator ShowLoadedObject()
    {
        showState = SHOW_STATE.SHOWING_OBJECT;
        yield return new WaitForSecondsRealtime(loadObjectConfigs[objectCount].timeBeforeVisible);
        loadObjectConfigs[objectCount].loadObject.SetActive(true);
        showState = SHOW_STATE.FINSIH_SHOW_OBJECT;
    }
}

[Serializable]
public class LoadObjectConfig
{
    public GameObject loadObjectPrefab;
    public Vector3 position;
    public Vector3 rotation;
    public float timeBeforeVisible;
    [HideInInspector] public GameObject loadObject;
}
