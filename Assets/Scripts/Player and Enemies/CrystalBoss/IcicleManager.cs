using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering.LWRP;

public class IcicleManager : MonoBehaviour
{
    public float icicleSpawnDelay = 0.3f;
    public GameObject iciclePrefab;

    private bool isTriggered;
    private Transform[] spawnPoses;
    private float[] spawnTimes;
    private GameObject[] spawnObjects;
    private float maxTravelDist = 90f;
    private int spawnCounter;
    private UnityEngine.Experimental.Rendering.Universal.Light2D usedLight;

    private Transform icicleLeftPos;
    private Transform[] icicleLeftPoses;
    private Transform icicleRightPos;
    private Transform[] icicleRightPoses;

    private UnityEngine.Experimental.Rendering.Universal.Light2D lightLeft;
    private UnityEngine.Experimental.Rendering.Universal.Light2D lightRight;
    private float lightMaxIntensity;
    private float lightAddIntensity = 0.02f;
    private float lightTimerValue = 0.04f;
    private float lightTimer;
    private bool timerRunning = true;

    private AudioSource audioLeft;
    private AudioSource audioRight;
    private AudioSource usedAudio;

    void Start()
    {
        spawnCounter = 0;
        isTriggered = false;
        icicleLeftPos = transform.Find("IciclePosManager_Left");
        icicleLeftPoses = new Transform[icicleLeftPos.childCount];
        icicleRightPos = transform.Find("IciclePosManager_Right");
        icicleRightPoses = new Transform[icicleLeftPos.childCount];
        for (int i = 0; i < icicleLeftPos.childCount; i++)
        {
            icicleLeftPoses[i] = icicleLeftPos.GetChild(i);
        }
        for (int i = 0; i < icicleRightPos.childCount; i++)
        {
            icicleRightPoses[i] = icicleRightPos.GetChild(i);
        }

        lightLeft = GameObject.Find("IcicleLight_Left").GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        lightRight = GameObject.Find("IcicleLight_Right").GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        lightMaxIntensity = lightLeft.intensity;
        lightTimer = lightTimerValue;

        audioLeft = GameObject.Find("Audio_Left").GetComponent<AudioSource>();
        audioRight = GameObject.Find("Audio_Right").GetComponent<AudioSource>();
    }

    void Update()
    {
        if(isTriggered && timerRunning)
        {
            lightTimer -= Time.deltaTime;
            if (lightTimer <= 0)
            {
                timerRunning = false;
                updateLight();
                lightTimer = lightTimerValue;
            }
        }
    }

    void FixedUpdate()
    {
        if (isTriggered)
        {
            for (int i = 0; i < spawnTimes.Length; i++)
            {
                if(Time.time > spawnTimes[i])
                {
                    spawnObjects[i] = Instantiate(iciclePrefab, spawnPoses[i].position, Quaternion.identity);
                    spawnObjects[i].GetComponent<AudioSource>().PlayDelayed(1);
                    spawnCounter++;
                    spawnTimes[i] = Mathf.Infinity;
                }
            }

            if (spawnCounter == spawnTimes.Length)
            {
                isTriggered = false;
                usedLight.enabled = false;
                usedLight.intensity = lightMaxIntensity;
            }
        } else if (spawnCounter > 0)
        {
            for (int i = 0; i < spawnObjects.Length; i++) {
                if(spawnObjects[i] == null)
                {
                    continue;
                }
                float distTravelled = (spawnObjects[i].transform.position - spawnPoses[i].position).magnitude;
                if (distTravelled >= maxTravelDist)
                {
                    Destroy(spawnObjects[i]);
                    spawnCounter --;
                }
            }
        }
    }

    public float IceDrop()
    {
        int leftRight = Random.Range(0, 2);
        switch (leftRight)
        {
            case 0:
                spawnPoses = icicleLeftPoses;
                spawnTimes = new float[icicleLeftPos.childCount];
                spawnObjects = new GameObject[icicleLeftPos.childCount];
                usedLight = lightLeft;
                usedAudio = audioLeft;
                break;

            case 1:
                spawnPoses = icicleRightPoses;
                spawnTimes = new float[icicleRightPos.childCount];
                spawnObjects = new GameObject[icicleRightPos.childCount];
                usedLight = lightRight;
                usedAudio = audioRight;
                break;
        }
        float currentTime = Time.time;
        for (int i = 0; i < spawnTimes.Length; i++)
        {
            spawnTimes[i] = currentTime + ((i + 1) * icicleSpawnDelay);
        }

        isTriggered = true;
        usedLight.enabled = true;
        usedLight.intensity = 0;
        usedAudio.Play();
        return 5.0f;
    }

    private void updateLight()
    {
        if (usedLight.intensity < lightMaxIntensity)
        {
            usedLight.intensity += lightAddIntensity;
        }
        timerRunning = true;
    }
}
