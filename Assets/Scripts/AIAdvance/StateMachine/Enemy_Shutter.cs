using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Eenemy_FSM.Shutter
{
    [RequireComponent(typeof(Animator),typeof(NavMeshAgent))]
    public class Enemy_Shutter : Enemy
    {
        [Header("Detect Range")]
        [SerializeField] float reachSqrThreshold = 0.04f;
        [SerializeField] float startChaseRange = 10f;
        [SerializeField] float stopChaseRange = 10f;
        [SerializeField] float attackRange = 10f;
        public float AttackRange => attackRange;    

        [Header("Speed")]
        [SerializeField] float patrolSpeed = 10;
        [SerializeField] float chaseSpeed = 10;
        public float PatrolSpeed => patrolSpeed;
        public float ChaseSpeed => chaseSpeed;

        [Header("Scan Setting")]
        [SerializeField] float waitingTimeAfterScan = 3f;
        [SerializeField] float slowSpeed = 10;
        public float SlowSpeed => slowSpeed;

        [SerializeField,ReadOnly] bool isInAttackRange;
        public bool IsInAttackRange => isInAttackRange;
        [SerializeField,ReadOnly] bool isInChaseRange;
        public bool IsInChaseRange => isInChaseRange;

        Scanable scanable;
        public Scanable Scanable => scanable;

        protected override void Awake()
        {
            base.Awake();
            
            scanable = GetComponentInChildren<Scanable>();
        }

        protected override void Start()
        {
            base.Start();
            
            scanable.onActiveScan += OnActiveScan;
            scanable.onDeactiveScanComplete += OnDeactiveScan;
            scanable.onScanComplete += OnScannedComplete; //On ScanComplete
        }


        protected override void Update()
        {
            base.Update();

            IsPlayerInChaseRange();
            IsPlayerOutOfChaseRange();
            IsPlayerInAttackRange();

            MakeVisibleEnemy(scanable.scanProgress);
        }

        protected override void AddState()
        {
            enemyFSM.AddState(EnemyState.Idle,new IdleState(true,this,1f));
            enemyFSM.AddState(EnemyState.Chase,new ChaseState(true,this));
            enemyFSM.AddState(EnemyState.Patrol,new PatrolState(true,this));
            enemyFSM.AddState(EnemyState.Attack,new AttackState(true,this,OnAttack));
            enemyFSM.AddState(EnemyState.Warp,new WarpState(true,this,1f));
        }

        protected override void AddTriggerTransition()
        {
            enemyFSM.AddTriggerTransition(StateEvent.ScannedComplete,new Transition<EnemyState>(EnemyState.Patrol,EnemyState.Idle));
            enemyFSM.AddTriggerTransition(StateEvent.ScannedComplete,new Transition<EnemyState>(EnemyState.Attack,EnemyState.Idle));
            enemyFSM.AddTriggerTransition(StateEvent.ScannedComplete,new Transition<EnemyState>(EnemyState.Chase,EnemyState.Idle));
        }

        protected override void AddTransition()
        {
            enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle,EnemyState.Warp,(transition) => scanable.AlreadyScan && Time.time - scanable.ScanCompletedTime >= waitingTimeAfterScan));
            enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle,EnemyState.Chase,(transition) => isInChaseRange && !scanable.AlreadyScan));
            enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle,EnemyState.Patrol,(transition) => !isInChaseRange && !isInAttackRange && !scanable.AlreadyScan));

            enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase,EnemyState.Idle,(transition) => !isInChaseRange && !isInAttackRange && IsReached()));
            enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase,EnemyState.Attack,(transition) => isInAttackRange));

            enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Patrol,EnemyState.Idle,(transition) => isInChaseRange));

            enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack,EnemyState.Idle,(transition) => !isInAttackRange));

            enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Warp,EnemyState.Idle,(transition) => true));
        }

        [Button]
        void Warp(Vector3 position)
        {
            transform.position = position;
        }   

        public void ResetScanState()
        {
            scanable.ResetProgress();
        }

        [Button]
        public void WarpFarFromPlayer()
        {
            Warp(PlayerTracker.Instance.GetRandomPositionFarthestFromPlayer()); 
        }
        
        void OnAttack(State<EnemyState, StateEvent> state)
        {
            //TODO Attack
            Debug.Log("Enemy Attack!!");
        }
        
        public void SetTargetPosition(Vector3 position)
        {
            targetPosition = position;
        }

        public void SetAISpeed(float speed)
        {
            agent.speed = speed;
        }

        void IsPlayerInAttackRange()
        {
            float distance = Vector3.Distance(player.position,transform.position);

            if(distance > attackRange)
            {
                isInAttackRange = false;
                return;
            }

            isInAttackRange = true;
        }

        void IsPlayerInChaseRange()
        {
            if(isInChaseRange)
            {
                return;
            }
            
            float distance = Vector3.Distance(player.position,transform.position);

            if(IsObstacleInDirectionToTarget(player.position,out RaycastHit hit,startChaseRange))
            {
                if(!hit.collider.CompareTag("Player"))
                {
                    isInChaseRange = false;
                    return;
                }
            }

            if(distance > startChaseRange)
            {
                isInChaseRange = false;
                return;
            }

            isInChaseRange = true;
        }

        void IsPlayerOutOfChaseRange()
        {
            if(!isInChaseRange)
                return;

            float distance = Vector3.Distance(player.position,transform.position);

            if(IsObstacleInDirectionToTarget(player.position,out RaycastHit hit,stopChaseRange))
            {
                if(!hit.collider.CompareTag("Player"))
                {
                    isInChaseRange = false;
                    return;
                }
            }

            if(distance > stopChaseRange)
            {
                isInChaseRange = false;
            }
        }

        public bool IsReached()
        {
            Vector3 enemyPos = transform.position;
            enemyPos.y = 0;
            Vector3 targetPos = targetPosition; 
            targetPos.y = 0;
            return (enemyPos - targetPos).sqrMagnitude <= reachSqrThreshold;
        }

        bool IsObstacleInDirectionToTarget(Vector3 target,out RaycastHit hit,float range)
        {
            Quaternion rotation = Quaternion.identity;

            target.y = transform.position.y;
            var direction = (target - transform.position).normalized;
            return Physics.BoxCast(transform.position, Vector3.one*2, direction, out hit, rotation, range,LayerMask.GetMask("Player","Wall"));
        }

        void OnActiveScan()
        {
            SetAISpeed(slowSpeed);
        }

        void OnDeactiveScan()
        {
            SetAISpeed(chaseSpeed);
        }

        void OnScannedComplete()
        {
            //warp
            enemyFSM.Trigger(StateEvent.ScannedComplete);
        }

        void MakeVisibleEnemy(float visibleValue)
        {
        //     float alpha = Mathf.Clamp(visibleValue / 100f,0f,1f); 

        //     sprite.color = new Color(sprite.color.r,sprite.color.g,sprite.color.b,alpha);
        }

        void OnDestroy() 
        {
            scanable.onActiveScan -= OnActiveScan;
            scanable.onDeactiveScan -= OnDeactiveScan;
            scanable.onScanComplete -= OnScannedComplete;

            // eventEmitter.Stop();
        }

        void OnDrawGizmos() 
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position,transform.up,attackRange);   

            Handles.color = Color.blue;
            Handles.DrawWireDisc(transform.position,transform.up,startChaseRange);    

            Handles.color = Color.green;
            Handles.DrawWireDisc(transform.position,transform.up,stopChaseRange);    

            if(enemyFSM != null)
            {
                Handles.Label(transform.position + Vector3.up*10,"Enemy State : " + enemyFSM.ActiveStateName.ToString(),new GUIStyle(){fontSize = 14});
                Handles.Label(transform.position + Vector3.up*15,"Speed : " + agent.speed,new GUIStyle(){fontSize = 14});
            }
        }
    }
}
