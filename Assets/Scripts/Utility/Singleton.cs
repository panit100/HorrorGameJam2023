using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;        
    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    protected virtual void Awake() 
    {
        Init();

        InitAfterAwake();
    }

    protected abstract void InitAfterAwake();

    protected Singleton()
    {
        instance = this as T;

        if(instance == null)
        {
            Debug.LogError($"There are no Singleton {GetType().Name} in Scene.");
            instance = this as T;
        }
    }

    void Init()
    {
        var objs = FindObjectsOfType(typeof(T)) as T[];
        if (objs.Length > 1) 
        {
            throw new System.Exception($"There are 2 Singleton in Scene. Plase remove one of them.");
        }
    }
}