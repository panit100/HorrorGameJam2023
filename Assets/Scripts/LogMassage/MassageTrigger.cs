using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class MassageTrigger : MonoBehaviour
{
    [SerializeField] string massageCode;
    [SerializeField] float massageDelay;

    string MassageId => $"Massage_{massageCode}";
    [SerializeField] UnityEvent unityEvent;

    
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    Sequence Schedule(float delay, TweenCallback callback, string id)
    {
        DOTween.Kill(id);  //TODO: Error not found id
        return DOTween.Sequence()
            .AppendInterval(delay)
            .AppendCallback(callback)
            .SetId(id).OnComplete(() => unityEvent?.Invoke());;
    }

    void SendLogToPipBoy()
    {
        MassageManager.Instance.AddLogData(massageCode);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            Schedule(massageDelay,SendLogToPipBoy,MassageId);
        }
    }
}
