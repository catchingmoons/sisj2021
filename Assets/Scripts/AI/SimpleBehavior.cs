using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SimpleBehavior : MonoBehaviour
{
    public float minDist = 0.1f;
    public float waitDuration = 5f;
    public float speed;

    public PickupCoffee pickup;

    private List<DestNode> nodes;

    private Vector3 origin;
    private bool atDestination;
    private float waitTime;
    private Stack<DestNode> curPath;
    private Stack<DestNode> returnPath;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        nodes = FindObjectsOfType<DestNode>().ToList().FindAll(x => x.destination);
        waitTime = waitDuration;
    }

    void Start()
    {
        origin = transform.position;
        curPath = BuildPathTo(nodes[Random.Range(0, nodes.Count)]);

        returnPath = new Stack<DestNode>();
        foreach (DestNode n in curPath)
        {
            returnPath.Push(n);
        }
    }

    void FixedUpdate()
    {
       if (curPath.Count > 0)
       {
            var diff = curPath.Peek().transform.position - transform.position;

            rb.MovePosition(transform.position + diff * speed * Time.fixedDeltaTime); //they're far enough apart who cares if we go a bit past the target?

            if ((curPath.Peek().transform.position - transform.position).sqrMagnitude < minDist)
            {
                curPath.Pop();
                atDestination = curPath.Count == 0;
            }
       }

        if (atDestination)
        {
            if ((origin - transform.position).sqrMagnitude < minDist)
            {
                Destroy(gameObject);
                return;
            }

            if (pickup != null)
            {
                pickup.on = waitTime > 0;
            }

            if (waitTime > 0)
            {
                waitTime -= Time.fixedDeltaTime;
                return;
            }

            atDestination = false;
            curPath = returnPath;
        }
    }

    public Stack<DestNode> BuildPathTo(DestNode lastNode)
    {
        var path = new Stack<DestNode>();

        path.Push(lastNode);
        while (path.Peek().next != null)
        {
            path.Push(path.Peek().next);
        }

        return path;
    }

    /*public DestNode findClosest(Vector3 position, IEnumerable<DestNode> nodes)
    {
        if (nodes.Count() == 0) return null;

        return nodes.OrderBy(node => (position - node.transform.position).sqrMagnitude).First();
    }*/
}
