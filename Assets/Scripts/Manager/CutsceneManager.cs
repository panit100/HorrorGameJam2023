using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using LlockhamIndustries.Misc;
using LogMassage;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneManager : Singleton<CutsceneManager>
{
    public List<VideoClip> Cutscene = new List<VideoClip>();
    public VideoPlayer VdoPlayer;
    private UIMessageNotification skipimage;
    
    private Dictionary<string, VideoClip> CutsceneID = new Dictionary<string, VideoClip>();

    private String currentCutscene;
    // Start is called before the first frame update

    protected override void InitAfterAwake()
    {
        VdoPlayer.loopPointReached += EndOfvideo;
        foreach (var VARIABLE in Cutscene)
        {
            CutsceneID.Add(VARIABLE.name,VARIABLE);
        }
      
    }

    public void initCutscene()
    {
        if (VdoPlayer == null)
        {
            VdoPlayer = FindObjectOfType<VideoPlayer>();
            skipimage = FindObjectOfType<UIMessageNotification>();
        }
        InputSystemManager.Instance.onSkipcutscene += SkipEndOfvideo;
        DOTween.Sequence().AppendCallback((() => skipimage.PlayEnter())).AppendInterval(2f)
            .AppendCallback(() => skipimage.PlayExit());
    }

 

    // Update is called once per frame
    void Update()
    {
        
    }
    
    [Button]
    public void Playcutscene(string EventID = "init")
    {
        currentCutscene = EventID;
        if (EventID == "init")
        {
            VdoPlayer.clip = CutsceneID[Cutscene[0].name];
        }
        else
        {
            VdoPlayer.clip = CutsceneID[EventID];
        }
        VdoPlayer.Play();
    }

    public void EndOfvideo(UnityEngine.Video.VideoPlayer vp)
    {
        if (currentCutscene == "As_ending")
        {
            StartCoroutine(  GameManager.Instance.GoToSceneMainMenu());
        }

        if (currentCutscene == "As_intro")
        {
            StartCoroutine(GameManager.Instance.GoToSceneGame());
        }
    }
    public void SkipEndOfvideo()
    {
        if (currentCutscene == "As_ending")
        {
            StartCoroutine(  GameManager.Instance.GoToSceneMainMenu());
        }

        if (currentCutscene == "As_intro")
        {
            StartCoroutine(GameManager.Instance.GoToSceneGame());
        }
        InputSystemManager.Instance.onSkipcutscene -= SkipEndOfvideo;
    }

    private void OnDestroy()
    {
        VdoPlayer.loopPointReached -= EndOfvideo;
    }
}
