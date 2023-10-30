using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public enum PlayerState
{
    Move,
    PipBoy,
}

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] PlayerState playerState;
    public PlayerState PlayerState => playerState;
    PlayerMovement playerMovement;
    PlayerInteract playerInteract;
    PlayerEquipment playerEquipment;
    PlayerCamera playerCamera;

    public PlayerMovement PlayerMovement {get { return playerMovement;}}
    public PlayerInteract PlayerInteract {get { return playerInteract;}}
    public PlayerEquipment PlayerEquipment {get { return playerEquipment;}}
    public PlayerCamera PlayerCamera {get { return playerCamera;}}
    
    protected override void InitAfterAwake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerInteract = GetComponent<PlayerInteract>();
        playerEquipment = GetComponent<PlayerEquipment>();
        playerCamera = GetComponentInChildren<PlayerCamera>();
    }

    public void OnChangePlayerState(PlayerState _playerState)
    {
        playerState = _playerState;

        switch (playerState)
        {
            case PlayerState.Move:
                InputSystemManager.Instance.TogglePlayerControl(true);
                break;
            case PlayerState.PipBoy:    
                GameManager.Instance.LockCursor(false);
                InputSystemManager.Instance.TogglePlayerControl(false);
                break;
        }
    }
}
