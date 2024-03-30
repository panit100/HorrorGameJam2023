using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Eenemy_FSM.Shutter
{
    public class IdleState : EnemyStateBase
    {
        public IdleState(bool needsExitTime, Enemy enemy,float ExitTime = 0.33f) : base(needsExitTime,enemy,ExitTime){}

        public override void OnEnter()
        {
            base.OnEnter();
            agent.isStopped = true;
        }

        public override void OnLogic()
        {
            base.OnLogic();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}
