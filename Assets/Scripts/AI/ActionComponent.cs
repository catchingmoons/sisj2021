using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class ActionComponent : MonoBehaviour, Action
{ 
	[SerializeField][Tooltip("Description of things that this agent wants done before it considers the action (e.g. \"thirsty\", \"comfy\")")]
	public List<string> _preconditions = new List<string>();
	[SerializeField][Tooltip("Description of changes upon completion of this action (e.g. \"warm\":true, \"has_coffee\":false, \"full_bladder\":true")]
	public List<Pair<string, bool>> _effects = new List<Pair<string, bool>>();
	[SerializeField][Tooltip("Lowest cost method of resolving a given action is chosen")]
	public float _cost = 1;

	public List<string> Preconditions => _preconditions;
	public List<Pair<string, bool>> Effects => _effects;
	public float Cost => _cost;

	//Can't change any state! Agent's attributes should not be used, only the supplied collection
	public virtual bool arePreconditionsMet(Agent agent, IEnumerable<string> state)
    {
		foreach (var condition in _preconditions)
        {
			if (!state.Contains(condition)) return false;
		}
		return true;
    }

	public virtual void Reset() { }

	public abstract bool Begin(Agent agent);
	public abstract bool isActive { get; }

	//This is used to exclude a script from being grabbed out of the pool of all available actions
	public virtual bool IsGloballyAvailable => true;
}
