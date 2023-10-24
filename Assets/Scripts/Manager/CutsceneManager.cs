using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneManager : MonoBehaviour
{
    public List<VideoClip> Cutscene = new List<VideoClip>();
    public VideoPlayer VdoPlayer;
    
    private Dictionary<string, VideoClip> CutsceneID = new Dictionary<string, VideoClip>();
    // Start is called before the first frame update
    private void Awake()
    {
        if (VdoPlayer == null)
        {
            VdoPlayer = FindObjectOfType<VideoPlayer>();
        }
    }

    void Start()
    {
        VdoPlayer.loopPointReached += EndOfvideo;
        foreach (var VARIABLE in Cutscene)
        {
            CutsceneID.Add(VARIABLE.name,VARIABLE);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Playcutscene(string EventID = "init")
    {
        if (EventID == "init")
        {
            VdoPlayer.clip = CutsceneID[Cutscene[0].name];
        }
        VdoPlayer.clip = CutsceneID[EventID];
        VdoPlayer.Play();
    }

    public void EndOfvideo(UnityEngine.Video.VideoPlayer vp)
    {
        
    }
}
