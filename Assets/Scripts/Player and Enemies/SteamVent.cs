using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamVent : MonoBehaviour
{
    public float interval = 1f;
    public float force = 50f;

    public ParticleSystem idleParticles;
    public ParticleSystem activeParticles;
    public bool isActive = false;


    void Start()
    {
        StartCoroutine(Loop());
        idleParticles = transform.GetChild(0).GetComponent<ParticleSystem>();
        activeParticles = transform.GetChild(1).GetComponent<ParticleSystem>();
    }


    IEnumerator Loop()
    {
        yield return new WaitForSeconds(interval);
        while (true)
        {
            idleParticles.Stop();

            yield return new WaitForSeconds(0.1f);

            isActive = true;
            activeParticles.Play();
            // play sound
            yield return new WaitForSeconds(1f);

            isActive = false;
            idleParticles.Play();
            yield return new WaitForSeconds(interval);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        print("TRIGGERED");
        if (isActive && other.gameObject.tag == "Player")
        {
            print(other.gameObject);
            print(other.gameObject.tag);

            other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector3.up * force, ForceMode2D.Impulse);
        }
    }
}
