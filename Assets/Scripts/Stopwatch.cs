using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stopwatch : MonoBehaviour
{
    private Text StopwatchText;
    private float startTime;
    private int currentTime;

    // Start is called before the first frame update
    void Start()
    {
        StopwatchText = GetComponent<Text>();
        currentTime = (int)Time.time;
        if (PlayerPrefs.GetFloat("startTime") == null)
        {
             startTime = Time.time;
             PlayerPrefs.SetFloat("startTime", startTime);
        } else 
        {
            startTime = PlayerPrefs.GetFloat("startTime");
        }
        currentTime = (int)Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.time;
        if(currentTime == (int)t)
        {
            return; //Update only once a second
        }
        float timePassed = t - startTime;
        string minutes = ((int)timePassed / 60).ToString("00");
        string seconds = (timePassed % 60).ToString("00");
        
        currentTime = (int)t;
        StopwatchText.text = minutes + ":" + seconds;
    }
}
