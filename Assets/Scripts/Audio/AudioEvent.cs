using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

public class AudioEvent : Singleton<AudioEvent>
{
    [field : SerializeField] public List<AudioEventConfig> eventList {get; private set;}

    public Dictionary<string,EventReference>  audioEventDictionary = new Dictionary<string, EventReference>();
    [ReadOnly] public  string jumpscare = "jumpScare";
    
    protected override void InitAfterAwake()
    {
        foreach(var n in eventList)
        {
            audioEventDictionary.Add(n.key,n.sound);    
        }
    }


    
}
