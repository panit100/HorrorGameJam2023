using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Eenemy_FSM.Shutter
{
    public class PatrolState : EnemyStateBase
    {
        Enemy_Shutter enemy_Shutter;

        public PatrolState(bool needsExitTime, Enemy enemy) : base(needsExitTime,enemy) 
        { 
            enemy_Shutter = enemy.GetComponent<Enemy_Shutter>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            agent.isStopped = false;

            Debug.Log("enemy.BeforeState " + enemy.BeforeState);

            if(enemy.BeforeState == EnemyState.Chase || enemy.BeforeState == EnemyState.Attack)
            {
                enemy_Shutter.SetTargetPosition(PlayerTracker.Instance.GetRandomPositionFarthestFromPlayer());
            }
            else
            {
                enemy_Shutter.SetTargetPosition(PlayerTracker.Instance.GetRandomPositionInPatrolZone());
            }
            
            enemy_Shutter.SetAISpeed(enemy_Shutter.PatrolSpeed);
            agent.SetDestination(enemy_Shutter.TargetPosition);
        }

        public override void OnLogic()
        {
            base.OnLogic();
            if(!RequestExit)
            {
                //set distination
                if(enemy_Shutter.IsReached())
                {
                    enemy_Shutter.SetTargetPosition(PlayerTracker.Instance.GetRandomPositionInPatrolZone());
                }
                agent.SetDestination(enemy_Shutter.TargetPosition);
            }
            else if(agent.remainingDistance <= agent.stoppingDistance)
            {
                fsm.StateCanExit();
            }
        }
    }
}
