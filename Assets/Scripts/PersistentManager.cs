using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentManager : MonoBehaviour
{
    public static GameObject player;
    public static PersistentManager instance = null;
    public static Vector3 checkpoint;
    public static bool firstRun = true;
    public static string prevScene;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            print("Set initial checkpoint");
            player = GameObject.FindWithTag("Player");
            checkpoint = player.transform.position;
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
        print("firstRun: " + firstRun);
        print("prev: " + prevScene + " curr: " + SceneManager.GetActiveScene().name);
        if (prevScene != SceneManager.GetActiveScene().name) // new level
        {
            firstRun = true;
            print("-----------firstRun set to true");
            prevScene = SceneManager.GetActiveScene().name;
            player = GameObject.FindWithTag("Player");
            checkpoint = player.transform.position;
            print("new checkpoint: " + checkpoint);
        }
        if (!firstRun)
        {
            player = GameObject.FindWithTag("Player");
            player.transform.position = checkpoint;
            Camera.main.transform.position = checkpoint;
        }
    }

    public static void Reload()
    {
        firstRun = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

