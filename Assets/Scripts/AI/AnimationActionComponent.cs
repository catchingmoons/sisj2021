using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationActionComponent : ActionComponent
{
    [SerializeField] [Tooltip("Name called on Agent's Animation Object")]
    private string agentAimationName;
    [SerializeField][Tooltip("Name of own animation")]
    private string selfAnimationName;

    private Animator agentAnim;
    private Animator selfAnim;

    public void Awake()
    {
        selfAnim = GetComponent<Animator>();
    }

    public override bool Begin(Agent agent)
    {
        if (!base.Begin(agent))
        {
            Debug.LogWarning($"{name} was unable to begin due to parent");
            return false;
        }

        agentAnim = agent.GetComponent<Animator>();

        if (agentAnim == null)
        {
            Debug.LogWarning("No animator on " + agent.name);
            return true;
        }

        agentAnim.Play(agentAimationName);

        selfAnim?.Play(selfAnimationName);

        return true;
    }

    public override bool arePreconditionsMet(Agent agent, IEnumerable<string> state)
    {
        Debug.Assert(agent != null);

        if (!base.arePreconditionsMet(agent, state)) return false;
        //TODO Uncomment once animations are actually in place? if (!agent.TryGetComponent<Animator>(out _)) return false; 
        //TODO check for clip name?

        return true;
    }

    public override bool Busy => isBusy(agentAnim, selfAnimationName) || isBusy(selfAnim, selfAnimationName);

    private static bool isBusy(Animator anim, string name)
    {
        if (anim == null) return false;
        var stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName(name)) return false;
        if (!anim.IsInTransition(0) && stateInfo.normalizedTime > stateInfo.length) return false;

        return true;
    }

    public override void Reset() => agentAnim = null;
    public override float Cost(Agent agent) => base.Cost(agent) + 1;
    public override bool GloballyAvailable => true;
}
