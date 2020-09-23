using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int maxHealth = 10;
    public int currHeatlth = 10;
    public float invincibleDuration = 1f;

    private float invincibleTime = 0; // invincible frames from taking damage
    private Collider2D colliderObj;

    // Start is called before the first frame update
    void Start()
    {
        colliderObj = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (invincibleDuration > 0)
        {
            invincibleDuration -= Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (invincibleTime > 0)
        {
            return;
        }

        if (col.gameObject.tag != tag && col.gameObject.GetComponent<Damaging>() != null)
        {
            TakeDamage(col.gameObject.GetComponent<Damaging>().damage);
        }
    }
    
    private void TakeDamage(int damage)
    {
        currHeatlth -= damage;
        invincibleTime = invincibleDuration;
        Debug.Log(name + "took damage of " + damage);

        if (currHeatlth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(name + "died");
    }
}
