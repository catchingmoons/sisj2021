using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 4f;

    private Rigidbody rb;

    private Vector3 forward;
    private Vector3 right;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward.Normalize();
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
    }

    public void Move(Vector2 direction, float delta)
    {
        if (direction == Vector2.zero) return;

        Vector3 movement = direction.x * right + direction.y * forward;
        transform.forward = movement;
        rb.MovePosition(transform.position + movement * moveSpeed * delta);
    }
}
