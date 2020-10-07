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
    public GameObject projectilePrefab;
    public List<GameObject> enemyPrefabs;

    public void StartGame()
    {
        SceneManager.LoadScene("Level 1-1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public GameObject pauseMenu;

    public void OnPause()
    {
        Debug.Log("OnPause");
        BroadcastMessage("GetMessageFromUIScript", true);
        //SendMessage("GetMessageFromUIScript", false);
        //SendMessageUpwards("GetMessageFromUIScript", false);

        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }

    public void OnResume()//点击“回到游戏”时执行此方法
    {
        Debug.Log("OnResume");
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        
        BroadcastMessage("GetMessageFromUIScript", false);
    }

    public void OnMainMenu()//点击“重新开始”时执行此方法
    {
        //Loading Scene0
        SceneManager.LoadScene("MenuScene");
        Time.timeScale = 1f;
    }

    public Save CreateSaveGameObject()
    {
        Save save = new Save();

        //Store the player position.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        save.livingTargetPositionsX.Add(rb.position.x);
        save.livingTargetPositionsY.Add(rb.position.y);

        //Stored enemies.
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("EnemySaved");
        foreach (GameObject enemy in enemys)
        {
            save.enemyTypes.Add(enemy.name);
            save.enemyPositionsX.Add(enemy.transform.position.x);
            save.enemyPositionsY.Add(enemy.transform.position.y);
        }

        GameObject[] burningObjs = GameObject.FindGameObjectsWithTag("BurningObj");

        return save;
    }

    public void SaveGame()
    {
        Debug.Log("Saving game.");
        // 1
        Save save = CreateSaveGameObject();

        // 2
        BinaryFormatter bf = new BinaryFormatter();
        Debug.Log("Storing file are stored at "+Application.persistentDataPath);
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();
        Debug.Log("Game Saved.");
    }

    public void LoadGame()
    {
        Debug.Log("Loading game.");
        //SceneManager.LoadScene("FireBoiSampleScene");

        //Judge if saving file exists.
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {

            //Get information from file.
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            //Recover player
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

            Vector2 moveDistance = new Vector2(save.livingTargetPositionsX[0]-rb.position.x, save.livingTargetPositionsY[0] - rb.position.y);
            rb.position += moveDistance;

            //Destroy existing enemys.
            GameObject[] enemys = GameObject.FindGameObjectsWithTag("EnemySaved");
            foreach (GameObject enemy in enemys)
            {
                Destroy(enemy);
            }

            //instantiate stored enemys.
            List<GameObject> newEnemys = new List<GameObject>();
            List<GameObject> enemyInsts = new List<GameObject>();
            GameObject enemiesList = GameObject.FindGameObjectWithTag("EnemySavedList"); ;
            int count = 0;
            foreach (string enemyType in save.enemyTypes)
            {
                newEnemys.Add((GameObject)Resources.Load("Prefabs/Enemies/"+ enemyType));
                if (newEnemys[count] != null && enemiesList != null)
                {                
                    enemyInsts.Add(Instantiate(newEnemys[count], new Vector2(save.enemyPositionsX[count], save.enemyPositionsY[count]), new Quaternion(0,0,0,0), enemiesList.transform));
                    //Because the name of instantial prefabs will have "(clone)" at tile. We have to delete this tile.
                    enemyInsts[count].name = enemyType;
                    Debug.Log("successful load "+enemyType);
                }
                else
                {
                    Debug.Log("Loading errors: Failed add prefabs.");
                }
                count++;
            }
            Debug.Log("Game Loaded.");

            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }
}
