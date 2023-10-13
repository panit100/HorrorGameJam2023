#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace HorrorJam.AI
{
    public class Waypoint : MonoBehaviour
    {
        public float DelayDuration => delayDuration;
        [SerializeField] float delayDuration;

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            var color = GUI.color;
            GUI.color = Color.red;
            var position = transform.position;
            var durationPos = position + new Vector3(0, 0.5f, 0);
            Debug.DrawLine(position, durationPos, Color.red);
            Handles.Label(durationPos + new Vector3(0, 0.03f, 0), "t: " + delayDuration);
            Gizmos.DrawSphere(position, 0.2f);
            GUI.color = color;
        }
#endif
    }
}