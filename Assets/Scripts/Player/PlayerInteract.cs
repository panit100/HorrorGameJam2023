using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] float InteractRange = 10f;
    Camera mainCamera;
    RaycastHit hit;
    void Start()
    {
        mainCamera = Camera.main;
        AddInputListener();
    }

    void AddInputListener()
    {
        InputSystemManager.Instance.onInteract += OnInteract;
    }   

    void RemoveInputListener()
    {
        InputSystemManager.Instance.onInteract -= OnInteract;
    }

    void OnInteract()
    {
        if(IsInteractObjectInRange())
            hit.transform.GetComponent<InteractObject>().OnInteract();
    }
    
    bool IsInteractObjectInRange()
    {
        Vector3 camPos = Camera.main.transform.position;
        Vector3 camForward = Camera.main.transform.TransformDirection(Vector3.forward);
        Ray ray = new Ray(camPos,camForward);

        if(Physics.Raycast(ray,out hit,InteractRange,LayerMask.GetMask("InteractObject")))
        {
            return true;
        }

        return false;
    }

    void OnDestroy() 
    {
        RemoveInputListener();
    }

    void OnDrawGizmos() 
    {
        Gizmos.color = IsInteractObjectInRange() ? Color.green : Color.red;    

        Vector3 camPos = Camera.main.transform.position;
        Vector3 camForward = Camera.main.transform.TransformDirection(Vector3.forward) * InteractRange;
        Gizmos.DrawRay(camPos,camForward);
    }
}
