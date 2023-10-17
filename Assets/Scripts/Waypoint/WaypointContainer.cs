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
            var farEnoughWaypoints = GetFarEnoughWaypoints(originPos, minDistance);
            if (farEnoughWaypoints.Length == 0)
            {
                return GetMostFarPoint(originPos);
            }

            return farEnoughWaypoints[Random.Range(0, farEnoughWaypoints.Length)];
        }

        Waypoint GetMostFarPoint(Vector2 originPos)
        {
            return waypointList
                .OrderByDescending(waypoint => (originPos - waypoint.PlanePosition).sqrMagnitude)
                .FirstOrDefault();
        }

        public Waypoint GetRandomWaypointWithinRange(Vector2 originPos, float minDistance, float maxDistance)
        {
            var farEnoughWaypoints = GetFarEnoughWaypoints(originPos, minDistance);
            var waypointsWithinRange = farEnoughWaypoints
                .Where(waypoint => (originPos - waypoint.PlanePosition).sqrMagnitude <= (maxDistance * maxDistance))
                .ToArray();

            if (waypointsWithinRange.Length == 0)
            {
                return farEnoughWaypoints.Length == 0 ? 
                    GetMostFarPoint(originPos) :
                    farEnoughWaypoints[Random.Range(0, farEnoughWaypoints.Length)];
            }

            var player = PlayerManager.Instance.transform;
            var waypointsInFrontOfPlayer = waypointsWithinRange
                .Where(waypoint => player.InverseTransformPoint(waypoint.PlanePosition).z > 0)
                .ToArray();

            if (waypointsInFrontOfPlayer.Length == 0)
                return waypointsWithinRange[Random.Range(0, waypointsWithinRange.Length)];
            
            return waypointsInFrontOfPlayer[Random.Range(0, waypointsInFrontOfPlayer.Length)];
        }

        Waypoint[] GetFarEnoughWaypoints(Vector2 originPos, float minDistance)
        {
            return waypointList
                .Where(waypoint => (originPos - waypoint.PlanePosition).sqrMagnitude >= (minDistance * minDistance))
                .ToArray();
        }
    }
}