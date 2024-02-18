using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using HorrorJam.Audio;

public class Door : MonoBehaviour
{
    [SerializeField] Vector3 movePos;
    [SerializeField] float duration = 1f;
    [SerializeField] Ease ease;
    [SerializeField] bool isDoorOpen = false;
    [SerializeField] DoorSwitch doorSwitch;

    Vector3 originPos;

    
    string doorID => gameObject.name + "_ID";
    void Start()
    {
        originPos = transform.position;
    }

    [Button]
    public void OpenDoor()
    {
        if(isDoorOpen == true)
            return;
            
        isDoorOpen = true;

        DOTween.Kill(doorID);
        transform
            .DOMove(transform.position + movePos, duration)
            .SetEase(ease)
            .SetId(doorID);
    }

    [Button]
    public void CloseDoor()
    {
        if(isDoorOpen == false)
            return;

        isDoorOpen = false;

        DOTween.Kill(doorID);
        transform
            .DOMove(originPos, duration)
            .SetEase(ease)
            .SetId(doorID);
    }

    public void PlayAudioAtPosition(string audioID)
    {
        AudioManager.Instance.PlayAudioOneShot(audioID);
    }

    public void OpenDoorWithSound()
    {
        OpenDoor();
        PlayAudioAtPosition("door_open");
    }

    public void CloseDoorWithSound()
    {
        CloseDoor();
        PlayAudioAtPosition("template");
    }
}
