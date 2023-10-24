using System;
using DG.Tweening;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.XR;

namespace HorrorJam.AI
{
    public class Enemy : MonoBehaviour
    {
        [TitleGroup("Movement")]
        [Header("Speed")]
        [Indent,SerializeField] float reachSqrThreshold = 0.04f;
        [Indent,SerializeField] SpeedSetting exploreSpeed;
        [Indent,SerializeField] SpeedSetting chaseSpeed;
        [Indent,SerializeField] SpeedSetting slowedDownSpeed;
        [Indent,SerializeField] SpeedSetting superSlowedDownSpeed;
        [Indent,SerializeField] SpeedSetting stopSpeed;
        
        
        [Header("Destination")]
        [Indent,SerializeField] WaypointContainer currentWaypointContainer;
        [Indent,SerializeField] Waypoint currentWaypoint;
        [Indent,SerializeField] NavMeshAgent agent;
        
        [TitleGroup("Detection")]
        [Header("Range")] 
        [Indent,SerializeField] SeenByCameraNotifier seenByCameraNotifier;
        [Indent,SerializeField] float closeDetectionRange = 2f;
        [Indent,SerializeField] float eyeDetectionRange = 5f;
        [Indent,SerializeField] float loseDetectionRange = 8f;
        
        [Header("Sight")]
        [Indent,SerializeField] float raycastOriginHeightOffset = 0.5f;
        [Indent,SerializeField] LayerMask raycastLayerMask;
        [Indent,SerializeField] Vector3 raycastSize = new Vector3(1, 1, 1);
        
        [Header("After Pursue")]
        [Indent,SerializeField] float standStillDurationAfterPursue = 1f;
        [Indent, SerializeField] bool isRespawnAfterPursue;
        [Indent,Indent,SerializeField] float respawnDelayAfterPursue = 1f;
        [Indent,Indent,SerializeField] float minRespawnDistance = 5f;
        
        [TitleGroup("Info")]
        [Header("Movement")]
        [Indent,SerializeField,ReadOnly] float currentMoveSpeed = 1f;
        [Indent,SerializeField,ReadOnly] float distanceToPlayer;
        
        [Header("Status")]
        [Indent,SerializeField,ReadOnly] bool isDetectedPlayer;
        [Indent,SerializeField,ReadOnly] bool isPursuingLostPlayer;
        [Indent,SerializeField,ReadOnly] bool isShouldStandStill;
        [Indent,SerializeField,ReadOnly] float currentDelayPassed;
        [Indent,SerializeField,ReadOnly] bool isSlowedDown;
        
        Vector3 lastKnownPlayerPosition;
        SpeedSetting currentSpeedSetting;
        
        public event Action OnDetectPlayer;
        public event Action OnLosePlayer;
        public event Action<Waypoint> OnFinishWaypoint;

        string StunId => gameObject.name + "_stun";
        string ChangeSpeedId => gameObject.name + "_changeSpeed";
        string RespawnId => gameObject.name + "_respawn";

        [Header("Scan Info")]
        [SerializeField] SpriteRenderer sprite;
        Scanable scanable;

        SpeedSetting speedBeforeStop;
        void Awake() 
        {
            scanable = GetComponentInChildren<Scanable>();
        }

        void Start()
        {
            scanable.onActiveScan += EnterSlowDown;
            scanable.onScanComplete += EnterSuperSlowDown;

            scanable.onDeactiveScan += ExitSlowDown;

            lastKnownPlayerPosition = transform.position;
            OnFinishWaypoint += NotifyOnFinishWaypoint;
            ChangeSpeedSetting(exploreSpeed);
            speedBeforeStop = exploreSpeed;
            MakeVisibleEnemy(scanable.scanProgress);
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
            if (GameManager.Instance.IsPause)
                return;

            OnBeingScanned();
            
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
            DOTween.Kill(ChangeSpeedId);
            DOTween.To(() => currentMoveSpeed, SetSpeed, currentSpeedSetting.speed, currentSpeedSetting.transitionDuration)
                .SetEase(currentSpeedSetting.transitionEase)
                .SetId(ChangeSpeedId);
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
            
            if (isShouldStandStill)
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
            SetStun(standStillDurationAfterPursue);
            ChangeSpeedSetting(exploreSpeed);
            speedBeforeStop = exploreSpeed;

            if (isRespawnAfterPursue)
            {
                scanable.OnDeactiveScanWithDuration(respawnDelayAfterPursue);
                Schedule(respawnDelayAfterPursue, RespawnFarFromPlayer, RespawnId);
            }
        }

