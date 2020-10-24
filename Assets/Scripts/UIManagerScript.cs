using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class UIManagerScript : MonoBehaviour
{
    Rigidbody2D rb;
    public GameObject pauseMenu;
    public GameObject loadMenu;
    public GameObject saveMenu;

    public void StartGame()
    {
        SceneManager.LoadSceneAsync("Level 1-1 - UITest");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnPause()
    {
        Debug.Log("OnPause");

        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void OnResume()//
    {
        Debug.Log("OnResume");

        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        
    }

    public void AtLoadBackToPause()//
    {
        loadMenu.SetActive(false);
        pauseMenu.SetActive(true);

    }

    public void OnMainMenu()//
    {
        //Loading Scene0
        //SceneManager.LoadSceneAsync("MenuScene");
        //Time.timeScale = 1f;
    }

    public Save CreateSaveGameObject()
    {
        Save save = new Save();

        //Store the scene name.
        Scene scene = SceneManager.GetActiveScene();
        save.sceneName = scene.name;

        //Store the player position.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            save.livingTargetPositionsX.Add(rb.position.x);
            save.livingTargetPositionsY.Add(rb.position.y);
        }
        

        //Stored enemies.
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("EnemySaved");
        foreach (GameObject enemy in enemys)
        {
            save.enemyTypes.Add(enemy.name);
            save.enemyPositionsX.Add(enemy.transform.position.x);
            save.enemyPositionsY.Add(enemy.transform.position.y);
        }

        //Stored burnings.
        GameObject[] burnings = GameObject.FindGameObjectsWithTag("BurningObj");
        foreach (GameObject burning in burnings)
        {
            if (burning.name != "PhysicalHitbox")
            {
                save.burningObjTypes.Add(burning.name);
                save.burningObjPositionsX.Add(burning.transform.position.x);
                save.burningObjPositionsY.Add(burning.transform.position.y);
            }
            
        }
        return save;
    }

    public void SaveButton()
    {
        pauseMenu.SetActive(false);

        Component[] saveButtons = saveMenu.GetComponentsInChildren<Button>();
        
        for (int i = 1; i <= 5; i++)
        {
            if (File.Exists(Application.persistentDataPath + "/gamesave" + i + ".save"))
            {
                string archive = "Archive " + i;
                saveButtons[i - 1].GetComponentInChildren<Text>().text = archive;
            }

        }

        saveMenu.SetActive(true);
    }

    public void SaveGame(int NumOfSavedFile)
    {Scene scene = SceneManager.GetActiveScene ();
        Debug.Log("Saving game.");
        //
        Save save = CreateSaveGameObject();

        //
        BinaryFormatter bf = new BinaryFormatter();
        Debug.Log("Storing file are stored at "+Application.persistentDataPath);
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave" + NumOfSavedFile + ".save");
        bf.Serialize(file, save);
        file.Close();
        Debug.Log("Game Saved.");

        saveMenu.SetActive(false);
        pauseMenu.SetActive(true); 
    }

    public void LoadButtonForMainMenu()
    {

        Component[] loadButtons = loadMenu.GetComponentsInChildren<Button>();

        for (int i = 1; i <= 5; i++)
        {
            if (File.Exists(Application.persistentDataPath + "/gamesave" + i + ".save"))
            {
                string archive = "Archive " + i;
                loadButtons[i - 1].GetComponentInChildren<Text>().text = archive;
            }

        }

        loadMenu.SetActive(true);
    }

    public void LoadButton()
    {
        pauseMenu.SetActive(false);

        Component[] loadButtons = loadMenu.GetComponentsInChildren<Button>();

        for (int i = 1; i <= 5; i++)
        {
            if (File.Exists(Application.persistentDataPath + "/gamesave" + i + ".save"))
            {
                string archive = "Archive " + i;
                loadButtons[i - 1].GetComponentInChildren<Text>().text = archive;
            }

        }

        loadMenu.SetActive(true);
    }

    public void LoadGame(int NumOfSavedFile)
    {
        Debug.Log("Loading game.");
        //SceneManager.LoadScene("FireBoiSampleScene");

        //Judge if saving file exists.
        if (File.Exists(Application.persistentDataPath + "/gamesave"+ NumOfSavedFile + ".save"))
        {

            //Get information from file.
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave" + NumOfSavedFile + ".save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            //Activate corresponding scene.
            SceneManager.LoadSceneAsync(save.sceneName);
            //Test if new scene loaded. Load objects when new scene loaded.
            SceneManager.activeSceneChanged += ChangedActiveScene;
            void ChangedActiveScene(Scene current, Scene next)
            {
                //Recover player
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                    rb.position = new Vector2(save.livingTargetPositionsX[0], save.livingTargetPositionsY[0]);
                }

                //Destroy existing enemys.
                GameObject[] enemys = GameObject.FindGameObjectsWithTag("EnemySaved");
                foreach (GameObject enemy in enemys)
                {
                    Destroy(enemy);
                }
                //Destroy existing burnings.
                GameObject[] burnings = GameObject.FindGameObjectsWithTag("BurningObj");
                foreach (GameObject burning in burnings)
                {
                    Destroy(burning);
                }
                //instantiate stored enemys.
                List<GameObject> newEnemys = new List<GameObject>();
                List<GameObject> enemyInsts = new List<GameObject>();
                GameObject enemiesList = GameObject.FindGameObjectWithTag("EnemySavedList"); ;
                int count_enemies = 0;
                foreach (string enemyType in save.enemyTypes)
                {
                    newEnemys.Add((GameObject)Resources.Load("Prefabs/Enemies/" + enemyType));
                    if (newEnemys[count_enemies] != null && enemiesList != null)
                    {
                        enemyInsts.Add(Instantiate(newEnemys[count_enemies], new Vector2(save.enemyPositionsX[count_enemies], save.enemyPositionsY[count_enemies]), new Quaternion(0, 0, 0, 0), enemiesList.transform));
                        //Because the name of instantial prefabs will have "(clone)" at tile. We have to delete this tile.
                        enemyInsts[count_enemies].name = enemyType;
                        Debug.Log("successful load " + enemyType);
                    }
                    else
                    {
                        Debug.Log("Loading errors: Failed add prefabs.");
                    }
                    count_enemies++;
                }



                //instantiate stored burnings.
                List<GameObject> newBurings = new List<GameObject>();
                List<GameObject> burningInsts = new List<GameObject>();
                GameObject burningsList = GameObject.FindGameObjectWithTag("BurningsSavedList"); ;
                int count_burnings = 0;
                foreach (string burningType in save.burningObjTypes)
                {
                    newBurings.Add((GameObject)Resources.Load("Prefabs/Burnings/" + burningType));
                    Debug.Log(newBurings[count_burnings]);
                    Debug.Log(burningsList);
                    if (newBurings[count_burnings] != null && burningsList != null)
                    {
                        burningInsts.Add(Instantiate(newBurings[count_burnings], new Vector2(save.burningObjPositionsX[count_burnings], save.burningObjPositionsY[count_burnings]), new Quaternion(0, 0, 0, 0), burningsList.transform));
                        //Because the name of instantial prefabs will have "(clone)" at tile. We have to delete this tile.
                        burningInsts[count_burnings].name = burningType;
                        Debug.Log("successful load " + burningType);
                    }
                    else
                    {
                        Debug.Log("Loading errors: Failed add " + burningType);
                    }
                    count_burnings++;
                }
            }
            Debug.Log("Game Loaded.");

            loadMenu.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }
}
