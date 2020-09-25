using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManagerScript : MonoBehaviour
{

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
}
