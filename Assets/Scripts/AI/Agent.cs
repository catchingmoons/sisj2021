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

        foreach (var actionComponent in FindObjectsOfType<ActionComponent>())
        {
            if (actionComponent.IsGloballyAvailable || actionComponent.gameObject == this.gameObject)
            {
                availableActions.Add(actionComponent);
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

            ApplyEffects(currentAction.Effects, ref attributes);

            currentActions.Pop();
        }

        if (currentActions.Count == 0)
        {
            EnqueueNextActions();
            Debug.Assert(currentActions.Count > 0, $"{name} computed no actions!");
        }

        var action = currentActions.Peek();

        if (!action.arePreconditionsMet(this, attributes)) Debug.LogWarning($"{name}'s action's preconditions are not met! Potentially unstable");

        action.Begin(this);
    }

    private void EnqueueNextActions()
    {
        Debug.Log($"{name} calculating actions...");
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
        Debug.Log($"{name} added {currentActions.Count} actions.");
    }

    private void FindAllAvailableActions(Node parent, IEnumerable<Action> actions, List<Node> solutions)
    {
        foreach (Action act in actions)
        {
            if (act.Preconditions.Intersect(parent.state).Count() == act.Preconditions.Count && act.arePreconditionsMet(this, parent.state))
            {
                var expandedState = parent.state.ToList();
                ApplyEffects(act.Effects, ref expandedState);
                var node = new Node(parent, parent.totalCost + act.Cost, expandedState, act);

                solutions.Add(node);

                FindAllAvailableActions(node, actions.Except(new Action[] { act }), solutions);
            }
        }
    }

    private void ApplyEffects(IEnumerable<Pair<string,bool>> effects, ref List<string> attributes)
    {
        foreach (var change in effects)
        {
            if (change.Value && !attributes.Contains(change.Key))
            {
                attributes.Add(change.Key);
            }
            else if (!change.Value && attributes.Contains(change.Key))
            {
                attributes.Remove(change.Key);
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
