using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class PhotoObjective : Objective
{
    Camera screenShotCamera;

    void Start()
    {
        screenShotCamera = GameObject.FindGameObjectWithTag("ScreenShotCamera").GetComponent<Camera>();

        PlayerManager.Instance.PlayerEquipment.AddOnUseCameraAction(OnTakeObjectivePhoto);
    }

    public void OnTakeObjectivePhoto()
    {
        if(IsObjectiveVisible())
            CheckObjective();
    }

    private bool IsObjectiveVisible()
    {
        // Cast a ray from the camera to the enemy
        Vector3 direction = transform.position - screenShotCamera.transform.position;
        Ray ray = new Ray(screenShotCamera.transform.position, direction);

        if (Physics.Raycast(ray, out RaycastHit hit, direction.magnitude))
        {
            if (hit.collider.gameObject != gameObject && !hit.collider.isTrigger)
            {
                // An obstacle is blocking the line of sight
                return false;
            }
        }

        return true;
    }
}
