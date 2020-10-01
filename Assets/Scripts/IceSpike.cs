using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpike : MonoBehaviour
{
    private Rigidbody2D rb;

    private bool fallen = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!fallen)
        {
            fallen = true;
            GetComponent<AudioSource>().Play();
            if (col.otherCollider is BoxCollider2D)
            {
                rb.gravityScale = 0.9f;
            }
        }
    }
}
