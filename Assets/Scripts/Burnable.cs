using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burnable : MonoBehaviour
{
    public GameObject fireParticles;

    bool isBurning;
    float startBurningTime;
    // Start is called before the first frame update
    void Start()
    {
        isBurning = false;
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isBurning && collision.tag == "Player")
        {
            isBurning = true;
            GameObject fires = Instantiate(fireParticles, transform);
            startBurningTime = Time.time;
        }
    }

    private void Update()
    {
        if (Time.time - startBurningTime > 2)
        {
            Destroy(this);
        }
    }
}
