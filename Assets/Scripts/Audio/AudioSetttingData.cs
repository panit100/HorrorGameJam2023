using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioSetttingData
{
    [Range(0f,1f)]
    public float masterVolume = 1;

    [Range(0f,1f)]
    public float musicVolume = 1;

    [Range(0f,1f)]
    public float sfxVolume = 1;

    public void Save()
    {
        JsonHelper.SaveJSONAsObject("AudioSetting",this);
    }

    static public AudioSetttingData Load()
    {   
        return JsonHelper.LoadUserJSONAsObject<AudioSetttingData>("AudioSetting");
    }
}
