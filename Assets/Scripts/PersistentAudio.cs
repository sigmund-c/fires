using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PersistentAudio : MonoBehaviour
{
    public List<AudioClip> audioStorage = new List<AudioClip>();
    public int isPlaying;

    private AudioSource _audioSource;
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        _audioSource = GetComponent<AudioSource>();

        OnLevelWasLoaded();
    }

    void OnLevelWasLoaded()
    {

        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "MenuScene" || sceneName == "EndingVideo")
        {
            _audioSource.Stop();
            return;
        }

        int toPlay = 0;
        if (sceneName.StartsWith("Level 2"))
        {
            toPlay = 2;
        }

        if (_audioSource.clip == null || isPlaying != toPlay)
        {
            _audioSource.clip = audioStorage[toPlay];
            isPlaying = toPlay;
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
        isPlaying = index;
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }
}