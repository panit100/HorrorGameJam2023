using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using Sirenix.OdinInspector;

public class PhotoObjective : Objective
{
    Camera screenShotCamera;
    [ReadOnly][SerializeField] bool isSeenByCamera;
    [ReadOnly][SerializeField] bool isSeenByPlayer;
    public bool IsSeenByPlayer => this.isSeenByPlayer;

    void Start()
    {
        screenShotCamera = GameObject.FindGameObjectWithTag("ScreenShotCamera").GetComponent<Camera>();

        PlayerManager.Instance.PlayerEquipment.AddOnUseCameraAction(OnTakeObjectivePhoto);
    }

    public void OnTakeObjectivePhoto()
    {
        if(!IsSeenByPlayer)
            return;

        if(CheckObjective())
        {
            MainObjectiveManager.Instance.UpdateProgress(objectiveCode);
            gameObject.SetActive(false);
        }
    }


    void OnBecameInvisible()
    {
        isSeenByCamera = false;
    }

    void OnBecameVisible()
    {
        isSeenByCamera = true;
    }

    bool IsObjectiveBehindObstacle()
    {
        // Use raycasting to check for obstacles between the enemy and the player
        RaycastHit hit;
        if (Physics.Linecast(transform.position, PlayerManager.Instance.transform.position, out hit))
        {
            if (hit.collider.gameObject.CompareTag("Obstacle"))
            {
                return true;
            }
        }
        return false;
    }

    private void Update()
    {
        // Check for visibility on every frame
        if(IsObjectiveBehindObstacle())
        {
            isSeenByPlayer = false;
            return;
        }

        if(!isSeenByCamera)
        {
            isSeenByPlayer = false;
            return;
        }
        
        isSeenByPlayer = true;
    }
}
