using HorrorJam.Audio;
using Sirenix.OdinInspector;
using UnityEngine;

public class DoorManager : Singleton<DoorManager>
{
    [SerializeField] Door[] doors;

    protected override void InitAfterAwake()
    {
        
    }

    void Start()
    {
        doors = FindObjectsOfType<Door>();
    }

    [Button]
    public void OnOpenAllDoor()
    {
        AudioManager.Instance.PlayAudioOneShot("door_openall");
        for(int i = 0; i < doors.Length; i++)
        {
            doors[i].OpenDoor();
        }
    }
}
