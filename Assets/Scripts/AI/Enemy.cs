using System;
using UnityEngine;
using UnityEngine.AI;

namespace HorrorJam.AI
{
    public class Enemy : MonoBehaviour
    {
        [Header("Destination")]
        [SerializeField] Transform defaultTarget;
        [SerializeField] WaypointContainer currentWaypointContainer;
        [SerializeField] Waypoint currentWaypoint;
        [SerializeField] NavMeshAgent agent;

        [SerializeField] float reachSqrThreshold = 0.04f;
        float currentDelayPassed;

        public event Action<Waypoint> OnFinishWaypoint;

        //TODO: Link with GameManager or something
        bool isPaused;

        void Start()
        {
            OnFinishWaypoint += NotifyOnFinishWaypoint;
        }

        void NotifyOnFinishWaypoint(Waypoint obj)
        {
            MoveToRandomWaypoint();
        }

        void MoveToRandomWaypoint()
        {
            if (currentWaypointContainer == null)
                return;

            SetWaypoint(currentWaypointContainer.GetRandomWaypoint());
        }

        void Update()
        {
            if (isPaused)
                return;
            
            ProcessWaypoint();
        }

        void SetDestination(Vector3 pos)
        {
            agent.destination = pos;
        }

        void ProcessWaypoint()
        {
            if (currentWaypoint == null)
            {
                if (defaultTarget)
                    SetDestination(defaultTarget.position);
                else
                    MoveToRandomWaypoint();
                
                return;
            }
            
            SetDestination(currentWaypoint.transform.position);
            if (!IsReachedWaypoint()) 
                return;
            
            currentDelayPassed += Time.deltaTime;
            if (currentDelayPassed >= currentWaypoint.DelayDuration)
            {
                OnFinishWaypoint?.Invoke(currentWaypoint);
                currentWaypoint = null;
            }
        }

        bool IsReachedWaypoint()
        {
            return (transform.position - currentWaypoint.transform.position).sqrMagnitude <= reachSqrThreshold;
        }

        public void SetWaypoint(Waypoint waypoint)
        {
            currentWaypoint = waypoint;
            currentDelayPassed = 0;
        }
    }
}