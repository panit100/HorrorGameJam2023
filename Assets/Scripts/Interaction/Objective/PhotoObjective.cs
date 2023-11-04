using UnityEngine;
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
        if(IsObjectiveBehindObstacle())
            isSeenByPlayer = false;

        if(isSeenByCamera && isSeenByPlayer)
            return;

        if(CheckObjective())
        {
            MainObjectiveManager.Instance.UpdateProgress(objectiveCode);
            unityEvent?.Invoke();
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
}
