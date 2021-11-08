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
    private Quaternion? _direction;
    private Transform _target;

    public Quaternion? Direction
    {
        get => _direction;
        set
        {
            if (_target != null)
            {
                Debug.LogWarning("Direction set on movement with target");
                _target = null;
            }
            if (_direction != value)
            {
                turnDuration = 0;
            }
            _direction = value;
        }
    }

    public Transform Target
    {
        get => _target;
        set
        {
            if (Direction.HasValue)
            {
                Debug.LogWarning("Target set on movement with direction");
                _direction = null;
            }
            if (_target != value)
            {
                turnDuration = 0;
            }
            _target = value;
        }
    }

    public bool Moving => Direction.HasValue || (Target != null && transform.position != Target.position);

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!Direction.HasValue && Target == null) return;
        if (!Moving) return;
        Debug.Assert(!Direction.HasValue || Target == null); //Both shouldn't be active!

        Quaternion desiredRotation = Direction.HasValue ? Direction.Value : Quaternion.LookRotation(Target.position - transform.position);
        turnDuration += rotateSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, desiredRotation, turnDuration));

        Vector3 desiredDestination = Direction.HasValue ? (transform.position + transform.forward * 1000f) : (Target.position);
        rb.MovePosition(Vector3.MoveTowards(transform.position, desiredDestination, moveSpeed * Time.fixedDeltaTime));
    }

    void OnDisable()
    {
        Direction = null;
    }
}
