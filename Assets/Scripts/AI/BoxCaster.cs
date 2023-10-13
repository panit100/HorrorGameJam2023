using UnityEngine;

namespace HorrorJam.AI
{
    public class BoxCaster : MonoBehaviour
    {
        public LayerMask layerMask; // Specify the layer(s) you want to check.
        public Vector3 size = new Vector3(1f, 1f, 1f);
        public Color gizmoColor = Color.red;

        void Update()
        {
            Vector3 origin = transform.position;
            Quaternion rotation = Quaternion.identity; 
            float maxDistance = 2f;
            
            RaycastHit hit;
            if (Physics.BoxCast(origin, size * 0.5f, transform.forward, out hit, rotation, maxDistance, layerMask))
            {
                // Handle the hit object.
                Debug.Log("Hit object: " + hit.collider.gameObject.name);
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.color = gizmoColor;
            Vector3 origin = transform.position;
            Quaternion rotation = Quaternion.identity;
            float maxDistance = 2f;

            // Draw the boxcast gizmo.
            Gizmos.DrawWireCube(origin + transform.forward * (maxDistance * 0.5f), size);
        }
    }
}