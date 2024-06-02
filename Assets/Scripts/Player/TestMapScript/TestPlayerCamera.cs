using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestPlayerCamera : MonoBehaviour
{

    [SerializeField] private float mouseSensitivity = 50;
    [SerializeField] private Transform playerBody;

    private PlayerInputActions playerInput;
    private Vector2 deltaMouse;
    private float xRotation;

    void Start()
    {
        playerInput = new PlayerInputActions();
        playerInput.Enable();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        RotateCamera();
    }

    void RotateCamera()
    {
        deltaMouse = playerInput.Player.MouseLook.ReadValue<Vector2>();

        float mouseX = deltaMouse.x * mouseSensitivity * Time.deltaTime;
        float mouseY = deltaMouse.y * mouseSensitivity  * Time.deltaTime;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

}