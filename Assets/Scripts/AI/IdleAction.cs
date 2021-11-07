using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Agent))] //Since this is not globally available, it needs to share a gameObject with a Agent
public class IdleAction : ActionComponent
{
    [SerializeField][Tooltip("Minimum time in seconds to do nothing")]
    public float MinTime;
    [SerializeField][Tooltip("Maximum time in seconds to do nothing")]
    public float MaxTime;

    private float timeRemaining;

    public override bool isActive => timeRemaining > 0;

    public override bool Begin(Agent agent)
    {
        timeRemaining = Random.Range(MinTime, MaxTime);
        return true;
    }

    public override bool IsGloballyAvailable => false;

    public virtual void Awake()
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
