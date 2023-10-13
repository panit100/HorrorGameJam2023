using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputSystemManager : Singleton<InputSystemManager>
{
    const string PLAYER_ACTIONMAP = "Player";
    const string UI_ACTIONMAP = "UI";
    // const string DIALOGUE_ACTIONMAP = "Dialogue";

    [SerializeField] InputActionAsset playerInputAction;

#region UnityAction

    // public UnityAction<Vector2> onMove;
    // public UnityAction onJump;
    // public UnityAction onNextDialogue;
    // public UnityAction onSkipDialogue;

#endregion

    InputActionMap playerControlMap;
    InputActionMap uiControlMap;
    // InputActionMap dialogueControlMap;

    bool globalInputEnable = false;
    bool playerControlEnable = true;   
    bool uiControlMapEnable = false;
    // bool dialogueControlMapEnable = false;

    protected override void InitAfterAwake()
    {
        playerControlMap = playerInputAction.FindActionMap(PLAYER_ACTIONMAP);
        uiControlMap = playerInputAction.FindActionMap(UI_ACTIONMAP);
        // dialogueControlMap = playerInputAction.FindActionMap(DIALOGUE_ACTIONMAP);
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

    // public void ToggleDialogueControl(bool toggle)
    // {
    //     dialogueControlMapEnable = toggle;
    //     UpdateInputState();
    // }

    void UpdateInputState()
    {
        if(globalInputEnable && playerControlEnable) playerControlMap.Enable();
        else playerControlMap.Disable();

        if(globalInputEnable && uiControlMapEnable) uiControlMap.Enable();
        else uiControlMap.Disable();

        // if(globalInputEnable && dialogueControlMapEnable) dialogueControlMap.Enable();
        // else dialogueControlMap.Disable();
    }

#endregion

#region ControlFunction
    // void OnMove(InputValue value)
    // {
    //     onMove?.Invoke(value.Get<Vector2>());
    // }

    // void OnJump(InputValue value)
    // {
    //     onJump?.Invoke();
    // }

    // void OnNextDialogue(InputValue value)
    // {
    //     onNextDialogue?.Invoke();
    // }

    // void OnSkipDialogue(InputValue value)
    // {
    //     onSkipDialogue?.Invoke();
    // }
#endregion
}
