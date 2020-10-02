using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PersistentAudio : MonoBehaviour
{
    public List<AudioClip> audioStorage = new List<AudioClip>();

    private AudioSource _audioSource;
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource.clip == null)
        {
            _audioSource.clip = audioStorage[0];
        }

        PlayMusic();
    }

    public void PlayMusic()
    {
        if (_audioSource.isPlaying) return;

        if (_audioSource.clip == null)
        {
            _audioSource.clip = audioStorage[0];
        }
        _audioSource.Play();
    }

    public void ChangeMusic(int index)
    {
        _audioSource.clip = audioStorage[index];
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }
}