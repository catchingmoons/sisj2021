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

    private bool running;

    public new void Awake()  //Idle action warns about stuff we don't care about, so we specifically avoid base
    {
        moveController = GetComponent<MovementController>();
    }

    void OnDisable()
    {
        running = false;
        moveController.Direction = null;
    }

    public override bool Begin(Agent agent)
    {
        if (running)
        {
            Debug.LogWarning("Begin called while already begun");
            return false;
        }
        if (!base.Begin(agent))
        {
            Debug.LogWarning($"{name} was unable to begin parent");
            return false;
        }

        running = true;
        StartCoroutine(ChangeHeading());
        return true;
    }

    public override void Reset()
    {
        running = false;
        moveController.Direction = null;
        base.Reset();
    }

    IEnumerator ChangeHeading()
    {
        while (running)
        {
            var turnAmount = Random.Range(-maxHeadingChange, maxHeadingChange);
            moveController.Direction = Quaternion.Euler(0f, turnAmount, 0f);
            yield return new WaitForSeconds(rotateInterval);
        }
    }
}
