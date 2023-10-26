using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HorrorJam.AI;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class DoorManager : Singleton<DoorManager>
{
    [SerializeField] Door[] doors;

    protected override void InitAfterAwake()
    {
        
    }

    void Start()
    {
        doors = GameObject.FindObjectsOfType<Door>();
    }

    [Button]
    public void OnOpenAllDoor()
    {
        for(int i = 0; i < doors.Length; i++)
        {
            doors[i].OpenDoor();
            if(i == doors.Length-1)
            {
                AIManager.Instance.BakeNavMeshAfterDelay(1f);
            }
        }
    }
}
