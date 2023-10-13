using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    protected override void InitAfterAwake()
    {
    }
    public enum PlayerState
    {
        Idle,
        Stop
    }
    public PlayerState playerState;
   
}
