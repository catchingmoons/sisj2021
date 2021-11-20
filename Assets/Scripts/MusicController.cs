using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField]
    private string directory;

    public AudioSource interstitialSound;
    public float overlapDuration;

    public AudioSource musicSource;
    private int currentClipNdx;

    private Object[] clips;

    private bool playing;
    private double musicEndsAt;

    public void Awake()
    {
        clips = Resources.LoadAll(directory, typeof(AudioClip));

        if (clips.Length > 0)
        {
            //TODO add unlocking of clips!
            musicSource.clip = (AudioClip)clips[Random.Range(0, clips.Length)];

            playing = true;
        }
    }

    public void Update()
    {
        if (!playing) return;

        if (!musicSource.isPlaying)
        {
            PlayMusic();
        }

        if (AudioSettings.dspTime >= musicEndsAt - overlapDuration)
        {
            NextTrack();
        }
    }

    public void PlayMusic()
    {
        if (musicSource.isPlaying) return;

        musicSource.Play();

        musicEndsAt = AudioSettings.dspTime + musicSource.clip.length;
    }

    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
        playing = false;
    }

    public void NextTrack()
    {
        interstitialSound.Play();
        
        currentClipNdx = (currentClipNdx + 1) % clips.Length;
        musicSource.clip = (AudioClip)clips[currentClipNdx];

        musicSource.PlayScheduled(AudioSettings.dspTime + interstitialSound.clip.length - overlapDuration);
        musicEndsAt = AudioSettings.dspTime + interstitialSound.clip.length + musicSource.clip.length;
        playing = true;
    }
}
