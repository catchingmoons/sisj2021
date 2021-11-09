using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationActionComponent : ActionComponent
{
    [SerializeField] [Tooltip("Name called on Agent's Animation Object")]
    private string animationName;

    private Animator anim;

    public override bool Begin(Agent agent)
    {
        if (!base.Begin(agent))
        {
            Debug.LogWarning($"{name} was unable to begin due to parent");
            return false;
        }

        anim = agent.GetComponent<Animator>();

        anim.Play(animationName);

        return true;
    }

    public override bool arePreconditionsMet(Agent agent, IEnumerable<string> state)
    {
        Debug.Assert(agent != null);

        if (!base.arePreconditionsMet(agent, state)) return false;
        if (!agent.TryGetComponent<Animator>(out _)) return false;
        //TODO check for clip name?

        return true;
    }

    public override bool Busy
    {
        get
        {
            if (anim == null) return false;
            var stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            if (!stateInfo.IsName(animationName)) return false;
            if (!anim.IsInTransition(0) && stateInfo.normalizedTime > stateInfo.length) return false;

            return true;
        }
    }

    public override void Reset() => anim = null;
    public override float Cost(Agent agent) => base.Cost(agent) + 1;
    public override bool GloballyAvailable => true;
}