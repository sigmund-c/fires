using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChargeEffects : MonoBehaviour
{
    public UnityEngine.Rendering.VolumeProfile volumeProfile;
    private Vignette vignette;
    public float maxVignetteIntensity =  0.25f;
    private float currIntensity = 0f;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        volumeProfile.TryGet(out vignette);
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currIntensity < maxVignetteIntensity) {
            currIntensity += 0.005f;
            vignette.intensity.value = currIntensity;
        }
        if (Input.GetMouseButtonUp(0))
        {
            vignette.intensity.value = 0;
            Destroy(gameObject);
        }
    }
}
