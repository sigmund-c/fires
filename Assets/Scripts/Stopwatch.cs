using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stopwatch : MonoBehaviour
{
    private Text StopwatchText;
    private int startTime;
    private int currentTime;

    // Start is called before the first frame update
    void Start()
    {
        StopwatchText = GetComponent<Text>();
        currentTime = (int)Time.time;
        if (PlayerPrefs.GetInt("startTime") == null)
        {
             startTime = (int)Time.time;
             PlayerPrefs.SetInt("startTime", startTime);
        } else 
        {
            startTime = PlayerPrefs.GetInt("startTime");
        }
        currentTime = (int)Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        int t = (int)Time.time;
        if(currentTime == t)
        {
            return; //Update only once a second
        }
        int timePassed = t - startTime;
        
        if(timePassed >= 0)
        {
            string minutes = (timePassed / 60).ToString("00");
            string seconds = (timePassed % 60).ToString("00");
            StopwatchText.text = minutes + ":" + seconds;
        } else 
        {
            startTime = t;
            PlayerPrefs.SetInt("startTime", startTime);
        }
        currentTime = t;
    }
}
