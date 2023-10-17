using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace HorrorJam.AI
{
    public class AIManager : Singleton<AIManager>
    {
        public Vector3 PlayerPlanePosition => new Vector2(PlayerPosition.x, PlayerPosition.z);
        public Vector3 PlayerPosition { get; private set; }
        Transform playerTransform;
        
        protected override void InitAfterAwake()
        {
            
        }

        void Start()
        {
            playerTransform = PlayerManager.Instance.transform;
        }

        void Update()
        {
            PlayerPosition = playerTransform.position;
        }

        public bool TryGetPositionOnSurface(Vector3 originalPosition, out Vector3 result)
        {
            result = originalPosition;
            if(NavMesh.SamplePosition(originalPosition, out var myNavHit, 100 , -1))
            {
                result = myNavHit.position;
                return true;
            }

            return false;
        }
    }
}