using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpPowerup : Collectible
{
    Transform player;

    public float moveSpeed = 10f;
    public GameObject pickupWooshEffect;

    // Start is called before the first frame update
    new protected void Start()
    {
        base.Start();
        player = GameObject.Find("Player").transform;
        float random = Random.Range(0f, 360f);
        Vector2 randomVector = new Vector2(Mathf.Cos(random), Mathf.Sin(random));
        GetComponent<Rigidbody2D>().AddForce(randomVector * 4);
    }

    new public void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
    }


    new void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null)
        {
            controller.maxJumps = 2; // set maxJumps to 2
            Instantiate(pickupWooshEffect, this.transform.position, Quaternion.identity);
            base.OnTriggerEnter2D(other);
        }
    }
}
