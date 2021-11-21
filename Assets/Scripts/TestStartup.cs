using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStartup : MonoBehaviour
{
    void Awake()
    {
        if (FindObjectOfType<MasterController>() == null)
        {
            Debug.LogWarning("Auto creating master controller. May not have listeners/music. Try loading the \"Master Scene\" if you want to test other scenes.");
            var obj = new GameObject("AUTO-master");
            obj.transform.parent = gameObject.transform;
            obj.AddComponent<MasterController>();
        }        
    }
}
