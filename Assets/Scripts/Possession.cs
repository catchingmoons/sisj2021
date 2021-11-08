using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Possession : MonoBehaviour
{
    bool possessing = false;
    bool possessable = false;
    public GameObject ghost_player;
    GameObject possessable_obj;
    [SerializeField]
    private Renderer ghost_rend;

    void Awake()
    {
        //ghost_player = GameObject.Find("Player");
    }

    public void Possess()
    {
        if (possessing == false && possessable == true)
        {
            possessable_obj.transform.SetParent(ghost_player.transform);
            ghost_rend.material.color = new Color(0.5322179f, 0.9811321f, 0.9420714f, 0.0f);
            possessing = true;
        }
        else if (possessing == true)
        {
            possessable_obj.transform.SetParent(null);
            ghost_rend.material.color = new Color(0.5322179f, 0.9811321f, 0.9420714f, 1.0f);
            possessing = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "possessable"){
            possessable_obj = other.gameObject;
            possessable = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "possessable"){
            possessable = false;
        }
    }
}