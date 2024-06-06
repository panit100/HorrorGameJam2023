using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    List<GameObject> entitys = new List<GameObject>();

    [SerializeField] Door door;

    public void OnInteract()
    {
        door.TriggerDoor();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!door.IsDoorOpen)
        {
            if (entitys.Count == 0)
                OnInteract();
        }

        entitys.Add(other.gameObject);

    }

    private void OnTriggerExit(Collider other)
    {
        entitys.Remove(other.gameObject);

        if (door.IsDoorOpen)
        {
            if (entitys.Count == 0)
                OnInteract();
        }
    }

}
