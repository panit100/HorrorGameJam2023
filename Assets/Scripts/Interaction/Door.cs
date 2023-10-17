using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class Door : InteractObject
{
    [SerializeField] Transform doorTransform;
    [SerializeField] float moveValue = 5f;
    [SerializeField] float duration = 1f;
    [SerializeField] Ease ease;
    [SerializeField] bool scanBeforeInteract = false;

    Vector3 originPos;
    SpriteRenderer buttonSprite;
    Scanable scanable;
    void Start()
    {
        buttonSprite = GetComponent<SpriteRenderer>();

        if(TryGetComponent<Scanable>(out Scanable _scanable))
            scanable = _scanable;

        originPos = transform.position;

        OnBeingScanned();
    }

    void Update()
    {
        OnBeingScanned();
    }

    [Button]
    void OpenDoor()
    {
        if(scanable != null)
            if(!scanable.AlreadyScan)
                return;

        doorTransform.DOMoveY(transform.position.y+moveValue,duration).SetEase(ease);
    }

    [Button]
    void CloseDoor()
    {
        doorTransform.DOMoveY(originPos.y,duration).SetEase(ease);;
    }

    public override void OnInteract()
    {
        OpenDoor();
    }

    public void OnBeingScanned()
    {
        if(scanable != null)
            MakeVisibleEnemy(scanable.scanProgress);
    }

    void MakeVisibleEnemy(float visibleValue)
    {
        float alpha = Mathf.Clamp(visibleValue / 100f,0f,1f); 

        buttonSprite.color = new Color(buttonSprite.color.r,buttonSprite.color.g,buttonSprite.color.b,alpha);
    }
}
