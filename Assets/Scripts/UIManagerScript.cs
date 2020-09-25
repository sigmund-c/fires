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

    public void StartGame()
    {
        SceneManager.LoadScene("FireBoiSampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public GameObject pauseMenu;

    public void OnPause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void OnResume()//点击“回到游戏”时执行此方法
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
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
        GameObject a = GameObject.FindGameObjectWithTag("Player");
        Rigidbody2D rb = a.GetComponent<Rigidbody2D>();
        Debug.Log(a);
        Debug.Log(rb);
        Debug.Log(rb.position.x);
        Debug.Log(rb.position.y);
        save.livingTargetPositionsX.Add(rb.position.x);
        save.livingTargetPositionsY.Add(rb.position.y);

        return save;
    }

    public void SaveGame()
    {
        // 1
        Save save = CreateSaveGameObject();

        // 2
        BinaryFormatter bf = new BinaryFormatter();
        Debug.Log(Application.persistentDataPath);
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();
        Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
        //SceneManager.LoadScene("FireBoiSampleScene");

        // 1
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {

            // 2
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            // 3
            GameObject a = GameObject.FindGameObjectWithTag("Player");
            Debug.Log(a);
            Rigidbody2D rb = a.GetComponent<Rigidbody2D>();
            Debug.Log(rb);
            Debug.Log(rb.position);
            Debug.Log(save.livingTargetPositionsX[0]);
            Debug.Log(save.livingTargetPositionsY[0]);

            Vector2 moveDistance = new Vector2(save.livingTargetPositionsX[0]-rb.position.x, save.livingTargetPositionsY[0] - rb.position.y);
            rb.position += moveDistance;
            Debug.Log(rb.position);
            Debug.Log("Game Loaded");

            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }
}
