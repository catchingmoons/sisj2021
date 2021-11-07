using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Agent : MonoBehaviour
{
    //Should be safe to this whenever... 
    [SerializeField][Tooltip("Description of various things a character might be (e.g. \"wet\", \"cold\", \"has_coffee\")")]
    public List<string> attributes;

    private ISet<Action> availableActions; //All actions in game that this might be able to use. 
    private Stack<Action> currentActions;

    public void Awake()
    {
        availableActions = new HashSet<Action>();
        currentActions = new Stack<Action>();

        foreach (Action action in FindObjectsOfType<Action>())
        {
            if (action.IsGloballyAvailable || action.gameObject == this.gameObject)
            {
                availableActions.Add(action);
            }
        }

        if (availableActions.Count == 0)
        {
            Debug.LogWarning("no available actions! Disabling Agent.");
            enabled = false;
            return;
        }
    }

    public void Update()
    {
        if (currentActions.Count > 0)
        {
            var currentAction = currentActions.Peek();

            if (currentAction.isActive) return; //Action is running

            foreach (string effect in currentAction.effects)
            {
                attributes.Add(effect);
            }

            currentActions.Pop();
        }

        if (currentActions.Count == 0)
        {
            EnqueueNextActions();
            Debug.Assert(currentActions.Count > 0, "No actions were enqueued!");
        }
        var action = currentActions.Peek();

        if (!action.arePreconditionsMet(attributes)) Debug.LogWarning("Chosen action's preconditions are not met! Potentially unstable");

        action.Begin();
    }

    private void EnqueueNextActions()
    {
        Debug.Log("Calculating action");
        var solutions = new List<Node>();
        FindAllAvailableActions(new Node(null, 0, attributes, null), availableActions, solutions);

        Debug.Assert(solutions.Count > 0, "No actions found viable!");

        //reduce to minimum chains for each potential action
        Dictionary<Action, Node> mins = new Dictionary<Action, Node>();
        foreach (var node in solutions)
        {
            if (mins.TryGetValue(node.action, out var currentMin))
            {
                if (currentMin.totalCost > node.totalCost)
                {
                    mins[node.action] = node;
                }
            }
            else
            {
                mins.Add(node.action, node);
            }
        }

        var potentialActions = mins.Keys.ToList();
        var selectedNode = mins[potentialActions[Random.Range(0, potentialActions.Count)]];
        while (selectedNode != null && selectedNode.action != null)
        {
            currentActions.Push(selectedNode.action);
            selectedNode = selectedNode.parent;
        }
    }

    private void FindAllAvailableActions(Node parent, IEnumerable<Action> actions, List<Node> solutions)
    {
        foreach (Action act in actions)
        {
            var expandedState = parent.state.Union(act.effects);
            if (act.preconditions.Intersect(parent.state).Count() == act.preconditions.Count && act.arePreconditionsMet(parent.state))
            {
                var node = new Node(parent, parent.totalCost + act.cost, expandedState, act);

                solutions.Add(node);

                FindAllAvailableActions(node, actions.Except(new Action[] { act }), solutions);
            }
        }
    }

    private class Node
    {
        public Node parent;
        public float totalCost;
        public IEnumerable<string> state;
        public Action action;

        public Node(Node parent, float totalCost, IEnumerable<string> state, Action action)
        {
            this.parent = parent;
            this.totalCost = totalCost;
            this.state = state;
            this.action = action;
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (currentActions != null && currentActions.Count > 0)
        {
            Handles.Label(transform.position, $"{currentActions.Peek().GetType()}.");
        }
    }
#endif
}
