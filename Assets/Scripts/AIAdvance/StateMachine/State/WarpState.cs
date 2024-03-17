using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHFSM;

namespace Eenemy_FSM.Shutter
{
    public class WarpState : EnemyStateBase
    {
        Enemy_Shutter enemy_Shutter;

        public WarpState(bool needsExitTime, Enemy enemy, float exitTime = 0.33f) : base(needsExitTime, enemy, exitTime)
        {
            enemy_Shutter = enemy.GetComponent<Enemy_Shutter>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            agent.isStopped = false;
            enemy_Shutter.ResetScanState();
            enemy_Shutter.WarpFarFromPlayer();
        }
    }
}
