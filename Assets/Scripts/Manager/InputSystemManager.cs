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
    const string PIPBOY_ACTIONMAP = "Pipboy";
    const string CUTSCENE_ACTIONMAP = "Cutscene";

    [SerializeField] InputActionAsset playerInputAction;

#region UnityAction
    public UnityAction<Vector2> onMove;
    public UnityAction<Vector2> onMouseLook;
    public UnityAction onInteract;
    public UnityAction onUseEquipment;
    public UnityAction onPause;
    public UnityAction onUseArmConsole;
    public UnityAction onSkipcutscene;
    public UnityAction<int> onUseScanner;
    public UnityAction<int> onUseCamera;

#endregion

    InputActionMap playerControlMap;
    InputActionMap uiControlMap;
    InputActionMap inGameControlMap;
    InputActionMap pipboyControlMap;
    InputActionMap cutsceneControlMap;
    

    bool globalInputEnable = false;
    bool playerControlEnable = false;   
    bool uiControlMapEnable = true;
    bool inGameMapEnable = false;
    bool pipboyMapEnable = false;
    bool cutsceneMapEnable = false;

    protected override void InitAfterAwake()
    {
        playerControlMap = playerInputAction.FindActionMap(PLAYER_ACTIONMAP);
        uiControlMap = playerInputAction.FindActionMap(UI_ACTIONMAP);
        inGameControlMap = playerInputAction.FindActionMap(INGAME_ACTIONMAP);
        pipboyControlMap = playerInputAction.FindActionMap(PIPBOY_ACTIONMAP);
        cutsceneControlMap = playerInputAction.FindActionMap(CUTSCENE_ACTIONMAP);
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

    public void TogglePipboyControl(bool toggle)
    {
        pipboyMapEnable = toggle;
        UpdateInputState();
    }

    public void ToggleCutsceneControl(bool toggle)
    {
        cutsceneMapEnable = toggle;
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

        if(globalInputEnable && pipboyMapEnable) pipboyControlMap.Enable();
        else pipboyControlMap.Disable();

        if(globalInputEnable && cutsceneMapEnable) cutsceneControlMap.Enable();
        else cutsceneControlMap.Disable();
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

    private void OnSkipCutscene(InputValue value)
    {
        onSkipcutscene?.Invoke();
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
