using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DestinationActionComponent : ActionComponent
{
    [SerializeField]
    private float minDistance = 0.2f;

    MovementController mover;

    public virtual void Update()
    {
        if (mover == null) return; //Not started

        if (InRangeOfTarget)
        {
            mover.Target = null;
            return;
        }
    }

    public override bool Begin(Agent agent)
    {
        if (!base.Begin(agent))
        {
            Debug.LogWarning($"{name} was unable to begin due to parent");
            return false;
        }
        var prevMover = mover;
        if (!agent.TryGetComponent<MovementController>(out mover))
        {
            Debug.LogWarning($"{name} unable to find mover on target {agent.name}!");
            return false;
        }
        if (prevMover != null && prevMover != mover) return false; //Return false if we already had this target

        mover.Target = transform;

        return true;
    }

    public override void Reset() 
    {
        mover = null;
    }

    public override float Cost(Agent agent) => base.Cost(agent) + (transform.position - agent.transform.position).magnitude;
    public override bool Busy => mover != null && !InRangeOfTarget;
    public override bool GloballyAvailable => true;

    private Vector3 Distance => transform.position - mover.transform.position;
    private bool InRangeOfTarget => Distance.magnitude <= minDistance;
}
