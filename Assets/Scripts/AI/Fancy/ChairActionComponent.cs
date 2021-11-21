using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO real logic. Code stolen from Idle
public class ChairActionComponent : ActionComponent
{
    private float timeRemaining;

    public override bool Busy => timeRemaining > 0;
    public override bool GloballyAvailable => true;

    public override bool Begin(Agent actor)
    {
        timeRemaining = 100;
        return true;
    }

    public void Awake()
    {
        Debug.LogWarning("Implementation of global pending!");
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;
    }
}
