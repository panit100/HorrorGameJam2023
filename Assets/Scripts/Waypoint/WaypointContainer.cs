using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
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
        
        public Waypoint GetFarEnoughRandomWaypointOnPlane(Vector2 originPos, float minDistance)
        {
            var farEnoughWaypointList = waypointList
                .Where(waypoint => (originPos - waypoint.PlanePosition).sqrMagnitude >= (minDistance * minDistance))
                .ToArray();

            if (farEnoughWaypointList.Length == 0)
            {
                return waypointList
                    .OrderByDescending(waypoint => (originPos - waypoint.PlanePosition).sqrMagnitude)
                    .FirstOrDefault();
            }

            return farEnoughWaypointList[Random.Range(0, farEnoughWaypointList.Length)];
        }
    }
}