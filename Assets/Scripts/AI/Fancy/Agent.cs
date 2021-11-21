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
    public List<string> goals;

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
        }

        while (currentActions.Count > 0)
        {
            var action = currentActions.Peek();
            Debug.Assert(action != null , ""+currentActions.Count);
            if (currentAction == null)
            {
                if (!action.arePreconditionsMet(this, attributes))
                {
                    Debug.LogWarning($"{name} cannot start {action}. Skipping");
                }
                else
                {
                    //Debug.Log("Starting " + action);
                    var started = action.Begin(this);
                    Debug.Assert(started);
                }
                currentAction = action;
            }

            if (action.Busy) return; //still running. EXITS EARLY

            ApplyEffects(action.Effects, ref attributes);
            //Debug.Log("Stopping " + currentAction);
            currentAction.Reset();
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
        FindAllAvailableActions(new Node(null, attributes, null), availableActions, ref solutions);

        if (solutions.Count == 0)
        {
            Debug.LogWarning("No viable actions found!");
            return;
        }
        Debug.Log("Found " + solutions.Count + " performable actions");

        var selectedNode = solutions[Random.Range(0, solutions.Count)];
        while (selectedNode != null && selectedNode.action != null)
        {
            PushAction(selectedNode.action);
            selectedNode = selectedNode.parent;
        }
    }

    private void FindAllAvailableActions(Node parent, IEnumerable<Action> actions, ref List<Node> solutions)
    {
        foreach (Action act in actions)
        {
            var preconditionsMet = act.Preconditions.Intersect(parent.state).Count() == act.Preconditions.Count;
            var actMet = act.arePreconditionsMet(this, parent.state);
            Debug.Log(act + " " + preconditionsMet + " " + actMet);
            if (preconditionsMet && actMet)
            {
                var expandedState = parent.state.ToList();
                ApplyEffects(act.Effects, ref expandedState);
                var node = new Node(parent, expandedState, act);

                if (goals.Intersect(expandedState).Count() > 0)
                {
                    solutions.Add(node);
                    continue;
                }
                
                FindAllAvailableActions(node, actions.Except(new Action[] { act }), ref solutions);
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
        public IEnumerable<string> state;
        public Action action;

        public Node(Node parent, IEnumerable<string> state, Action action)
        {
            this.parent = parent;
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
