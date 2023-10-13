using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public Vector2 deltaMouse;

    void Start()
    {
        InputSystemManager.Instance.onMouseLook += OnMouseLook;
    }

    void OnMouseLook(Vector2 value)
    {
        deltaMouse = value;
    }
}
