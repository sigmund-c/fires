using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rb;

    public new Rigidbody2D rigidbody { get { return rb; }}

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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
