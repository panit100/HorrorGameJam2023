using UnityEngine;

namespace HorrorJam.AI
{
    public class AIManager : Singleton<AIManager>
    {
        public Vector3 PlayerPosition { get; private set; }
        [SerializeField] Transform playerTransform;
        protected override void InitAfterAwake()
        {
            
        }

        void Update()
        {
            PlayerPosition = playerTransform.position;
        }
    }
}