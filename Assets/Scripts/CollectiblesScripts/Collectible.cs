using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    float originalY;
    public float hoverIntensity = 0.25F;

    private new AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        originalY = transform.position.y;
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void Update()
    {
        //Floating animation
        transform.position = new Vector3(transform.position.x,
            originalY + (Mathf.Sin(Time.time) * hoverIntensity),
            transform.position.z);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null)
        {
            StartCoroutine(DestroyCollectible());
        }
    }
    IEnumerator DestroyCollectible()
    {
        audio.Play();
        yield return new WaitForSeconds(0.6f);
        Destroy(gameObject);
    }
}
