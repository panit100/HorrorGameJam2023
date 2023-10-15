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
    PlayerInteract playerInteract;

    public PlayerMovement PlayerMovement {get { return playerMovement;}}
    public PlayerInteract PlayerInteract {get { return playerInteract;}}
    

    protected override void InitAfterAwake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerInteract = GetComponent<PlayerInteract>();
    }
}
