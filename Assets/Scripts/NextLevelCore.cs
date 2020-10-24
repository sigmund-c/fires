using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelCore : MonoBehaviour
{

    public string sceneName;

    public bool isActivated = false;

    protected SpriteRenderer sr;
    protected Animation anim;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animation>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!isActivated && col.gameObject.tag == "Player")
        {
            col.GetComponent<PlayerDamageable>().currHealth = 50;
            isActivated = true;
            ActivateSprite();
        }
    }

    protected void ActivateSprite()
    {
        Debug.Log("nextLevel");
        GetComponent<AudioSource>().Play();
        anim.Play();
        StartCoroutine(nextLevelDelay());
    }

    IEnumerator nextLevelDelay()
    {
        yield return new WaitForSeconds(8.5f);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
