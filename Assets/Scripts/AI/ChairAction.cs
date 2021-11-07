using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO real logic. Code stolen from Idle
public class ChairAction : Action
{
    private float timeRemaining;

    public override bool isActive => timeRemaining > 0;

    public virtual void Awake()
    {
        Debug.LogWarning("Implementation of global object pending!");
    }

    public override bool Begin()
    {
        timeRemaining = 100;
        return true;
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;
    }
}
