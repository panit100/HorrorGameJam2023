using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassageManager : MonoBehaviour
{
    TimeSpan currentTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       currentTime += TimeSpan.FromSeconds(Time.deltaTime);
    }

    public void Test()
    {
        print(1);
    }
}
