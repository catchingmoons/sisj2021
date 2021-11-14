using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Possessable : MonoBehaviour
{
    [SerializeField]
    public bool Unlocked;
    private Outline Outliner;

    public string scene;

    void Start()
    {
        Outliner = GetComponent<Outline>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Unlocked)
        {
            Outliner.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Outliner.enabled = false;
        }
        
    }
}



