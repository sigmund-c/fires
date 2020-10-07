using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosionParticles : MonoBehaviour
{
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
            Destroy(gameObject, audioSource.clip.length);
        }
        else
        {
            Destroy(gameObject, 2f);
        }
    }
}
