using Sirenix.OdinInspector;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;

namespace HorrorJam.AI
{
    public class AIManager : Singleton<AIManager>
    {
        public Vector3 PlayerPlanePosition => new Vector2(PlayerPosition.x, PlayerPosition.z);
        public Vector3 PlayerPosition { get; private set; }
        Transform playerTransform;

        [SerializeField] NavMeshSurface surface;
        
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

        [Button]
        public void BakeNavMesh()
        {
            surface.BuildNavMesh();
        }
    }
}