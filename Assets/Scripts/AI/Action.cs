using System.Collections.Generic;

[System.Serializable]
public struct Pair<TKey, TValue>
{
	public TKey Key;
	public TValue Value;
}

public interface Action
{
	public List<string> Preconditions { get; }
	public List<Pair<string, bool>> Effects { get; }
	public bool GloballyAvailable { get; }
	public bool Busy { get; }
	public bool arePreconditionsMet(IEnumerable<string> state);
	public float Cost(Agent actor);
	public bool Begin(Agent actor);
	public void Reset();
}
