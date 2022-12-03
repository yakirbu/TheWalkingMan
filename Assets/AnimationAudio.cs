using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAudio : MonoBehaviour
{
    private AudioSource _audioSource;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void PlaySound()
    {
        _audioSource.Play();
    }

    void PlaySoundAudioSource(AudioClip audioClip)
    {
        var audioSource = new GameObject("wind_audio_source").AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.Play();
    }
    
}
