using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class PipboyMaterialController : Singleton<PipboyMaterialController>
{
    public MeshRenderer Pipboymesh;
    public GameObject MeshGroup;
    public CanvasGroup group;

    public UnityAction allEqupment;

    [SerializeField] Ease ScreenEase;
    [SerializeField] float fadeinduration = 1f;
    [SerializeField] private Vector3 startpos;
    [SerializeField] private Vector3 endpose;
     Tween pipboytween;
     private Material TempScreenmat;
     private Vector3 sparkleprop;
     private Vector3 sparkleprop_temp;
   
     private bool isUsingPipboy ;
     private bool FadeInDone;

     public bool IsUsingPipboy => isUsingPipboy;
     
     protected override void InitAfterAwake()
     {
         
     }
     void AddInputListener()
     {
         InputSystemManager.Instance.onUseArmConsole += togglepipboy;
     }

     void RemoveInputListener()
     {
         InputSystemManager.Instance.onUseArmConsole -= togglepipboy;
     }
     private void Awake()
     {
       
         TempScreenmat = Pipboymesh.materials[0];
         TempScreenmat = Instantiate(TempScreenmat);
         Pipboymesh.materials[0] = TempScreenmat;
        
     }

     private void Start()
     {
         sparkleprop_temp = Pipboymesh.materials[0].GetVector("_SparkleTCI");
         AddInputListener();
     }

     void togglepipboy()
     {
        if (!isUsingPipboy && !FadeInDone)
            StartPipboy();
        else
            QuitPipboy();
     }
     
     [Button]
     public void StartPipboy()
     { 
         
         if(!Application.isPlaying)return;
         group.interactable = true;
         isUsingPipboy = true;
         GameManager.Instance.OnChangeGameStage(GameStage.OnPipboy);
         MeshGroup.SetActive(true);
         Pipboymesh.materials[0] = TempScreenmat;
         sparkleprop = sparkleprop_temp;
         pipboytween.Kill();
         pipboytween = DOTween.To(() => sparkleprop.x, x => sparkleprop.x = x, sparkleprop.x+20f, fadeinduration).SetEase(ScreenEase).OnStart(SparkleSequence).OnUpdate(UpdateMaterial).OnComplete(fadeInAlpha);

     
         transform.localPosition = startpos;
         transform.DOLocalMove(endpose, fadeinduration*0.95f).SetEase(ScreenEase).OnComplete((() => FadeInDone = true));
        this.transform.localRotation = Quaternion.Euler(new Vector3(0,-90,-90));
        transform.DOLocalRotate(
            new Vector3(0, -90, 0f), fadeinduration).SetEase(ScreenEase);

     
     }
    
     [Button]
     public void QuitPipboy()
     {
         if(!Application.isPlaying)return;
       
         group.interactable = false;
         GameManager.Instance.OnChangeGameStage(GameStage.Playing);
         fadeOutAlpha();
         transform.DOLocalRotate(new Vector3(0, -90, -90f), fadeinduration).SetEase(ScreenEase);
         transform.DOLocalMove(startpos, fadeinduration*0.95f).SetEase(ScreenEase).OnComplete( ()=>
         {
             MeshGroup.SetActive(false);
             isUsingPipboy = false;
             FadeInDone = false;
         });
     }

     private void fadeInAlpha()
     {
        MassageManager.Instance.HideNotificationText();
         Tween fadein ;
         fadein = DOTween.To(()=> group.alpha, x => group.alpha = x, 1, fadeinduration*0.35f).SetEase(ScreenEase);
     }
     private void fadeOutAlpha()
     {
         Tween fadein ;
         fadein = DOTween.To(()=> group.alpha, x => group.alpha = x, 0, fadeinduration ).SetEase(ScreenEase);
     }
     Tween _fadein ;
     public void PanelTransition()
     { 
         _fadein.Kill();
         _fadein = DOTween.To(()=> group.alpha, x => group.alpha = x, 0, 0 ).SetEase(ScreenEase).OnStart(resetdefaultmat);
     }

     private void resetdefaultmat()
     { 
         Debug.Log("working");
         Pipboymesh.materials[0] = TempScreenmat;
        sparkleprop = sparkleprop_temp;
        DOTween.To(() => sparkleprop.x, x => sparkleprop.x = x, sparkleprop.x+20f, fadeinduration*2f).SetEase(ScreenEase).OnStart(SparkleSequence).OnUpdate(UpdateMaterial).OnComplete(fadeInAlpha);
     }
     private void SparkleSequence()
     {
         DOVirtual.Vector3(sparkleprop, new Vector3(sparkleprop.x, 1f, 0.1f), fadeinduration, v => sparkleprop = v).SetEase(ScreenEase).OnComplete(()=> sparkleprop = sparkleprop_temp);
     }
     private void UpdateMaterial()
     {
         
         Pipboymesh.materials[0].SetVector("_SparkleTCI",sparkleprop);
     }

     private void OnDestroy()
     {
         RemoveInputListener();
     }
}
