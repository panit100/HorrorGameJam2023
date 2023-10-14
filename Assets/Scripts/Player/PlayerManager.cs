using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Stop
}

public class PlayerManager : Singleton<PlayerManager>
{
    public PlayerState playerState;

    PlayerMovement playerMovement;

    public PlayerMovement PlayerMovement {get { return playerMovement;}}

    protected override void InitAfterAwake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    

    
   
}
