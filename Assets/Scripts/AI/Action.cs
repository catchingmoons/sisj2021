using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Action : MonoBehaviour
{ 
	[SerializeField][Tooltip("Description of things that this agent wants done before it considers the action (e.g. \"thirsty\", \"comfy\")")]
	public List<string> preconditions = new List<string>();
	[SerializeField][Tooltip("Description of things that will be set upon completion of this action (e.g. \"warm\", \"has_coffee\", \"full_bladder\"")]
	public List<string> effects = new List<string>();
	[SerializeField][Tooltip("Lowest cost method of resolving a given action is chosen")]
	public float cost = 1;

	public virtual bool arePreconditionsMet(IEnumerable<string> state)
    {
		foreach (var condition in preconditions)
        {
			if (!state.Contains(condition)) return false;
        }
		return true;
    }

	public abstract bool isActive { get; }
	public abstract bool Begin();
	public virtual void Reset() { }

	//This is used to exclude a script from being grabbed out of the pool of all available actions
	public virtual bool IsGloballyAvailable => true;
}
