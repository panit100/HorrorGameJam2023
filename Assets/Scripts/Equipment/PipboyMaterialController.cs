using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using Sirenix.OdinInspector;

public class PipboyMaterialController : MonoBehaviour
{
    public MeshRenderer Pipboymesh;
    
    [SerializeField] Ease ScreenEase;
    [SerializeField] float fadeinduration = 1f;
    [SerializeField] private Vector3 startpos;
    [SerializeField] private Vector3 endpose;
     Tween pipboytween;
     private Material TempScreenmat;
     private Vector3 sparkleprop;
     private Vector3 sparkleprop_temp;
   
     private bool isUsingPipboy;

     public bool IsUsingPipboy => isUsingPipboy;

     private void Awake()
     {
       
         TempScreenmat = Pipboymesh.materials[0];
         TempScreenmat = Instantiate(TempScreenmat);
         Pipboymesh.materials[0] = TempScreenmat;
         sparkleprop_temp = Pipboymesh.materials[0].GetVector("_SparkleTCI");
     }
  
     
     [Button]
     void StartPipboy()
     { 
         if(!Application.isPlaying)return;
         isUsingPipboy = true;
         Pipboymesh.materials[0] = TempScreenmat;
         sparkleprop = sparkleprop_temp;
         pipboytween.Kill();
         pipboytween = DOTween.To(() => sparkleprop.x, x => sparkleprop.x = x, sparkleprop.x+20f, fadeinduration).SetEase(ScreenEase).OnStart(SparkleSequence).OnUpdate(UpdateMaterial);

     
         transform.localPosition = startpos;
         transform.DOLocalMove(endpose, fadeinduration*0.95f).SetEase(ScreenEase);
        this.transform.rotation = Quaternion.Euler(new Vector3(0,-90,-90));
        transform.DORotate(
            new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y,
                transform.rotation.eulerAngles.z + 90f), fadeinduration ).SetEase(ScreenEase);
     
     }

     void QuitPipboy()
     {
         isUsingPipboy = false;
     }

     private void SparkleSequence()
     {
         DOVirtual.Vector3(sparkleprop, new Vector3(sparkleprop.x, 1f, 0.1f), fadeinduration, v => sparkleprop = v).SetEase(ScreenEase).OnComplete(()=> sparkleprop = sparkleprop_temp);
     }
     private void UpdateMaterial()
     {
         Pipboymesh.materials[0].SetVector("_SparkleTCI",sparkleprop);
     }

}
