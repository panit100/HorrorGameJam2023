using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public Vector2 deltaMouse;
    float xRotation;
    float yRotation;
    [SerializeField] float sensitivity = 100;
    [SerializeField] Transform orientation;
    [SerializeField] Transform cameraPosition;

    void Start()
    {
        InputSystemManager.Instance.onMouseLook += OnMouseLook;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnMouseLook(Vector2 value)
    {
        deltaMouse = value;
    }
    void CameraRotate()
    {
        yRotation += deltaMouse.x * sensitivity * Time.deltaTime;
        xRotation -= deltaMouse.y * sensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation,yRotation,0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    private void Update()
    {
        transform.position = cameraPosition.transform.position;
        CameraRotate();
    }
    void OnDestroy()
    {
        InputSystemManager.Instance.onMouseLook -= OnMouseLook;
    }
}
