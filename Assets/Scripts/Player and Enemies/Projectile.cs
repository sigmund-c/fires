using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float velocity = 10f;
    public float maxRange = 10f;

    public GameObject explosionHitEffect;

    private Damaging damaging;
    private int maxDamage;
    private Rigidbody2D rb;
    private Vector3 startPos;
    private float sqrMaxRange;

    public void Awake()
    {
        damaging = GetComponent<Damaging>();
        maxDamage = damaging.damage;
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        sqrMaxRange = maxRange * maxRange;
    }

    void FixedUpdate()
    {
        if (!rb.isKinematic)
        {
            transform.position += transform.up * velocity * Time.fixedDeltaTime;

            float sqrTravelled = (transform.position - startPos).sqrMagnitude;
            if (sqrTravelled > sqrMaxRange)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Shoot()
    {
        if (rb.isKinematic)
        {
            rb.isKinematic = false;
        }
    }

    public void SetPower(float percent)
    {
        transform.localScale = percent * Vector3.one * 2;
        damaging.damage = (int)(percent * maxDamage) * 2;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Projectile Collision with " + other.gameObject);
        if (explosionHitEffect != null)
        {
            Instantiate(explosionHitEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    //     EnemyController controller = other.collider.GetComponent<EnemyController>();
    //     if (controller != null)
    //     {
    //         e.TakeDamage();
    //     }
        
    //     Destroy(gameObject);
    }
}
