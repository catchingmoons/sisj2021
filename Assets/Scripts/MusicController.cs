using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoBehaviour
{
    [SerializeField]
    private string directory;

    private AudioSource source;
    private int currentClipNdx;

    private Object[] clips;

    public void Awake()
    {
        source = GetComponent<AudioSource>();

        clips = Resources.LoadAll(directory, typeof(AudioClip));

        if (clips.Length > 0)
        {
            //TODO add unlocking of clips!
            source.clip = (AudioClip)clips[Random.Range(0, clips.Length)];
            PlayMusic();
        }
    }

    public void PlayMusic()
    {
        if (!source.isPlaying)
        {
            source.Play();
        }
    }

    public void StopMusic()
    {
        if (source.isPlaying)
        {
            source.Stop();
        }
    }

    public void NextTrack()
    {
        StopMusic();
        currentClipNdx = (currentClipNdx + 1) % clips.Length;
        source.clip = (AudioClip)clips[currentClipNdx];
        PlayMusic();
    }
}
