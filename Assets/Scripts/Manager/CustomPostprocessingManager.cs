using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HorrorJam.Audio;
using UnityEngine;

public class CustomPostprocessingManager : Singleton<CustomPostprocessingManager>
{
   public Fog fog;
   public Camera postProcessingStack;
   public VHSPostProcessEffect vhsEffect;
   public AcerolaLensFlare Flare;
   protected override void InitAfterAwake()
   {
      fog = FindFirstObjectByType<Fog>();
   }

   public void DeadSequnce()
   {
      fog.fogDensity = 0.057f;
      fog.fogOffset = 0;
      fog.fogColor = Color.white;
      PlayerManager.Instance.KilledByEnemy.Stopsound();
      PlayerManager.Instance.KilledByEnemy.PlayJumpScareSound();
      DOTween.To(() => postProcessingStack.rect.height, x => postProcessingStack.rect = new Rect(Vector2.zero, new Vector2(1,x)), 1f, 0.314f)
         .OnComplete((() => { TweenSequnce2();}));
   }

   void TweenSequnce2()
   {
      var afterTrigger  = DOTween.Sequence();
      afterTrigger.AppendInterval(4.8f).AppendCallback(() =>
      {
         PlayerManager.Instance.KilledByEnemy.JumpScareEventInstance.setParameterByName("IsJumpscareloop1", 1);
         vhsEffect.enabled = false ;
         Flare.enabled = true;
      }).AppendInterval(1.5f).AppendCallback(() =>
      {
         AudioManager.Instance.StopAudio(AudioEvent.Instance.jumpscare);
         GameManager.Instance.OnChangeGameStage(GameStage.Pause); 
         Flare.enabled = false;GameOverPanel.Instance.ShowPanelUp();
      });
   }
}
