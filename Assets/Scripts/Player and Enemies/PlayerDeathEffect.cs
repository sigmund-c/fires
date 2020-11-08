using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathEffect : MonoBehaviour
{
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = 2;
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            StartCoroutine(WaitForRespawn(audioSource.clip.length));
        }
        else
        {
            Destroy(gameObject, 2f);
        }
    }


    IEnumerator WaitForRespawn(float wait)
    {
        yield return new WaitForSeconds(wait * 0.2f);

        if (PersistentManager.instance != null)
            PersistentManager.Reload();
    }
}
