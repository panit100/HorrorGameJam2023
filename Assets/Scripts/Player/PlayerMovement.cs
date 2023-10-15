using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float playerBaseSpeed;

    Vector3 direction;

    private void Start()
    {
        AddInputListener();
    }

    void AddInputListener()
    {
        InputSystemManager.Instance.onMove += OnMove;
    }

    void RemoveInputListener()
    {
        InputSystemManager.Instance.onMove -= OnMove;
    }
    
    private void FixedUpdate()
    {
        Move();
    }
    
    private void Move()
    {
        transform.Translate(direction * playerBaseSpeed * Time.deltaTime);
    }

    public void OnMove(Vector2 value)
    {
        direction = new Vector3(value.x,0,value.y);
    }

    private void OnDestroy()
    {
        RemoveInputListener();
    }
}
