using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;

    public Rigidbody2D rigidbody { get { return rigidbody2d; }}

    public void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    public void Update()
    {
        if(transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
    //     EnemyController controller = other.collider.GetComponent<EnemyController>();
    //     if (controller != null)
    //     {
    //         e.TakeDamage();
    //     }
        
    //     Destroy(gameObject);
    }
}
