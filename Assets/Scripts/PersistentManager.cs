using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentManager : MonoBehaviour
{
    public static GameObject player;
    public static PersistentManager instance = null;
    public static Vector3 checkpoint;
    public static Vector3 camCheckpoint;
    public static bool firstRun = true;
    public static string prevScene;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            print("== PersistentManager first Awake() ==");
            print("Set initial checkpoint");

            player = GameObject.FindWithTag("Player");
            checkpoint = player.transform.position;
            camCheckpoint = Camera.main.transform.position;
            print("new checkpoint: " + checkpoint);
            print("cam checkpoint:" + camCheckpoint);

            prevScene = SceneManager.GetActiveScene().name;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Store the level to playerPrefs.
        string currScene = SceneManager.GetActiveScene().name;
        if (currScene != "MenuScene" && currScene != "EndingVideo")
        {
            PlayerPrefs.SetString("Last_Level", SceneManager.GetActiveScene().name);
            Debug.Log(currScene + " saved.");
        }
        
        if (prevScene != currScene) // new level
        {
            firstRun = true;
            print("== First run (new level) ==");
            
            if (currScene != "MenuScene" && currScene != "EndingVideo")
            {
                player = GameObject.FindWithTag("Player");
                checkpoint = player.transform.position;
                camCheckpoint = Camera.main.transform.position; // at the start, camera won't be on player

                prevScene = currScene;
            }

        }
        if (!firstRun)
        {
            player = GameObject.FindWithTag("Player");
            player.transform.position = checkpoint;
            Camera.main.transform.position = camCheckpoint;
        }
    }

    public static void Reload()
    {
        firstRun = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

