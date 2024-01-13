using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using HorrorJam.AI;
using Sirenix.OdinInspector;
using UnityEngine;
using Cinemachine;
using System;

public enum ScreenShakeLevel
{
    NONE,
    WEAK,
    MEDIUM,
    STRONG,
}

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
    
    [SerializeField] ScreenShakeSetting weakScreenShake;
    [SerializeField] ScreenShakeSetting mediumScreenShake;
    [SerializeField] ScreenShakeSetting strongScreenShake;
    


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
    void ToggleCameraShake(ScreenShakeLevel level)
    {
        switch(level)
        {
            case ScreenShakeLevel.NONE:
                vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = null;
                break;
            case ScreenShakeLevel.WEAK:
                vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = weakScreenShake.noiseSettings;
                vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = weakScreenShake.amplitude;
                vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = weakScreenShake.frequency;
                break;
            case ScreenShakeLevel.MEDIUM:
                vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = mediumScreenShake.noiseSettings;
                vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = mediumScreenShake.amplitude;
                vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = mediumScreenShake.frequency;
                break;
            case ScreenShakeLevel.STRONG:
                vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_NoiseProfile = strongScreenShake.noiseSettings;
                vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = strongScreenShake.amplitude;
                vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = strongScreenShake.frequency;
                break;
        }
    }
}

[Serializable]
public class ScreenShakeSetting
{
    public NoiseSettings noiseSettings;
    public float amplitude;
    public float frequency;
}
