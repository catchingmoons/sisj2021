using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatronSpawner : MonoBehaviour
{
    [SerializeField]
    private SpawnController spawner;
    [SerializeField]
    private List<ActionComponent> intialActions;

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

    void OnDisable()
    {
        if (spawner != null && spawner.onCreation != null)
        {
            spawner.onCreation -= StartWithActions;
        }
    }

    public void StartWithActions(GameObject obj)
    {
        var agent = obj.GetComponent<Agent>();
        foreach(var action in intialActions.Reverse<ActionComponent>())
        {
            agent.PushAction(action);
        }
    }
}
