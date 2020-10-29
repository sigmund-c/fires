using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class ReadCollectible : Collectible
{
    public ParticleSystem particle;
    new UnityEngine.Experimental.Rendering.Universal.Light2D light;
    private bool setLight;

    new void Start()
    {
        setLight = true;
        light = GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        base.Start();
    }

    new void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null)
        {
            if (setLight)
            {
                setLight = false;
                particle.Play();
                light.intensity = 1.0f;
            }
            Time.timeScale = 0;
        }

        base.PlayAudio();
    }
}
