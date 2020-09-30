using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioStorage : MonoBehaviour
{
    public List<AudioClip> audios;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void PlayClip(int index)
    {
        audioSource.PlayOneShot(audios[index]);
    }
}
