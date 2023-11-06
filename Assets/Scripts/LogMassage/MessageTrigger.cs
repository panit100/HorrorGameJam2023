using DG.Tweening;
using HorrorJam.Audio;
using UnityEngine;
using UnityEngine.Events;

public class MessageTrigger : MonoBehaviour
{
    [SerializeField] string massageCode;
    [SerializeField] float massageDelay;

    [Header("Event")]
    [SerializeField] UnityEvent unityEvent;

    string MassageId => $"Massage_{massageCode}";
    
    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

    Sequence Schedule(float delay, TweenCallback callback, string id)
    {
        DOTween.Kill(id); 
        return DOTween.Sequence()
            .AppendInterval(delay)
            .AppendCallback(callback)
            .SetId(id).OnComplete(() => {unityEvent?.Invoke();});;
    }

    void SendLogToPipBoy()
    {
        MessageManager.Instance.AddLogData(massageCode);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            Schedule(massageDelay,SendLogToPipBoy,MassageId);
        }
    }

    public void PlayAudio(string audioID)
    {
        AudioManager.Instance.PlayAudioOneShot(audioID);
    }
}
