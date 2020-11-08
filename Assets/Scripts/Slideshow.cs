using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Slideshow : MonoBehaviour
{
    public GameObject[] sprites;
    public float[] displayTimes;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DisplaySprites());
    }

    IEnumerator DisplaySprites()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].SetActive(true);
            SpriteRenderer sr = sprites[i].GetComponent<SpriteRenderer>();
            yield return StartCoroutine(FadeIn(sr));
            yield return new WaitForSeconds(displayTimes[i]);
            yield return StartCoroutine(FadeOut(sr));
        }
        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }

    IEnumerator FadeIn(SpriteRenderer sr)
    {
        for (int i = 1; i <= 50; i++)
        {
            sr.color = Color.Lerp(Color.black, Color.white, 1f/50 * i);
            yield return new WaitForSeconds(0.04f);
        }
    }

    IEnumerator FadeOut(SpriteRenderer sr)
    {
        for (int i = 1; i <= 50; i++)
        {
            sr.color = Color.Lerp(Color.white, Color.black, 1f/50 * i);
            yield return new WaitForSeconds(0.03f);
        }
    }
}
