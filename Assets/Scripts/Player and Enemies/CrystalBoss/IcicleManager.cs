using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleManager : MonoBehaviour
{
    public float icicleSpawnDelay = 0.3f;
    public GameObject iciclePrefab;

    private bool isTriggered;
    private Transform[] spawnPoses;
    private float[] spawnTimes;
    private int spawnCounter;

    private Transform icicleLeftPos;
    private Transform[] icicleLeftPoses;
    private Transform icicleRightPos;
    private Transform[] icicleRightPoses;

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
    }

    void Update()
    {
        if (isTriggered)
        {
            for (int i = 0; i < spawnTimes.Length; i++)
            {
                if(Time.time > spawnTimes[i])
                {
                    Instantiate(iciclePrefab, spawnPoses[i].position, Quaternion.identity);
                    spawnCounter++;
                    spawnTimes[i] = Mathf.Infinity;
                }
            }

            if (spawnCounter == spawnTimes.Length)
            {
                isTriggered = false;
                spawnCounter = 0;
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
                break;

            case 1:
                spawnPoses = icicleRightPoses;
                spawnTimes = new float[icicleRightPos.childCount];
                break;
        }
        float currentTime = Time.time;
        for (int i = 0; i < spawnTimes.Length; i++)
        {
            spawnTimes[i] = currentTime + ((i + 1) * icicleSpawnDelay);
        }

        isTriggered = true;
        return 5.0f;
    }
}
