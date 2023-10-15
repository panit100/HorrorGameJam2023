using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Door : InteractObject
{
    [SerializeField] float moveValue = 5f;
    [SerializeField] float duration = 1f;
    Vector3 originPos;

    public Ease ease;

    void Start()
    {
        originPos = transform.position;
    }

    [Button]
    void OpenDoor()
    {
        transform.DOMoveY(transform.position.y+moveValue,duration).SetEase(ease);
    }

    [Button]
    void CloseDoor()
    {
        transform.DOMoveY(originPos.y,duration).SetEase(ease);;
    }

    public override void OnInteract()
    {
        OpenDoor();
    }
}
