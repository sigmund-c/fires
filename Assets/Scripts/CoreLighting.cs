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
        this.light = GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        this.Timer = updateFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        handleTimer();
        adjustLighting();
    }

    void handleTimer()
    {
        if (this.timerRunning)
        {
            this.Timer -= Time.deltaTime;
            if (this.Timer <= 0)
            {
                this.timerRunning = false;
                this.Timer = updateFrequency;
            }
        }
    }

    void adjustLighting()
    {
        if (!this.timerRunning)
        {
            if(this.goDim)
            {
                this.light.intensity -= 0.01f;
                if (this.light.intensity <= this.minIntensity){
                    this.goDim = false;
                }
            } else {
                this.light.intensity += 0.01f;
                if (this.light.intensity >= maxIntensity){
                    this.goDim = true;
                }
            }
            this.timerRunning = true;
        }
    }
}
