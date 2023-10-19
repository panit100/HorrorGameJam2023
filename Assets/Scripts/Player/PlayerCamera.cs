using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] private Transform playerBody;

    Vector2 deltaMouse;
    float xRotation;
    float yRotation;

    public UnityAction onDie;

    void Start()
    {
        AddInputListener();
    }

    void AddInputListener()
    {
        InputSystemManager.Instance.onMouseLook += OnMouseLook;
    }

    void RemoveInputListener()
    {
        InputSystemManager.Instance.onMouseLook -= OnMouseLook;
    }

    void Update()
    {
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

    void OnMouseLook(Vector2 value)
    {
        deltaMouse = value;
    }

    void OnDestroy()
    {
        RemoveInputListener();
    }

    public void OnDie()
    {
        transform.DORotate(new Vector3(0,0,90f),3f);
    }
}
