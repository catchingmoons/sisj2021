using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5;
    [SerializeField]
    private float rotateSpeed = 1;

    private float turnDuration;
    private Vector3? _direction;

    public Vector3? Direction
    {
        get => _direction;
        set
        {
            if (value == null || value == Vector3.zero)
            {
                _direction = null;
            }
            else if (_direction != value)
            {
                turnDuration = 0;
                _direction = value.Value.normalized * 360;
            }
        }
    }

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!Direction.HasValue) return;

        turnDuration += rotateSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Direction.Value, Vector3.up), turnDuration));
        rb.MovePosition(transform.position + transform.forward * moveSpeed * Time.fixedDeltaTime);
    }

    void OnDisable()
    {
        Direction = null;
    }
}
