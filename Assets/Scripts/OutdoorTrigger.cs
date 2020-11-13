using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutdoorTrigger : MonoBehaviour
{
    public ParticleSystem[] snowParticles;
    public GameObject cavesBg;
    public GameObject skyBg;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("Player outside");
            foreach(ParticleSystem p in snowParticles)
            {
                // p.SetActive(true);
                p.Play();
            }

            cavesBg.SetActive(false);
            // skyBg.SetActive(true);

        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && GameObject.FindWithTag("Player").activeInHierarchy) // check that exit wasn't due to death
        {
            // print("find tag: " + GameObject.FindWithTag("Player"));
            // print("active:" + GameObject.FindWithTag("Player").activeInHierarchy);
            print("Player inside");
            foreach(ParticleSystem p in snowParticles)
            {
                // p.SetActive(false);
                p.Stop();
            }

            cavesBg.SetActive(true);
            // skyBg.SetActive(false);

        }
    }
}
