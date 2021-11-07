using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO real logic. Code stolen from Idle
public class ChairAction : ActionComponent
{
    private float timeRemaining;

    public override bool isActive => timeRemaining > 0;

    public override bool Begin(Agent agent)
    {
        timeRemaining = 100;
        return true;
    }

    public void Awake()
    {
        Debug.LogWarning("Implementation of global object pending!");
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;
    }
}
