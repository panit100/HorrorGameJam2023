using HorrorJam.AI;
using UnityEngine;

namespace Minimap
{
    public class Minimap : MonoBehaviour
    {
        [SerializeField] Transform cameraTransform;

        void LateUpdate()
        {
            if (cameraTransform == null)
                return;

            var playerTransform = PlayerManager.Instance.transform;
            var targetPos = playerTransform.position;
            targetPos.y = cameraTransform.transform.position.y;
            cameraTransform.position = targetPos;
            
            var targetAngle = cameraTransform.transform.eulerAngles;
            targetAngle.y = playerTransform.transform.eulerAngles.y;
            cameraTransform.eulerAngles = targetAngle;
        }
    }
}