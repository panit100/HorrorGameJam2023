using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

namespace Eenemy_FSM.Shutter
{
    public class AttackState : EnemyStateBase
    {
        public AttackState(bool needsExitTime, Enemy enemy,Action<State<EnemyState,StateEvent>> onEnter,float ExitTime = 0.33f) : base(needsExitTime,enemy,ExitTime,onEnter) {}

        public override void OnEnter()
        {
            agent.isStopped = true;
            base.OnEnter();
        }
    }
}
