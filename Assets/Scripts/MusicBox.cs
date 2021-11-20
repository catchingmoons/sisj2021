using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class MusicBox : MonoBehaviour
{
    Outline outline;
    MusicController music;

    public bool isInRange => outline.enabled;
    
    void Awake()
    {
        outline = GetComponent<Outline>();
        music = FindObjectOfType<MusicController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && isInRange)
        {
            music.NextTrack();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (Possession.currentFocus == null && other.CompareTag("Player"))
        {
            outline.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (isInRange && other.CompareTag("Player"))
        {
            outline.enabled = false;
        }
    }
}
