using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class PipboyManager: Singleton<PipboyManager>
{
    public MeshRenderer pipboymesh;
    public GameObject meshGroup;
    public CanvasGroup group;

    [SerializeField] GameObject pipboyObject;
    [SerializeField] Ease screenEase;
    [SerializeField] float fadeinduration = 1f;
    [SerializeField] Vector3 startpos;
    [SerializeField] Vector3 endpose;
    Tween pipboytween;
    Material tempScreenmat;
    Vector3 sparkleprop;
    Vector3 sparkleprop_temp;

    bool isUsingPipboy ;
    bool FadeInDone;

    public bool IsUsingPipboy => isUsingPipboy;
    
    protected override void InitAfterAwake()
    {
        tempScreenmat = pipboymesh.materials[0];
        tempScreenmat = Instantiate(tempScreenmat);
        pipboymesh.materials[0] = tempScreenmat;
    }

    void AddInputListener()
    {
        InputSystemManager.Instance.onUseArmConsole += togglepipboy;
    }

    void RemoveInputListener()
    {
        InputSystemManager.Instance.onUseArmConsole -= togglepipboy;
    }
    
    private void Start()
    {
        sparkleprop_temp = pipboymesh.materials[0].GetVector("_SparkleTCI");
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
    PlayerManager.Instance.OnChangePlayerState(PlayerState.PipBoy);
    meshGroup.SetActive(true);
    pipboymesh.materials[0] = tempScreenmat;
    sparkleprop = sparkleprop_temp;
    pipboytween.Kill();
    pipboytween = DOTween.To(() => sparkleprop.x, x => sparkleprop.x = x, sparkleprop.x+20f, fadeinduration).SetEase(screenEase).OnStart(SparkleSequence).OnUpdate(UpdateMaterial).OnComplete(fadeInAlpha);


    pipboyObject.transform.localPosition = startpos;
    pipboyObject.transform.DOLocalMove(endpose, fadeinduration*0.95f).SetEase(screenEase).OnComplete((() => FadeInDone = true));
    pipboyObject.transform.localRotation = Quaternion.Euler(new Vector3(0,-90,-90));
    pipboyObject.transform.DOLocalRotate(
        new Vector3(0, -90, 0f), fadeinduration).SetEase(screenEase);
    }

    [Button]
    public void QuitPipboy()
    {
        if(!Application.isPlaying)return;
    
        group.interactable = false;
        PlayerManager.Instance.OnChangePlayerState(PlayerState.Move);
        fadeOutAlpha();
        pipboyObject.transform.DOLocalRotate(new Vector3(0, -90, -90f), fadeinduration).SetEase(screenEase);
        pipboyObject.transform.DOLocalMove(startpos, fadeinduration*0.95f).SetEase(screenEase).OnComplete( ()=>
        {
            meshGroup.SetActive(false);
            isUsingPipboy = false;
            FadeInDone = false;
            PlayerManager.Instance.PlayerEquipment.SwitchPipboyToEquipment();
        });
    }

    private void fadeInAlpha()
    {
    MessageManager.Instance.HideNotificationText();
        Tween fadein ;
        fadein = DOTween.To(()=> group.alpha, x => group.alpha = x, 1, fadeinduration*0.35f).SetEase(screenEase);
    }

    private void fadeOutAlpha()
    {
        Tween fadein ;
        fadein = DOTween.To(()=> group.alpha, x => group.alpha = x, 0, fadeinduration ).SetEase(screenEase);
    }

    Tween _fadein ;
    public void PanelTransition()
    { 
        _fadein.Kill();
        _fadein = DOTween.To(()=> group.alpha, x => group.alpha = x, 0, 0 ).SetEase(screenEase).OnStart(resetdefaultmat);
    }

    private void resetdefaultmat()
    { 
        Debug.Log("working");
        pipboymesh.materials[0] = tempScreenmat;
    sparkleprop = sparkleprop_temp;
    DOTween.To(() => sparkleprop.x, x => sparkleprop.x = x, sparkleprop.x+20f, fadeinduration*2f).SetEase(screenEase).OnStart(SparkleSequence).OnUpdate(UpdateMaterial).OnComplete(fadeInAlpha);
    }

    private void SparkleSequence()
    {
        DOVirtual.Vector3(sparkleprop, new Vector3(sparkleprop.x, 1f, 0.1f), fadeinduration, v => sparkleprop = v).SetEase(screenEase).OnComplete(()=> sparkleprop = sparkleprop_temp);
    }

    private void UpdateMaterial()
    {
        
        pipboymesh.materials[0].SetVector("_SparkleTCI",sparkleprop);
    }

    private void OnDestroy()
    {
        RemoveInputListener();
    }
}
