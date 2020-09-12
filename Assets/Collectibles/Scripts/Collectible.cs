using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    float originalY;
    public float hoverIntensity = 0.5F;

    // Start is called before the first frame update
    void Start()
    {
        this.originalY = this.transform.position.y;
    }

    // Update is called once per frame
    public void Update()
    {
        //Floating animation
        transform.position = new Vector3(transform.position.x,
            originalY + (Mathf.Sin(Time.time) * hoverIntensity),
            transform.position.z);
    }

    // public void OnTriggerEnter2D(Collider2D other)
    // {
    //     PlayerController controller = other.GetComponent<PlayerController>();

    //     if (controller != null)
    //     {
    //         Destroy(gameObject);
    //     }
    // }
}
