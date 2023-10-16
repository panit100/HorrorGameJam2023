using System;
using DG.Tweening;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.AI;

namespace HorrorJam.AI
{
    public class Enemy : MonoBehaviour
    {
        [Header("Movement")] 
        [SerializeField] float reachSqrThreshold = 0.04f;
        [SerializeField] SpeedSetting exploreSpeed;
        [SerializeField] SpeedSetting chaseSpeed;
        [SerializeField] SpeedSetting slowedDownSpeed;
        
        [Header("Destination")]
        [SerializeField] WaypointContainer currentWaypointContainer;
        [SerializeField] Waypoint currentWaypoint;
        [SerializeField] NavMeshAgent agent;
        
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
        
        [Header("Info - Movement")]
        [ReadOnly][SerializeField] float currentMoveSpeed = 1f;
        [ReadOnly][SerializeField] float distanceToPlayer;
        
        [Header("Info - Status")]
        [ReadOnly][SerializeField] bool isDetectedPlayer;
        [ReadOnly][SerializeField] bool isPursuingLostPlayer;
        [ReadOnly][SerializeField] bool isDelayed;
        [ReadOnly][SerializeField] float currentDelayPassed;
        [ReadOnly][SerializeField] bool isSlowedDown;
        
        Vector3 lastKnownPlayerPosition;
        SpeedSetting currentSpeedSetting;
        
        public event Action OnDetectPlayer;
        public event Action OnLosePlayer;
        public event Action<Waypoint> OnFinishWaypoint;

        //TODO: Link with GameManager or something
        bool isPaused;

        string stunId => gameObject.name + "_stun";
        string changeSpeedId => gameObject.name + "_changeSpeed";

        void Start()
        {
            lastKnownPlayerPosition = transform.position;
            OnFinishWaypoint += NotifyOnFinishWaypoint;
            ChangeSpeedSetting(exploreSpeed);
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

            if (isDelayed)
                return;
            
            ProcessDetection();
            ProcessMovement();
        }

        void SetDestination(Vector3 pos)
        {
            agent.destination = pos;
        }

        [Button]
        void ChangeSpeedSetting(SpeedSetting setting)
        {
            if (currentSpeedSetting == setting)
                return;

            currentSpeedSetting = setting;
            DOTween.Kill(changeSpeedId);
            DOTween.To(() => currentMoveSpeed, SetSpeed, currentSpeedSetting.speed, currentSpeedSetting.transitionDuration)
                .SetEase(currentSpeedSetting.transitionEase)
                .SetId(changeSpeedId);
        }

        void SetSpeed(float val)
        {
            currentMoveSpeed = val;
            agent.speed = currentMoveSpeed;
        }

        void ProcessMovement()
        {
            if (TryMoveToPlayer()) 
                return;

            MoveToCurrentWaypoint();
        }

        bool TryMoveToPlayer()
        {
            if (isDetectedPlayer)
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
                    EndPursue();
                }

                return true;
            }

            return false;
        }

        [Button]
        void EndPursue()
        {
            isPursuingLostPlayer = false;
            SetStun(delayDurationAfterPursue);
            ChangeSpeedSetting(exploreSpeed);
        }

        [Button]
        void SetStun(float duration)
        {
            DOTween.Kill(stunId);
            isDelayed = true;
            DOTween.Sequence()
                .AppendInterval(duration)
                .AppendCallback(() => isDelayed = false)
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
            
            if (isDetectedPlayer)
            {
                lastKnownPlayerPosition = AIManager.Instance.PlayerPosition;
                if (distanceToPlayer >= loseDetectionRange)
                    LosePlayer();
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
                    DetectPlayer();
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
                        DetectPlayer();
                }
            }
            else if (distanceToPlayer <= eyeDetectionRange && isSeenByCamera)
            {
                DetectPlayer();
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

        [Button]
        void DetectPlayer()
        {
            OnDetectPlayer?.Invoke();
            ChangeSpeedSetting(chaseSpeed);
            isDetectedPlayer = true;
        }

        [Button]
        void LosePlayer()
        {
            OnLosePlayer?.Invoke();
            ExitSlowDown();
            isDetectedPlayer = false;
            isPursuingLostPlayer = true;
        }

        [Button]
        public void EnterSlowDown()
        {
            if (!isDetectedPlayer)
                return;
            
            if (isSlowedDown)
                return;

            ChangeSpeedSetting(slowedDownSpeed);
            isSlowedDown = true;
        }

        [Button]
        public void ExitSlowDown()
        {
            if (!isSlowedDown)
                return;

            ChangeSpeedSetting(isDetectedPlayer? chaseSpeed : exploreSpeed);
            isSlowedDown = false;
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

    [Serializable]
    public class SpeedSetting
    {
        public float speed = 1f;
        public Ease transitionEase;
        public float transitionDuration = 1f;
    }
}