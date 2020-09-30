﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float velocity = 10f;
    public float maxRange = 10f;

    private Rigidbody2D rb;
    private Vector3 startPos;
    private float sqrMaxRange;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        sqrMaxRange = maxRange * maxRange;
    }

    void FixedUpdate()
    {
        transform.position += transform.up * velocity * Time.fixedDeltaTime;

        float sqrTravelled = (transform.position - startPos).sqrMagnitude;
        if(sqrTravelled > sqrMaxRange)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Projectile Collision with " + other.gameObject);
        Destroy(gameObject);
    //     EnemyController controller = other.collider.GetComponent<EnemyController>();
    //     if (controller != null)
    //     {
    //         e.TakeDamage();
    //     }
        
    //     Destroy(gameObject);
    }
}