        [Button]
        void RespawnFarFromPlayer()
        {
            var playerPlanePos = AIManager.Instance.PlayerPlanePosition;
            var targetWaypoint = currentWaypointContainer.GetFarEnoughRandomWaypointOnPlane(playerPlanePos, minRespawnDistance);
            if (targetWaypoint)
                ReplaceTo(targetWaypoint);

            scanable.ResetProgress();
        }

        void ReplaceTo(Waypoint targetWaypoint)
        {
            agent.enabled = false;
            transform.position = targetWaypoint.transform.position;
            agent.enabled = true;
        }

        [Button]
        void SetStun(float duration)
        {
            isShouldStandStill = true;
            Schedule(standStillDurationAfterPursue, () => isShouldStandStill = false, StunId);
        }

        Sequence Schedule(float delay, TweenCallback callback, string id)
        {
            DOTween.Kill(id);
            return DOTween.Sequence()
                .AppendInterval(delay)
                .AppendCallback(callback)
                .SetId(id);
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
            var isSeenByPlayer = seenByCameraNotifier.IsSeenByPlayer;
            if (distanceToPlayer <= closeDetectionRange)
            {
                if (isSeenByPlayer)
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
            else if (distanceToPlayer <= eyeDetectionRange && isSeenByPlayer)
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
            CancelRespawning();
            OnDetectPlayer?.Invoke();
            ChangeSpeedSetting(chaseSpeed);
            speedBeforeStop = chaseSpeed;
            isDetectedPlayer = true;
        }
        
        [Button]
        void CancelRespawning()
        {
            DOTween.Kill(RespawnId);
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
            speedBeforeStop = slowedDownSpeed;
            isSlowedDown = true;
        }

        [Button]
        public void ExitSlowDown()
        {
            if (!isSlowedDown)
                return;

            if(isDetectedPlayer)
            {
                ChangeSpeedSetting(chaseSpeed);
                speedBeforeStop = chaseSpeed;
            }
            else
            {
                ChangeSpeedSetting(exploreSpeed);
                speedBeforeStop = exploreSpeed;
            }
            isSlowedDown = false;
        }

        [Button]
        public void EnterSuperSlowDown()
        {
            if (isSlowedDown)
            {
                ChangeSpeedSetting(superSlowedDownSpeed);
                speedBeforeStop = superSlowedDownSpeed;
            }
        }

        [Button]
        public void EnterStop()
        {
            ChangeSpeedSetting(stopSpeed);
        }

        [Button]
        public void ExitStop()
        {
            ChangeSpeedSetting(speedBeforeStop);
        }

        public void OnBeingScanned()
        {
            MakeVisibleEnemy(scanable.scanProgress);
        }

        void MakeVisibleEnemy(float visibleValue)
        {
            float alpha = Mathf.Clamp(visibleValue / 100f,0f,1f); 

            sprite.color = new Color(sprite.color.r,sprite.color.g,sprite.color.b,alpha);
        }

        void OnDestroy() 
        {
            scanable.onActiveScan -= EnterSlowDown;
            scanable.onScanComplete -= EnterSuperSlowDown;

            scanable.onDeactiveScan -= ExitSlowDown;
        }

        void OnCollisionEnter(Collision other) 
        {
            if(other.gameObject.CompareTag("Player"))
            {
                GameManager.Instance.OnDie();
            }    
        }

        public void HideAI()
        {
            gameObject.SetActive(false);
        }

        public void ShowAI()
        {
            gameObject.SetActive(true);
        }

        public void SetCurrentWaypointContainer(WaypointContainer waypointContainer)
        {
            currentWaypointContainer = waypointContainer;
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