using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;


public class TestPlayerMovement : MonoBehaviour
{
    
    [SerializeField, ReadOnly] float playerCurrentSpeed;
    [SerializeField] float playerBaseSpeed = 25f;
    [SerializeField] float playerRunSpeed = 40f;

    PlayerInputActions playerInput;
    Rigidbody Rigidbody;
    Vector3 direction;

    void Start()
    {
        playerInput = new PlayerInputActions();
        playerInput.Enable();

        Rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Run();
        Move();
    }
    
    void Run()
    {
        float runValue = playerInput.Player.Run.ReadValue<float>();

        if(runValue == 0)
            playerCurrentSpeed = playerBaseSpeed;
        else
            playerCurrentSpeed = playerRunSpeed;
    }

    void Move()
    {
        direction = playerInput.Player.Move.ReadValue<Vector2>();

        Rigidbody.velocity = transform.TransformDirection(new Vector3(direction.x, 0, direction.y) * playerCurrentSpeed);
    }

}
