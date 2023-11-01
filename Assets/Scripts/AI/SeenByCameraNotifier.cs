using UnityEngine;
using Sirenix.OdinInspector;

namespace HorrorJam.AI
{
    [RequireComponent(typeof(Renderer))]
    public class SeenByCameraNotifier : MonoBehaviour
    {
        [ReadOnly][SerializeField] bool isSeenByCamera;
        [ReadOnly][SerializeField] bool isSeenByPlayer;
        public bool IsSeenByPlayer => this.isSeenByPlayer;

        void OnBecameInvisible()
        {
            isSeenByCamera = false;
        }

        void OnBecameVisible()
        {
            isSeenByCamera = true;
        }

        bool IsEnemyBehindObstacle()
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
            if(IsEnemyBehindObstacle())
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
}
