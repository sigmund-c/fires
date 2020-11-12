using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAwake : MonoBehaviour
{
    private void Awake()
    {
        if (PlayerPrefs.HasKey("volume") == true)
        {
            float storedVolume = PlayerPrefs.GetFloat("volume");
            AudioListener.volume = storedVolume;
        }
    }
}
