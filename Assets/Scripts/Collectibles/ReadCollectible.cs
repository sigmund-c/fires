using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class ReadCollectible : Collectible
{
    public TextManager textManager;
    public ParticleSystem particle;
    public int textIndex;
    new UnityEngine.Experimental.Rendering.Universal.Light2D light;
    private bool setLight;

    new void Start()
    {
        setLight = true;
        light = GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        base.Start();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null)
        {
            textManager.SetBody(textIndex);
            textManager.Enable(true);
            if (setLight)
            {
                setLight = false;
                particle.Play();
                light.intensity = 1.0f;
            }
        }
    }

    new void OnTriggerEnter2D(Collider2D other)
    {

    }

    void OnTriggerExit2D(Collider2D other)
    {
        textManager.Enable(false);
    }
}
