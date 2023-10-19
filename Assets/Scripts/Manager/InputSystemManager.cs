using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputSystemManager : Singleton<InputSystemManager>
{
    const string PLAYER_ACTIONMAP = "Player";
    const string UI_ACTIONMAP = "UI";
    const string INGAME_ACTIONMAP = "InGame";

    [SerializeField] InputActionAsset playerInputAction;

#region UnityAction
    public UnityAction<Vector2> onMove;
    public UnityAction<Vector2> onMouseLook;
    public UnityAction onInteract;
    public UnityAction onUseEquipment;
    public UnityAction onPause;
    public UnityAction onUseArmConsole;
    public UnityAction<int> onUseScanner;
    public UnityAction<int> onUseCamera;

#endregion

    InputActionMap playerControlMap;
    InputActionMap uiControlMap;
    InputActionMap inGameControlMap;

    bool globalInputEnable = false;
    bool playerControlEnable = true;   
    bool uiControlMapEnable = false;
    bool inGameMapEnable = true;

    protected override void InitAfterAwake()
    {
        playerControlMap = playerInputAction.FindActionMap(PLAYER_ACTIONMAP);
        uiControlMap = playerInputAction.FindActionMap(UI_ACTIONMAP);
        inGameControlMap = playerInputAction.FindActionMap(INGAME_ACTIONMAP);
    }

    void Start() 
    {
        ToggleGlobalInput(true);
    }

#region ToggleInput

    public void ToggleGlobalInput(bool toggle)
    {
        globalInputEnable = toggle;
        UpdateInputState();
    }

    public void TogglePlayerControl(bool toggle)
    {
        playerControlEnable = toggle;
        UpdateInputState();
    }

    public void ToggleUIControl(bool toggle)
    {
        uiControlMapEnable = toggle;
        UpdateInputState();
    }

    public void ToggleInGameControl(bool toggle)
    {
        inGameMapEnable = toggle;
        UpdateInputState();
    }

    void UpdateInputState()
    {
        if(globalInputEnable && playerControlEnable) playerControlMap.Enable();
        else playerControlMap.Disable();

        if(globalInputEnable && uiControlMapEnable) uiControlMap.Enable();
        else uiControlMap.Disable();

        if(globalInputEnable && inGameMapEnable) inGameControlMap.Enable();
        else inGameControlMap.Disable();
    }

#endregion

#region ControlFunction
    
    void OnMouseLook(InputValue value)
    {
        onMouseLook?.Invoke(value.Get<Vector2>());
    }
    private void OnMove(InputValue value)
    {
        onMove?.Invoke(value.Get<Vector2>());
    }

    private void OnInteract(InputValue value)
    {
        onInteract?.Invoke();
    }

    private void OnUseEquipment(InputValue value)
    {
        onUseEquipment?.Invoke();
    }

    private void OnPause(InputValue value)
    {
        onPause?.Invoke();
    }

    private void OnUsePipBoy(InputValue value)
    {
        //TODO: LockCursor(false), InputSystemManager.Instance.TogglePlayerControl(false); when open armconsole and LockCursor(true) , InputSystemManager.Instance.TogglePlayerControl(true); when close armconsole
        //TODO: if press Q , Check is pipboy using. if it didn't use then use it, but if it use then hide.
        onUseArmConsole?.Invoke();
    }

    private void OnUseScanner(InputValue value)
    {
        onUseScanner?.Invoke(0);
    }

    private void OnUseCamera(InputValue value)
    {
        onUseCamera?.Invoke(1);
    }

    #endregion
}
