using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputSystemManager : PersistentSingleton<InputSystemManager>, IAstronosisDebug
{
    const string PLAYER_ACTIONMAP = "Player";
    const string UI_ACTIONMAP = "UI";
    const string INGAME_ACTIONMAP = "InGame";
    const string INGAMEUI_ACTIONMAP = "InGameUI";
    const string CUTSCENE_ACTIONMAP = "Cutscene";

    [SerializeField] InputActionAsset playerInputAction;
    public IAstronosisDebug.debugMode debugMode;
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
    InputActionMap inGameUIControlMap;
    InputActionMap cutsceneControlMap;


    bool globalInputEnable = false;
    bool playerControlEnable = false;
    bool uiControlMapEnable = true;
    bool inGameMapEnable = false;
    bool inGameUIMapEnable = false;
    bool cutsceneMapEnable = false;

    protected override void InitAfterAwake()
    {
        playerControlMap = playerInputAction.FindActionMap(PLAYER_ACTIONMAP, true);
        uiControlMap = playerInputAction.FindActionMap(UI_ACTIONMAP, true);
        inGameControlMap = playerInputAction.FindActionMap(INGAME_ACTIONMAP, true);
        inGameUIControlMap = playerInputAction.FindActionMap(INGAMEUI_ACTIONMAP, true);
        cutsceneControlMap = playerInputAction.FindActionMap(CUTSCENE_ACTIONMAP, true);
    }

    void Start()
    {
        ToggleGlobalInput(true);
        DebugToggle(debugMode);
    }

    public void DebugToggle(IAstronosisDebug.debugMode mode)
    {
        switch (mode)
        {
            case IAstronosisDebug.debugMode.None:
                break;
            case IAstronosisDebug.debugMode.IgnoreDependencie:
                break;
            case IAstronosisDebug.debugMode.Playing:
                TogglePlayerControl(true);
                break;
            default:
                break;
        }
    }

    #region ToggleInput

    [Button]
    public void ToggleGlobalInput(bool toggle)
    {
        globalInputEnable = toggle;
        UpdateInputState();
    }

    [Button]
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

    public void ToggleInGameUIControl(bool toggle)
    {
        inGameUIMapEnable = toggle;
        UpdateInputState();
    }

    public void ToggleCutsceneControl(bool toggle)
    {
        cutsceneMapEnable = toggle;
        UpdateInputState();
    }

    void UpdateInputState()
    {
        if (globalInputEnable && playerControlEnable) playerControlMap.Enable();
        else playerControlMap.Disable();

        if (globalInputEnable && uiControlMapEnable) uiControlMap.Enable();
        else uiControlMap.Disable();

        if (globalInputEnable && inGameMapEnable) inGameControlMap.Enable();
        else inGameControlMap.Disable();

        if (globalInputEnable && inGameUIMapEnable) inGameUIControlMap.Enable();
        else inGameUIControlMap.Disable();

        if (globalInputEnable && cutsceneMapEnable) cutsceneControlMap.Enable();
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
