using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Eenemy_FSM
{
    [RequireComponent(typeof(Animator),typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] float reachSqrThreshold = 0.04f;
        [SerializeField] float startChaseRange = 10f;
        [SerializeField] float stopChaseRange = 10f;
        [SerializeField] float attackRange = 10f;
        public float AttackRange => attackRange;


        [SerializeField,ReadOnly] bool isInAttackRange;
        public bool IsInAttackRange => isInAttackRange;
        [SerializeField,ReadOnly] bool isInChaseRange;
        public bool IsInChaseRange => isInChaseRange;

        
        StateMachine<EnemyState,StateEvent> enemyFSM;
        Animator animator;
        NavMeshAgent agent;

        Transform player;
        public Transform Player => player;
        Vector3 targetPosition;
        public Vector3 TargetPosition => targetPosition;

        void Awake()
        {
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            enemyFSM = new StateMachine<EnemyState, StateEvent>();

            enemyFSM.AddState(EnemyState.Idle,new IdleState(true,this,1f));
            enemyFSM.AddState(EnemyState.Chase,new ChaseState(true,this));
            enemyFSM.AddState(EnemyState.Patrol,new PatrolState(true,this));
            enemyFSM.AddState(EnemyState.Attack,new AttackState(true,this,OnAttack));
            
            // enemyFSM.AddTriggerTransition(StateEvent.DetectPlayer,new Transition<EnemyState>(EnemyState.Idle,EnemyState.Chase));
            // enemyFSM.AddTriggerTransition(StateEvent.LostPlayer,new Transition<EnemyState>(EnemyState.Chase,EnemyState.Idle));

            enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle,EnemyState.Chase,(transition) => isInChaseRange));
            enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Idle,EnemyState.Patrol,(transition) => !isInChaseRange && !isInAttackRange));

            enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase,EnemyState.Idle,(transition) => !isInChaseRange && !isInAttackRange && IsReached()));
            enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Chase,EnemyState.Attack,(transition) => isInAttackRange));

            enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Patrol,EnemyState.Idle,(transition) => isInChaseRange));

            enemyFSM.AddTransition(new Transition<EnemyState>(EnemyState.Attack,EnemyState.Idle,(transition) => !isInAttackRange));

            enemyFSM.Init();
        }

        void Start()
        {
            player = PlayerManager.Instance.transform;
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

        void Update() 
        {
            IsPlayerInChaseRange();
            IsPlayerOutOfChaseRange();
            IsPlayerInAttackRange();
            
            enemyFSM.OnLogic();    
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

        void OnDrawGizmos() 
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(transform.position,transform.up,attackRange);   

            Handles.color = Color.blue;
            Handles.DrawWireDisc(transform.position,transform.up,startChaseRange);    

            Handles.color = Color.green;
            Handles.DrawWireDisc(transform.position,transform.up,stopChaseRange);    

            if(enemyFSM != null)
                Handles.Label(transform.position + Vector3.up*10,"Enemy State : " + enemyFSM.ActiveStateName.ToString(),new GUIStyle(){fontSize = 14});
        }
    }
}
