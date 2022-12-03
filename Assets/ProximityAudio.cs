using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProximityAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Transform target;

    [SerializeField] private float distanceThreshold;
    
    private void Update()
    {
        AdjustVolume();
    }

    void AdjustVolume()
    {
        if (audioSource.isPlaying)
        {
            // do this only if some audio is being played in this gameObject's AudioSource

            float distanceToTarget =
                Vector3.Distance(transform.position,
                    target.position); // Assuming that the target is the player or the audio listener

            if (distanceToTarget < distanceThreshold)
            {
                distanceToTarget = 1;
            }
            
            if (distanceToTarget > 110)
            {
                audioSource.volume = 0;
            }
            else
            {
                audioSource.volume =
                    1 / (distanceToTarget / 10);
            }
        }
        
    }
}
