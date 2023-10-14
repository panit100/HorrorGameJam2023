using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputSystemManager : Singleton<InputSystemManager>
{
    const string PLAYER_ACTIONMAP = "Player";
    const string UI_ACTIONMAP = "UI";

    [SerializeField] InputActionAsset playerInputAction;

#region UnityAction
    public UnityAction<Vector2> onMove;
    public UnityAction<Vector2> onMouseLook;

#endregion

    InputActionMap playerControlMap;
    InputActionMap uiControlMap;

    bool globalInputEnable = false;
    bool playerControlEnable = true;   
    bool uiControlMapEnable = false;

    protected override void InitAfterAwake()
    {
        playerControlMap = playerInputAction.FindActionMap(PLAYER_ACTIONMAP);
        uiControlMap = playerInputAction.FindActionMap(UI_ACTIONMAP);
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

    void UpdateInputState()
    {
        if(globalInputEnable && playerControlEnable) playerControlMap.Enable();
        else playerControlMap.Disable();

        if(globalInputEnable && uiControlMapEnable) uiControlMap.Enable();
        else uiControlMap.Disable();
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

    #endregion
}
