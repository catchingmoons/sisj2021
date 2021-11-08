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

    private ISet<Action> availableActions;
    private Stack<Action> currentActions;
    private Action currentAction; //needed to know if the first action was started

    public void Awake()
    {
        availableActions = new HashSet<Action>();
        currentActions = new Stack<Action>();

        foreach (var actionComponent in FindObjectsOfType<ActionComponent>())
        {
            if (actionComponent.GloballyAvailable || actionComponent.gameObject == gameObject)
            {
                availableActions.Add(actionComponent);
            }
        }

        if (availableActions.Count == 0)
        {
            Debug.LogWarning("No available actions!");
            return;
        }
    }

    public void Update()
    {
        if (currentActions.Count == 0)
        {
            AddNewActions();
            Debug.Assert(currentActions.Count > 0, $"{name} computed no actions!");
            Debug.Assert(currentAction == null, $"{name} has active action: {currentAction}");
        }

        while (currentActions.Count > 0)
        {
            var action = currentActions.Peek();

            if (currentAction == null)
            {
                action.Begin();
                currentAction = action;
            }

            if (action.isActive) return; //still running. EXITS EARLY

            ApplyEffects(action.Effects, ref attributes);

            currentAction = null;

            currentActions.Pop();
        }
    }

    public void PushAction(Action action)
    {
        currentActions.Push(action);
    }

    private void AddNewActions()
    {
        Debug.Log($"{name} calculating actions...");
        var solutions = new List<Node>();
        FindAllAvailableActions(new Node(null, 0, attributes, null), availableActions, solutions);

        Debug.Assert(solutions.Count > 0, "No viable actions found!");

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
            PushAction(selectedNode.action);
            selectedNode = selectedNode.parent;
        }
    }

    private void FindAllAvailableActions(Node parent, IEnumerable<Action> actions, List<Node> solutions)
    {
        foreach (Action act in actions)
        {
            if (act.Preconditions.Intersect(parent.state).Count() == act.Preconditions.Count && act.arePreconditionsMet(parent.state))
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
            Handles.Label(transform.position, $"{currentAction?.GetType()}.");
        }
    }
#endif
}
