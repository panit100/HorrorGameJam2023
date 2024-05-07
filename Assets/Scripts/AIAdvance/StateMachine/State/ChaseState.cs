using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Eenemy_FSM.Shutter
{
    public class ChaseState : EnemyStateBase
    {
        Enemy_Shutter enemy_Shutter;

        public ChaseState(bool needsExitTime, Enemy enemy,float ExitTime = 0.33f) : base(needsExitTime, enemy,ExitTime) 
        { 
            enemy_Shutter = enemy.GetComponent<Enemy_Shutter>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            agent.isStopped = false;
            enemy_Shutter.SetAISpeed(enemy_Shutter.ChaseSpeed);
        }

        public override void OnLogic()
        {
            base.OnLogic();

            if(!RequestExit)
            {
                //set distination
                if(enemy_Shutter.IsInChaseRange)
                {
                    enemy_Shutter.SetTargetPosition(enemy_Shutter.Player.position);
                }
                agent.SetDestination(enemy_Shutter.TargetPosition);
            }
            else if(agent.remainingDistance <= enemy_Shutter.AttackRange)
            {
                fsm.StateCanExit();
            }
        }
    }
}
