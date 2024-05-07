using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAudioEvent", menuName = "AduioEvent/CraeteAudioEvent")]
public class AudioEventScriptableObject : ScriptableObject
{
    public List<AudioEventConfig> eventList;
}
