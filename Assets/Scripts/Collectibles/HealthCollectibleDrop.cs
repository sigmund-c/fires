using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectibleDrop : HealthCollectible
{
    Transform player;

    public float moveSpeed = 10f;


    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        player = GameObject.Find("Player").transform;
        float random = Random.Range(0f, 360f);
        Vector2 randomVector = new Vector2(Mathf.Cos(random), Mathf.Sin(random));
        GetComponent<Rigidbody2D>().AddForce(randomVector * 4);
    }

    override public void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
    }

}
