using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Based on https://gist.github.com/mminer/1331271
//Reuses "Idle" to just wait awhile before stopping. important to call the base parts too!
[RequireComponent(typeof(Rigidbody))]
public class WanderingAction : IdleAction
{
    [SerializeField]
    private float moveSpeed = 5;
    [SerializeField]
    private float rotateSpeed = 1;
    [SerializeField][Tooltip("Wait in seconds before picking a new direction")]
    private float rotateInterval = 1;
    [SerializeField][Tooltip("Every new direction, turn +/- this many degrees")]
    private float maxHeadingChange = 15;

    private float heading;
    private float turnDuration;

    private Rigidbody rb;

    private bool isReset; //used to terminate coroutine

    public override void Awake()  //Override is important, because idle action warns about stuff we don't care about, so we specifically avoid base
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        turnDuration += rotateSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.Euler(new Vector3(0, heading, 0)), turnDuration));
        rb.MovePosition(transform.position + transform.forward * moveSpeed * Time.fixedDeltaTime);
    }

    public override bool Begin()
    {
        // Set random initial rotation
        heading = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, heading, 0);

        isReset = false;
        StartCoroutine(NewHeading());

        base.Begin();

        return true;
    }

    public override void Reset()
    {
        isReset = true;
        base.Reset();
    }

    IEnumerator NewHeading()
    {
        while (!isReset)
        {
            heading = (heading + Random.Range(-maxHeadingChange, maxHeadingChange)) % 360;
            turnDuration = 0;
            yield return new WaitForSeconds(rotateInterval);
        }
    }
}
