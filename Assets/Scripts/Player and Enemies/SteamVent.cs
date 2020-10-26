using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamVent : MonoBehaviour
{
    public float interval = 1f;
    public float force = 50f;
    public float timeOffset;

    public ParticleSystem idleParticles;
    public ParticleSystem activeParticles;
    public Collider2D hitbox;
    public bool isActive = false;
    private Quaternion rotation;
    private AudioSource soundEffect;


    void Start()
    {
        idleParticles = transform.GetChild(0).GetComponent<ParticleSystem>();
        activeParticles = transform.GetChild(1).GetComponent<ParticleSystem>();
        hitbox = GetComponent<Collider2D>();
        rotation = transform.rotation;
        soundEffect = GetComponent<AudioSource>();
        StartCoroutine(Loop());
    }


    IEnumerator Loop()
    {
        yield return new WaitForSeconds(timeOffset);
        while (true)
        {
            idleParticles.Stop();

            yield return new WaitForSeconds(0.1f);

            isActive = true;
            hitbox.enabled = true;
            activeParticles.Play();
            soundEffect.Play();
            // play sound
            yield return new WaitForSeconds(1f);

            isActive = false;
            hitbox.enabled = false;
            idleParticles.Play();
            yield return new WaitForSeconds(interval);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        print("TRIGGERED");
        if (isActive && other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(rotation * Vector3.up * force, ForceMode2D.Impulse);
        }
    }
}
