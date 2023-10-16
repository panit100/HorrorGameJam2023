using System;
using DG.Tweening;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace HorrorJam.AI
{
    public class Enemy : MonoBehaviour
    {
        [ReadOnly][ShowInInspector] float distanceToPlayer;
        [ReadOnly][ShowInInspector] public bool IsDetectedPlayer { get; private set; }
        [ReadOnly][ShowInInspector] public bool IsStunned { get; private set; }
        
        [Header("Detection")] 
        [SerializeField] SeenByCameraNotifier seenByCameraNotifier;
        [SerializeField] float closeDetectionRange = 2f;
        [SerializeField] float eyeDetectionRange = 5f;
        [SerializeField] float loseDetectionRange = 8f;
        [SerializeField] float delayDurationAfterPursue = 1f;
        
        [Header("Sight")]
        [SerializeField] float raycastOriginHeightOffset = 0.5f;
        [SerializeField] LayerMask raycastLayerMask;
        [SerializeField] Vector3 raycastSize = new Vector3(1, 1, 1);
        
        [Header("Destination")]
        [SerializeField] WaypointContainer currentWaypointContainer;
        [SerializeField] Waypoint currentWaypoint;
        [SerializeField] NavMeshAgent agent;

        [Header("Movement")]
        [SerializeField] float reachSqrThreshold = 0.04f;
        [ReadOnly][ShowInInspector] float currentDelayPassed;

        Vector3 lastKnownPlayerPosition;
        [ReadOnly][ShowInInspector] bool isPursuingLostPlayer;

        public event Action OnDetectPlayer;
        public event Action OnLosePlayer;
        public event Action<Waypoint> OnFinishWaypoint;

        //TODO: Link with GameManager or something
        bool isPaused;

        string stunId => gameObject.name + "_stun";

        void Start()
        {
            lastKnownPlayerPosition = transform.position;
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
            Debug.DrawLine(transform.position, agent.destination, Color.white);
            if (isPaused)
                return;

            if (IsStunned)
                return;
            
            ProcessDetection();
            ProcessMovement();
        }

        void SetDestination(Vector3 pos)
        {
            agent.destination = pos;
        }

        void ProcessMovement()
        {
            if (TryMoveToPlayer()) 
                return;

            MoveToCurrentWaypoint();
        }

        bool TryMoveToPlayer()
        {
            if (IsDetectedPlayer)
            {
                SetDestination(lastKnownPlayerPosition);
                return true;
            }

            if (isPursuingLostPlayer)
            {
                if (!IsReached(lastKnownPlayerPosition))
                {
                    SetDestination(lastKnownPlayerPosition);
                }
                else
                {
                    isPursuingLostPlayer = false;
                    SetStun(delayDurationAfterPursue);
                }

                return true;
            }

            return false;
        }

        void SetStun(float duration)
        {
            DOTween.Kill(stunId);
            IsStunned = true;
            DOTween.Sequence()
                .AppendInterval(duration)
                .AppendCallback(() => IsStunned = false)
                .SetId(stunId);
        }

        void MoveToCurrentWaypoint()
        {
            if (currentWaypoint == null)
            {
                MoveToRandomWaypoint();
                return;
            }

            SetDestination(currentWaypoint.transform.position);
            if (!IsReachedCurrentWaypoint())
                return;

            currentDelayPassed += Time.deltaTime;
            if (currentDelayPassed >= currentWaypoint.DelayDuration)
            {
                OnFinishWaypoint?.Invoke(currentWaypoint);
                currentWaypoint = null;
            }
        }

        bool IsReachedCurrentWaypoint()
        {
            return IsReached(currentWaypoint.transform.position);
        }

        bool IsReached(Vector3 targetPos)
        {
            Vector3 enemyPos = transform.position;
            enemyPos.y = 0;
            Vector3 _targetpos = targetPos;
            _targetpos.y = 0;
            return (enemyPos - _targetpos).sqrMagnitude <= reachSqrThreshold;
        }

        public void SetWaypoint(Waypoint waypoint)
        {
            currentWaypoint = waypoint;
            currentDelayPassed = 0;
        }
        
        void ProcessDetection()
        {
            var playerPos = AIManager.Instance.PlayerPosition;
            var myPos = transform.position;
            distanceToPlayer = Vector3.Distance(playerPos, myPos);
            
            if (IsDetectedPlayer)
            {
                lastKnownPlayerPosition = AIManager.Instance.PlayerPosition;
                if (distanceToPlayer >= loseDetectionRange)
                    NotifyLoseDetection();
            }
            else
                TryDetectPlayer();
        }

        void TryDetectPlayer()
        {
            var isSeenByCamera = seenByCameraNotifier.IsSeenByCamera;
            if (distanceToPlayer <= closeDetectionRange)
            {
                if (isSeenByCamera)
                {
                    NotifyDetection();
                    return;
                }
                
                Vector3 origin = transform.position;
                origin.y += raycastOriginHeightOffset;
                
                if (IsSeeSomethingInDirectionOfPlayer(origin, out var hit))
                {
                    var isPlayer = hit.collider.GetComponent<PlayerManager>();
                    var hitPosition = hit.collider.transform.position;
                    hitPosition.y = origin.y;
                    Debug.DrawLine(origin, hitPosition, isPlayer ? Color.red: Color.blue);
                    if (isPlayer)
                        NotifyDetection();
                }
            }
            else if (distanceToPlayer <= eyeDetectionRange && isSeenByCamera)
            {
                NotifyDetection();
            }
        }

        bool IsSeeSomethingInDirectionOfPlayer(Vector3 origin, out RaycastHit hit)
        {
            Quaternion rotation = Quaternion.identity;

            var playerPos = AIManager.Instance.PlayerPosition;
            playerPos.y = origin.y;
            var direction = (playerPos - origin).normalized;
            return Physics.BoxCast(origin, raycastSize * 0.5f, direction, out hit, rotation, closeDetectionRange, raycastLayerMask);
        }

        void NotifyDetection()
        {
            OnDetectPlayer?.Invoke();
            IsDetectedPlayer = true;
        }

        void NotifyLoseDetection()
        {
            OnLosePlayer?.Invoke();
            IsDetectedPlayer = false;
            isPursuingLostPlayer = true;
        }

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            var myPosition = transform.position;
            var color = Handles.color;
            Handles.color = Color.red;
            Handles.DrawWireDisc(myPosition,Vector3.up, closeDetectionRange);
            
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(myPosition,Vector3.up,  eyeDetectionRange);
            
            Handles.color = Color.green;
            Handles.DrawWireDisc(myPosition,Vector3.up,  loseDetectionRange);
            Handles.color = color;
        }
#endif
    }
}