using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Outline))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Possessable : MonoBehaviour
{
    [SerializeField]
    public bool Unlocked;

    public string scene;

    public bool inRangeToBePossessed { get; private set; }
    public bool isTarget => Possession.currentFocus == this;

    private Outline outline;

    public void Awake()
    {
        outline = GetComponent<Outline>();
    }

    public void Update()
    {
        outline.enabled = inRangeToBePossessed && isTarget;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRangeToBePossessed = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (inRangeToBePossessed && other.CompareTag("Player"))
        {
            inRangeToBePossessed = false;
        }
    }
}
