using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public int maxHealth = 10;
    public int currHealth = 10;
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
    
    public void TakeDamage(int damage)
    {
        currHealth -= damage;
        invincibleTime = invincibleDuration;
        Debug.Log(name + " took " + damage +", with [" + currHealth + "] hp left");

        if (currHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamageNoInvin(int damage)
    {

        currHealth -= damage;
        Debug.Log(name + " took " + damage + ", with [" + currHealth + "] hp left");

        if (currHealth <= 0)
        {
            Die();
        }
    }

    public void RestoreHealth(int restore)
    {
        currHealth += restore;
        Debug.Log(name + " restored " + restore + ", with [" + currHealth + "] hp left");
    }

    private void Die()
    {
        Debug.Log(name + " died");
        Destroy(gameObject);
    }
}
