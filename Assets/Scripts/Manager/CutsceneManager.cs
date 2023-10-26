using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using LlockhamIndustries.Misc;
using LogMassage;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Video;

public class CutsceneManager : Singleton<CutsceneManager>
{
    public List<videolist> Cutscene = new List<videolist>();
    public VideoPlayer VdoPlayer;
    private UIMessageNotification skipimage;
    
   

    private String currentCutscene;
    // Start is called before the first frame update

    protected override void InitAfterAwake()
    {
        VdoPlayer.loopPointReached += EndOfvideo;
    }

    private void Start()
    {
      
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
            VdoPlayer.clip = Cutscene[0].video ;
        }
        else
        {
            VdoPlayer.clip = Cutscene.Find(x=>x.ID== EventID).video;
        }
        VdoPlayer.Prepare();
        VdoPlayer.prepareCompleted += (source =>  VdoPlayer.Play());
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

[Serializable]
public class videolist
{
    public string ID;
    public VideoClip video;
}
