using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] private Transform playerBody;

    Vector2 deltaMouse;
    float xRotation;
    float yRotation;

    void Start()
    {
        AddInputListener();

        LockCursor(true);
    }

    void LockCursor(bool toggle)
    {
        if(toggle)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
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
        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        yRotation = Mathf.Clamp(yRotation, 0f, 360f);

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
}
