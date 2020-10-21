using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableDoor : Triggerable
{
    private Collider2D collider;
    private SpriteRenderer sr;
    private AudioSource audioSource;

    void Start()
    {
        collider = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    public override void Trigger()
    {
        audioSource.Play();
        StartCoroutine(DelayedTrigger());
    }

    IEnumerator DelayedTrigger()
    {
        yield return new WaitForSeconds(0.5f);
        collider.enabled = false;
        sr.color = new Color(0.4f, 0.4f, 0.4f, 0.5f);
    }
}
