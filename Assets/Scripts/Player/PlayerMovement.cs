using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float playerBaseSpeed;
    Rigidbody rb;
    Vector3 direction;
    [SerializeField] Transform orientation;
    [SerializeField] Transform playerCamera;
    bool isMove = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        InputSystemManager.Instance.onMove += OnMove;
        InputSystemManager.Instance.onPressMove += OnPressMove;
    }

    public void OnMove(Vector2 value)
    {
        direction = (orientation.forward * value.y + orientation.right * value.x).normalized;
    }
    public void OnPressMove(bool value)
    {
        isMove = value;
    }
    private void Move()
    {
        if (!isMove)
        {
            direction = Vector3.zero;
        }
        if (PlayerManager.Instance.playerState != PlayerManager.PlayerState.Idle)
        {
            return;
        }
        rb.velocity = direction * playerBaseSpeed;
    }
    void Rotation()
    {
        transform.rotation = playerCamera.transform.rotation;
    }
    private void FixedUpdate()
    {
        Move();
        Rotation();
    }
    private void OnDestroy()
    {
        InputSystemManager.Instance.onMove -= OnMove;
        InputSystemManager.Instance.onPressMove -= OnPressMove;
    }
}
