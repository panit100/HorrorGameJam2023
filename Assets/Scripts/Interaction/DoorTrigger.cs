using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    List<GameObject> entitys = new List<GameObject>();

    [SerializeField] Door door;

    Coroutine closeDoor;

    public void OnInteract()
    {
        door.TriggerDoor();
    }

    private void OnTriggerEnter(Collider other)
    {
        CancelCloseDoor();

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
            {
                DelayCloseDoor();
            }
        }
    }

    void DelayCloseDoor()
    {
        closeDoor = StartCoroutine(CloseDoor());
    }

    void CancelCloseDoor()
    {
        if (closeDoor != null)
        {
            StopCoroutine(closeDoor);
            closeDoor = null;
        }
    }

    IEnumerator CloseDoor()
    {
        print("Start Coroutine");
        yield return new WaitForSeconds(2f);

        OnInteract();
    }

}
