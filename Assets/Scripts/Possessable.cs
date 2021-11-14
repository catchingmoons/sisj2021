using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possessable : MonoBehaviour
{
    [SerializeField]
    public bool Unlocked;
    private Outline Outliner;

    void Start()
    {
        Outliner = GetComponent<Outline>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && Unlocked)
        {
            Outliner.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Outliner.enabled = false;
        }
        
    }
}



