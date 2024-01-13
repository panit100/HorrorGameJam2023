using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using HorrorJam.AI;
using Sirenix.OdinInspector;
using UnityEngine;
using Cinemachine;

public class PlayerCamera : MonoBehaviour ,IAstronosisDebug
{
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] Transform playerBody;
    public IAstronosisDebug.debugMode debugMode;
    Vector2 deltaMouse;
    float xRotation;
    float yRotation;
    bool Isdead;
    CinemachineVirtualCamera vCam;
    public NoiseSettings screenShake;

    void Awake() 
    {
        vCam = GetComponent<CinemachineVirtualCamera>();    
    }

    void Start()
    {
        DebugToggle(debugMode);
        Isdead = false;
        AddInputListener();
    }

    public void DebugToggle(IAstronosisDebug.debugMode mode)
    {
        switch (mode)
        {
            case IAstronosisDebug.debugMode.None:
                SetUpInputSensitivity();
                break;
            case IAstronosisDebug.debugMode.IgnoreDependencie:
                break;
            default:
                break;
        }
    }

    void AddInputListener()
    {
        InputSystemManager.Instance.onMouseLook += OnMouseLook;
    }

    void SetUpInputSensitivity()
    {
        mouseSensitivity = MouseSettingController.Instance.MouseSenvalue;
    }

    void RemoveInputListener()
    {
        InputSystemManager.Instance.onMouseLook -= OnMouseLook;
    }

    void LateUpdate()
    {
        if(Isdead)return;
        CameraRotate();
    }

    void CameraRotate()
    {
        float mouseX = deltaMouse.x * mouseSensitivity * Time.deltaTime;
        float mouseY = deltaMouse.y * mouseSensitivity  * Time.deltaTime;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    public void OnDead(Transform lookpoint)
    {
        PlayerManager.Instance.KilledByEnemy = lookpoint.parent.GetComponent<Enemy>();
        Isdead = true;
        Debug.Log(Vector3.Dot(transform.forward,lookpoint.forward));
        float _duration = 1;
        if (Vector3.Dot(transform.forward, lookpoint.forward) < 0)
        {
            _duration = 1 * Mathf.Abs(Vector3.Dot(transform.forward, lookpoint.forward) *1f);
        }
        else
        {
            _duration = 1 * Mathf.Abs((Vector3.Dot(transform.forward, lookpoint.forward) /2f));
        }
        Debug.Log(_duration);
        transform.DOLookAt(lookpoint.position, _duration ).SetEase(Ease.OutQuint).OnComplete((() => {CustomPostprocessingManager.Instance.DeadSequnce();}));
            //.OnComplete((() => GameManager.Instance.OnChangeGameStage(GameStage.GameOver)));
    }

    void OnMouseLook(Vector2 value)
    {
        deltaMouse = value;
    }

    void OnDestroy()
    {
        RemoveInputListener();
    }

    [Button]
    void ToggleCameraShake(bool toggle)
    {
        if(toggle)
            vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = screenShake;
        else
            vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = null;
    }
}
