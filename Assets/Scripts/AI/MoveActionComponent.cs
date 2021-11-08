using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class MoveActionComponent : MonoBehaviour, Action
{
    public List<string> Preconditions => new List<string>();
    public List<Pair<string, bool>> Effects => new List<Pair<string, bool>>();
    public bool GloballyAvailable => false;

    public float Cost => Distance.magnitude;

    private MovementController mover;

    private Transform target;
    private float targetDistanceSq;
    private Vector3 prevPosition; //In case of overshoot

    private bool running;

    public void Awake()
    {
        mover = GetComponent<MovementController>();
    }

    public void Update()
    {
        if (!running || !arePreconditionsMet(null))
        {
            mover.Direction = null;
            running = false;
            return;
        }

        mover.Direction = Distance;
        prevPosition = transform.position;
    }

    public void SetTarget(Transform target, float minDistance = 0.5f)
    {
        if (isActive)
        {
            if (target == this.target) return;

            Debug.LogWarning($"{name} set new move target when already moving!");
        }
        this.target = target;
        targetDistanceSq = minDistance * minDistance;
        running = false;
    }

    public bool arePreconditionsMet(IEnumerable<string> state) => target != null && target.gameObject.activeInHierarchy && Distance.sqrMagnitude > targetDistanceSq;
    public bool isActive => running;

    public bool Begin()
    {
        if (!arePreconditionsMet(null))
        {
            Debug.LogWarning($"{gameObject.name} unable to start move with target {target?.name}");
            return false;
        }
        if (running)
        {
            Debug.Log($"{gameObject.name} Began move while already moving");
            return false;
        }
        running = true;
        return true;
    }

    public void Reset() { running = false; }

    private Vector3 Distance => target.position - transform.position;
}
