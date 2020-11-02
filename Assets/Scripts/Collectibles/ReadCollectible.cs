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
    private AudioSource audioSource;

    new void Start()
    {
        setLight = true;
        light = GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        audioSource = GetComponent<AudioSource>();
        if (textManager == null)
        {
            textManager = GameObject.Find("TextManager").GetComponent<TextManager>();
        }
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
        if (other.CompareTag("Player"))
        {
            audioSource.Play();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        textManager.Enable(false);
    }
}
