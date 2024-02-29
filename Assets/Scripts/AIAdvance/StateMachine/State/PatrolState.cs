using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Eenemy_FSM
{
    public class PatrolState : EnemyStateBase
    {

        public PatrolState(bool needsExitTime, Enemy enemy) : base(needsExitTime,enemy) 
        { }

        public override void OnEnter()
        {
            base.OnEnter();
            agent.isStopped = false;
            enemy.SetTargetPosition(PlayerTracker.Instance.GetRandomPositionInPatrolZone());
            agent.SetDestination(enemy.TargetPosition);
        }

        public override void OnLogic()
        {
            base.OnLogic();
            if(!RequestExit)
            {
                //set distination
                if(enemy.IsReached())
                {
                    enemy.SetTargetPosition(PlayerTracker.Instance.GetRandomPositionInPatrolZone());
                }
                agent.SetDestination(enemy.TargetPosition);
            }
            else if(agent.remainingDistance <= agent.stoppingDistance)
            {
                fsm.StateCanExit();
            }
        }
    }
}
