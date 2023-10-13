using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HorrorJam.AI
{
    public class WaypointContainer : MonoBehaviour
    {
        List<Waypoint> waypointList = new List<Waypoint>();

        void Awake()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent<Waypoint>(out var wp))
                    waypointList.Add(wp);
            }
        }

        public Waypoint GetRandomWaypoint()
        {
            return waypointList[Random.Range(0, waypointList.Count)];
        }
    }
}