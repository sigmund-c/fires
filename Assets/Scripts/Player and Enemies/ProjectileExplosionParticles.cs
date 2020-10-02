using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosionParticles : MonoBehaviour
{
    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        if (audio != null)
        {
            audio.Play();
            Destroy(gameObject, audio.clip.length);
        }
        else
        {
            Destroy(gameObject, 2f);
        }
    }
}
