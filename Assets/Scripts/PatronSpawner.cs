using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatronSpawner : MonoBehaviour
{
    [SerializeField]
    private SpawnController spawner;
    [SerializeField]
    private Transform destination;

    void Awake()
    {
        if (spawner == null)
        {
            Debug.LogWarning($"Spawner null. Disabling PatronSpawner {name}");
            enabled = false;
            return;
        }

        spawner.onCreation += StartWithActions;
    }

    void Start()
    {
        if (!spawner.PrefabHasComponent<MoveActionComponent>())
        {
            Debug.LogWarning($"Spawner associated prefab cannot move. Disabling PatronSpawner {name}");
            enabled = false;
            return;
        }
    }

    void OnDisable()
    {
        if (spawner != null && spawner.onCreation != null)
        {
            spawner.onCreation -= StartWithActions;
        }
    }

    public void StartWithActions(GameObject obj)
    {
        var movement = obj.GetComponent<MoveActionComponent>();
        movement.SetTarget(destination);

        var agent = obj.GetComponent<Agent>();
        agent.PushAction(movement);
    }
}
