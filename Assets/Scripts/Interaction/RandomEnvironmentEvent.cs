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
            AudioManager.Instance.PlayAudioOneShot(eventInfo.soundId);
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
    public string soundId;
    public GameObject moveObject;
    public Transform moveToPosition;
    public Ease ease;
}
