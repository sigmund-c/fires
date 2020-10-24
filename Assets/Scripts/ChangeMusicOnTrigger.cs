using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusicOnTrigger : MonoBehaviour
{
    public int toPlay = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PersistentAudio audio = GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentAudio>();
        if (audio.isPlaying != toPlay)
        {
            audio.ChangeMusic(toPlay);
        }
    }
}
