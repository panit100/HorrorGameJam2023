using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitch : MonoBehaviour , InteractObject
{
    [SerializeField] Door door;

    public void OnInteract()
    {
        door.OpenDoor();
    }
}
