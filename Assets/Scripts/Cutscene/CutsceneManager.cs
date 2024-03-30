using System.Collections.Generic;
using DG.Tweening;
using HorrorJam.Audio;
using LogMassage;
using Sirenix.OdinInspector;
using UnityEngine.Video;

public class CutsceneManager : PersistentSingleton<CutsceneManager>
{
    public List<VideoConfig> Cutscene = new List<VideoConfig>();
    public VideoPlayer VdoPlayer;
    private UIMessageNotification skipimage;

    private string currentCutscene;

    string playingAudioID;

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

        VideoConfig _videoConfig;

        if (Cutscene.Find(x => x.ID == currentCutscene) != null)
            _videoConfig = Cutscene.Find(x => x.ID == currentCutscene);
        else
            _videoConfig = Cutscene[0];

        VdoPlayer.clip = _videoConfig.video;
        VdoPlayer.Prepare();
        VdoPlayer.prepareCompleted += (source => { VdoPlayer.Play(); PlayVideoAudio(_videoConfig.videoAudioID); });
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
        StopVideoAudio();
        VdoPlayer.Stop();
        if (currentCutscene == "As_ending")
            StartCoroutine(GameManager.Instance.GoToSceneMainMenu());

        if (currentCutscene == "As_intro")
            StartCoroutine(GameManager.Instance.GoToSceneGame());

        InputSystemManager.Instance.onSkipcutscene -= SkipEndOfvideo;
    }

    private void OnDestroy()
    {
        VdoPlayer.loopPointReached -= EndOfvideo;
    }

    void PlayVideoAudio(string id)
    {
        playingAudioID = id;
        AudioManager.Instance.PlayAudio(id);
    }

    void StopVideoAudio()
    {
        AudioManager.Instance.StopAudio(playingAudioID);
    }
}
