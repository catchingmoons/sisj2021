using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;

    private Vector2 direction;

    public void Update()
    {
        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    public void FixedUpdate()
    {
        player.Move(direction, Time.fixedDeltaTime);
    }
}
