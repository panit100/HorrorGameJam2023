using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Eenemy_FSM
{
    [RequireComponent(typeof(Animator),typeof(NavMeshAgent))]
    public abstract class Enemy : MonoBehaviour
    {
        protected StateMachine<EnemyState,StateEvent> enemyFSM;
        protected Animator animator;
        protected NavMeshAgent agent;

        protected Transform player;
        public Transform Player => player;

        protected Vector3 targetPosition;
        public Vector3 TargetPosition => targetPosition;

        [SerializeField,ReadOnly] EnemyState beforeState = EnemyState.Idle;
        public EnemyState BeforeState => beforeState;
        [SerializeField,ReadOnly] EnemyState currentState = EnemyState.Idle;
        public EnemyState CurrentState => currentState;

        protected virtual void Awake() 
        {
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            enemyFSM = new StateMachine<EnemyState, StateEvent>();

            AddState();
            AddTriggerTransition();
            AddTransition();

            enemyFSM.Init();
        }
        
        protected virtual void Start()
        {
            player = PlayerManager.Instance.transform;
        }

        protected abstract void AddState();

        protected abstract void AddTriggerTransition();
        
        protected abstract void AddTransition();

        protected virtual void Update()
        {
            if (GameManager.Instance.IsPause)
                return;
                
            enemyFSM.OnLogic();    
        }

        public void SetCurrentState()
        {   
            currentState = enemyFSM.ActiveStateName;
        }
        
        public void SetBeforeState()
        {   
            if(enemyFSM.ActiveStateName != EnemyState.Idle)  
                beforeState = enemyFSM.ActiveStateName;
        }
    }
}
