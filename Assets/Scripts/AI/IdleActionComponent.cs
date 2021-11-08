using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Agent))] //Since this is not globally available, it needs to share a gameObject with a Agent
public class IdleActionComponent : ActionComponent
{
    [SerializeField][Tooltip("Minimum time in seconds to do nothing")]
    public float MinTime;
    [SerializeField][Tooltip("Maximum time in seconds to do nothing")]
    public float MaxTime;

    private float timeRemaining;

    public override bool Busy => timeRemaining > 0;
    public override bool GloballyAvailable => false;

    public override bool Begin(Agent agent)
    {
        if (!base.Begin(agent))
        {
            Debug.LogWarning($"{name} was unable to begin ActionComponent");
        }
        timeRemaining = Random.Range(MinTime, MaxTime);
        return true;
    }

    public void Awake()
    {
        if (Preconditions.Count > 0)
        {
            Debug.LogWarning("Idle action has preconditions! May result in no viable actions.");
        }
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;
    }
}
