using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentSingleton<T> : Singleton<T> where T : Singleton<T>
{
    protected override void InitAfterAwake()
    {

    }

    protected override void Init()
    {
        base.Init();
        if (Application.isPlaying)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
