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
    public GameObject moveObject;
    public Transform moveToPosition;
    private void Start()
    {
        AddEventInfoToRandomEventList();
        //RandomEvent();
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
        if (eventInfo.soundId != null)
        {
            AudioManager.Instance.PlayAudioOneShot(eventInfo.soundId);
        }

        if (eventInfo.moveObject != null)
        {
            print("OK");
            Vector3 movePosition = eventInfo.moveToPosition.transform.position;
            eventInfo.moveObject.transform.DOMove(movePosition,5);
        }
    }
    void RandomEvent()
    {
        int randomNumber = UnityEngine.Random.Range(0, randomEventList.Count + 5);
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
}
