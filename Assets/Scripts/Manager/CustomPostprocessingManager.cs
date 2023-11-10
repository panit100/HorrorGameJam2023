using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
      DOTween.To(() => postProcessingStack.rect.height, x => postProcessingStack.rect = new Rect(Vector2.zero, new Vector2(1,x)), 1f, 0.314f)
         .OnComplete((() => { TweenSequnce2();}));
   }

   void TweenSequnce2()
   {
      var afterTrigger  = DOTween.Sequence();
      afterTrigger.AppendInterval(2.5f).AppendCallback((() => { vhsEffect.enabled = false ;
         Flare.enabled = true;
      })).AppendInterval(1.5f).AppendCallback((() => {GameManager.Instance.OnChangeGameStage(GameStage.Pause); Flare.enabled = false;GameOverPanel.Instance.ShowPanelUp();}));
   }
}
