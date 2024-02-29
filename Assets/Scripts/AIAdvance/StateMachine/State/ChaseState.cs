using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Eenemy_FSM
{
    public class ChaseState : EnemyStateBase
    {
        public ChaseState(bool needsExitTime, Enemy enemy,float ExitTime = 0.33f) : base(needsExitTime, enemy,ExitTime) 
        { }

        public override void OnEnter()
        {
            base.OnEnter();
            agent.isStopped = false;
        }

        public override void OnLogic()
        {
            base.OnLogic();
            if(!RequestExit)
            {
                //set distination
                if(enemy.IsInChaseRange)
                {
                    enemy.SetTargetPosition(enemy.Player.position);
                }
                agent.SetDestination(enemy.TargetPosition);
            }
            else if(agent.remainingDistance <= enemy.AttackRange)
            {
                fsm.StateCanExit();
            }
        }
    }
}
