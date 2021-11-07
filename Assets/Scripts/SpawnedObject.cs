using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to tracks objects of a given type from a spawner. Added if not present
public class SpawnedObject : MonoBehaviour
{
    public SpawnController spawner;

    void OnDestroy()
    {
        if (spawner != null)
        {
            spawner.Decrement();
        }
    }
}
