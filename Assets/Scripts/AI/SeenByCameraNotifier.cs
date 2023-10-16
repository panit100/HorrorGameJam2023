using Sirenix.OdinInspector;
using UnityEngine;

namespace HorrorJam.AI
{
    [RequireComponent(typeof(Renderer))]
    public class SeenByCameraNotifier : MonoBehaviour
    {
        [ReadOnly][SerializeField] bool isSeenByCamera;
        public bool IsSeenByCamera => this.isSeenByCamera;

        void OnBecameInvisible()
        {
            isSeenByCamera = false;
        }

        void OnBecameVisible()
        {
            isSeenByCamera = true;
        }
    }
}