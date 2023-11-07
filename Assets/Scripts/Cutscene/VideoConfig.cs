using System;
using UnityEngine.Video;

[Serializable]
public class VideoConfig
{
    public string ID;
    public VideoClip video;
    public string videoAudioID; //Use it to play sound in video because when implement fmod to project, old listener is must to be remove and instant with fmod listener
}
