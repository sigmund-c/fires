using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class CoreLighting : MonoBehaviour
{
    bool goDim = true;
    public float minIntensity = 0.0f;
    public float maxIntensity = 0.5f;
    new UnityEngine.Experimental.Rendering.Universal.Light2D light;

    bool timerRunning = true;
    public float updateFrequency = 0.04f;
    float Timer;

    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        Timer = updateFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        handleTimer();
    }

    void OnGUI()
    {
        adjustLighting();
    }

    void handleTimer()
    {
        if (timerRunning)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                timerRunning = false;
                Timer = updateFrequency;
            }
        }
    }

    void adjustLighting()
    {
        if (!timerRunning)
        {
            if(goDim)
            {
                light.intensity -= 0.01f;
                if (light.intensity <= minIntensity){
                    goDim = false;
                }
            } else {
                light.intensity += 0.01f;
                if (light.intensity >= maxIntensity){
                    goDim = true;
                }
            }
            timerRunning = true;
        }
    }
}
