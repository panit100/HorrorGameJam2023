using System.Collections.Generic;
using DG.Tweening;
using LogMassage;
using Sirenix.OdinInspector;
using UnityEngine.Video;

public class CutsceneManager : Singleton<CutsceneManager>
{
    public List<VideoConfig> Cutscene = new List<VideoConfig>();
    public VideoPlayer VdoPlayer;
    private UIMessageNotification skipimage;
    
    private string currentCutscene;

    protected override void InitAfterAwake()
    {
        VdoPlayer.loopPointReached += EndOfvideo;
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
    
    [Button]
    public void Playcutscene(string EventID = "init")
    {
        currentCutscene = EventID;

        if(Cutscene.Find(x=>x.ID== currentCutscene) != null)
            VdoPlayer.clip = Cutscene.Find(x=>x.ID== currentCutscene).video;
        else
            VdoPlayer.clip = Cutscene[0].video ;
        
        VdoPlayer.Prepare();
        VdoPlayer.prepareCompleted +=  (source =>  VdoPlayer.Play());
    }

    public void EndOfvideo(VideoPlayer vp)
    {
        VdoPlayer.Stop();
        if (currentCutscene == "As_ending")
            StartCoroutine(GameManager.Instance.GoToSceneMainMenu());

        if (currentCutscene == "As_intro")
            StartCoroutine(GameManager.Instance.GoToSceneGame());
    }
    public void SkipEndOfvideo()
    {
        VdoPlayer.Stop();
        if (currentCutscene == "As_ending")
            StartCoroutine(  GameManager.Instance.GoToSceneMainMenu());

        if (currentCutscene == "As_intro")
            StartCoroutine(GameManager.Instance.GoToSceneGame());

        InputSystemManager.Instance.onSkipcutscene -= SkipEndOfvideo;
    }

    private void OnDestroy()
    {
        VdoPlayer.loopPointReached -= EndOfvideo;
    }
}
