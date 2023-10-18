using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MassageTrigger : MonoBehaviour
{
    [SerializeField] string massageCode;
    [SerializeField] float massageDelay;

    string MassageId => $"Massage_{massageCode}";
    
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    Sequence Schedule(float delay, TweenCallback callback, string id)
    {
        // DOTween.Kill(id);  //TODO: Error not found id
        return DOTween.Sequence()
            .AppendInterval(delay)
            .AppendCallback(callback)
            .SetId(id);
    }

    void SendLogToPipBoy()
    {
        MassageManager.Instance.AddLogData(massageCode);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            Schedule(massageDelay,SendLogToPipBoy,MassageId);
        }
    }
}
