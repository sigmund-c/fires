using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosionParticles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().Play();
        Destroy(gameObject, 2f);
    }
}
