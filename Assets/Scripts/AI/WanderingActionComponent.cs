using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Based on https://gist.github.com/mminer/1331271
//Reuses "Idle" to just wait awhile before stopping. important to call the base parts too!
[RequireComponent(typeof(MovementController))]
public class WanderingActionComponent : IdleActionComponent
{
    [SerializeField][Tooltip("Wait in seconds before picking a new direction")]
    private float rotateInterval = 1;
    [SerializeField][Tooltip("Every new direction, turn +/- this many degrees")]
    private float maxHeadingChange = 15;

    private MovementController moveController;

    private float? heading;

    public override void Awake()  //Idle action warns about stuff we don't care about, so we specifically avoid base
    {
        moveController = GetComponent<MovementController>();
    }

    void OnDisable()
    {
        heading = null;
    }

    public override bool Begin()
    {
        heading = transform.rotation.eulerAngles.y;
        StartCoroutine(ChangeHeading());

        if (!base.Begin())
        {
            heading = null;
            return false;
        }
        return true;
    }

    public override void Reset()
    {
        heading = null;
        base.Reset();
    }

    IEnumerator ChangeHeading()
    {
        while (heading.HasValue)
        {
            heading = (heading + Random.Range(-maxHeadingChange, maxHeadingChange)) % 360;
            moveController.Direction = new Vector3(0, heading.Value, 0);
            yield return new WaitForSeconds(rotateInterval);
        }
    }
}
