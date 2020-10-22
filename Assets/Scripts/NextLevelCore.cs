using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelCore : Checkpoint
{
    private void ActivateSprite()
    {
        Debug.Log("actives");
        GetComponent<AudioSource>().Play();
        anim.Play("CheckpointFlashActivate");
        sr.material = activatedMat;
        anim.PlayQueued("CheckpointFloat");
    }
}
