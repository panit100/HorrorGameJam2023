using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System;
using HorrorJam.Audio;

public class RandomEnvironmentEvent : MonoBehaviour
{
    [HideInInspector] public List<UnityEvent> randomEventList;
    [SerializeField] List<EventInfo> eventInfoList;
    
    private void Start()
    {
        AddEventInfoToRandomEventList();
    }

    void AddEventInfoToRandomEventList()
    {
        foreach (EventInfo info in eventInfoList)
        {
            UnityEvent addEvent = new UnityEvent();
            addEvent.AddListener(() => EnvironmentEvent(info));
            randomEventList.Add(addEvent);
        }
    }

    void EnvironmentEvent(EventInfo eventInfo)
    {
        if (!string.IsNullOrEmpty(eventInfo.soundId))
        {
            if(eventInfo.soundPosition != null)
            {
                AudioManager.Instance.PlayAudioOneShot(eventInfo.soundId,eventInfo.soundPosition.transform.position);
                print("Play Area Sound : " + eventInfo.soundId);
            }
            else
            {
                AudioManager.Instance.PlayAudioOneShot(eventInfo.soundId);
                print("Play Sound : " + eventInfo.soundId);
            }
        }

        if (eventInfo.moveObject != null)
        {
            Vector3 movePosition = eventInfo.moveToPosition.transform.position;
            eventInfo.moveObject.transform.DOMove(movePosition,2f).SetEase(eventInfo.ease);
        }
    }

    void RandomEvent()
    {
        int randomNumber = UnityEngine.Random.Range(0, randomEventList.Count);
        if (randomNumber <= randomEventList.Count)
        {
            randomEventList[randomNumber].Invoke();
            print("PlayEvent");
        }
        else
            print("Notting Happen");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RandomEvent();
            gameObject.SetActive(false);
        }
    }
}
[Serializable]
public class EventInfo{
    [Header("Sound Event")]
    public string soundId;
    public Transform soundPosition;
    [Header("Object Event")]
    public GameObject moveObject;
    public Transform moveToPosition;
    public Ease ease;
}
