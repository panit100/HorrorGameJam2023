using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class AudioEvent : Singleton<AudioEvent>
{
    [field : SerializeField] public List<AudioEventConfig> eventList {get; private set;}

    public Dictionary<string,EventReference>  audioEventDictionary = new Dictionary<string, EventReference>();
    
    
    
    protected override void InitAfterAwake()
    {
        foreach(var n in eventList)
        {
            audioEventDictionary.Add(n.key,n.sound);    
        }
    }


    
}
