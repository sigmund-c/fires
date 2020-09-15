using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burning : MonoBehaviour
{
    public float burningTime;
    public bool showParticles = true;

    void Start()
    {
        if (showParticles)
            Utils.SpawnScaledParticleSystem(ParticleType.Fire, transform, burningTime);
    }
}
