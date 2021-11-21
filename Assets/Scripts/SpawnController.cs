using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SpawnController : MonoBehaviour
{
    //Unique counter for names. 
    private static int Index;

    [SerializeField]
    private GameObject prefab;

    [SerializeField][Tooltip("Parent used for organization during runtime")]
    private Transform parent;

    [SerializeField][Tooltip("Places where objects will be spawned. Chosen randomly")]
    private Transform[] spawnPoints;

    [SerializeField][Tooltip("Minimum Time in seconds before spawning the prefab")]
    private float minWait;

    [SerializeField][Tooltip("Maximum Time in seconds before spawning the prefab")]
    private float maxWait;

    [SerializeField][Tooltip("Maximum objects to spawn")]
    private int maxObjects = 1;

    public delegate void OnCreation(GameObject obj);
    public OnCreation onCreation;

    private int numObjects;
    private float timeToSpawn;

    public void FixedUpdate()
    {
        if (numObjects >= maxObjects) return;

        timeToSpawn -= Time.fixedDeltaTime;

        if (timeToSpawn < 0)
        {
            AddPrefab(out _);

            timeToSpawn = Random.Range(minWait, maxWait);
        }
    }

    public bool AddPrefab(out SpawnedObject result)
    {
        result = null;
        if (numObjects >= maxObjects) return false;

        numObjects++; //increment before doing anything. TODO scan periodically for sync?

        var spawnPoint = RandomSpawnPoint();
        var obj = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation, parent);
        obj.name = $"{prefab.name} {Index++}"; //makes scene view easier
        if (!obj.TryGetComponent<SpawnedObject>(out result))
        {
            result = obj.AddComponent<SpawnedObject>();
        }
        result.spawner = this;
        if (onCreation != null)
        {
            onCreation(obj);
        }

        return true;
    }

    public void Decrement() => numObjects--;

    public bool PrefabHasComponent<T>() where T : Component => prefab.GetComponent<T>() != null;

    private Transform RandomSpawnPoint() => spawnPoints[Random.Range(0, spawnPoints.Length)];

    /*public void OnGUI()
    {
        if (GUILayout.Button("Spawn"))
        {
            AddPrefab(out _);
        }
    }*/
}
