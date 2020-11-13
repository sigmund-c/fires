using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool isActivated = false;
    public Material activatedMat;

    protected SpriteRenderer sr;
    protected Animation anim;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animation>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && !isActivated)
        {
            isActivated = true;
            ActivateSprite();
        }
    }

    private void ActivateSprite()
    {
        GetComponent<AudioSource>().Play();
        anim.Play("CheckpointActivate");
        sr.material = activatedMat;
        anim.PlayQueued("CheckpointFloat");
    }
}
