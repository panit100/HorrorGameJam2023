using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Eenemy_FSM
{
    public class EnemyStateBase : State<EnemyState,StateEvent>
    {
        protected readonly Enemy enemy;
        protected readonly NavMeshAgent agent;
        protected readonly Animator animator;
        protected bool RequestExit;
        protected float ExitTime;

        protected readonly Action<State<EnemyState,StateEvent>> onEnter;
        protected readonly Action<State<EnemyState,StateEvent>> onLogic;
        protected readonly Action<State<EnemyState,StateEvent>> onExit;
        protected readonly Func<State<EnemyState,StateEvent>,bool> canExit;
        
        public EnemyStateBase(bool needsExitTime,
            Enemy enemy,
            float exitTime = 0.1f,
            Action<State<EnemyState,StateEvent>> onEnter = null,
            Action<State<EnemyState,StateEvent>> onLogic = null,
            Action<State<EnemyState,StateEvent>> onExit = null,
            Func<State<EnemyState,StateEvent>,bool> canExit = null)
        {
            this.enemy = enemy;
            this.onEnter = onEnter;
            this.onLogic = onLogic;
            this.onExit = onExit;
            this.canExit = canExit;
            this.ExitTime = exitTime;
            this.needsExitTime = needsExitTime;
            agent = enemy.GetComponent<NavMeshAgent>();
            animator = enemy.GetComponent<Animator>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            RequestExit = false;
            onEnter?.Invoke(this);
        }

        public override void OnLogic()
        {
            base.OnLogic();
            if(RequestExit && timer.Elapsed >= ExitTime)
            {
                fsm.StateCanExit();
            }
        }

        public override void OnExitRequest()
        {
            if(!needsExitTime || canExit != null && canExit(this))
            {
                fsm.StateCanExit();
            }
            else
            {
                RequestExit = true;
            }
        }
    }
}
