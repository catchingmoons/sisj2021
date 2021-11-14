using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possession : MonoBehaviour
{
    bool possessing = false;
    Possessable possessable_obj;
    [SerializeField]
    private Renderer ghost_rend;
    [SerializeField]
    public Transform possessableRoot;

    public void TogglePossess()
    {
        if (possessing)
        {
            possessable_obj.transform.SetParent(possessableRoot);
            ghost_rend.material.color = new Color(0.5322179f, 0.9811321f, 0.9420714f, 1.0f);
            possessing = false;
        }
        else if (possessable_obj != null)
        {
            possessable_obj.transform.SetParent(transform);
            ghost_rend.material.color = new Color(0.5322179f, 0.9811321f, 0.9420714f, 0.0f);
            possessing = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Possessable>(out var possessable))
        {
            possessable_obj = possessable.Unlocked ? possessable : null;
        }
    }

    void OnTriggerExit(Collider other)
    {
        possessable_obj = null;
    }
}