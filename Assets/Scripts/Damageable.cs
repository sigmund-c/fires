using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team { Player, Enemy, Neutal }

public class Damageable : MonoBehaviour
{
    public static Color HITFLASH_COLOR = new Color(1f, 0.58f, 0.58f);
    public int maxHealth = 10;
    public int currHealth = 10;
    public float invincibleDuration = 1f;
    public bool takeKnockback = false;
    public Team team;

    private float invincibleTimer = 0; // invincible frames from taking damage
    private Collider2D colliderObj;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        colliderObj = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
            sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (invincibleTimer > 0)
        {
            invincibleTimer -= Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (invincibleTimer > 0)
        {
            return;
        }

        Damaging damaging = col.gameObject.GetComponent<Damaging>();

        if (damaging != null && damaging.team != team)
        {
            TakeDamage(damaging.damage);
            if (takeKnockback)
            {
                TakeKnockback(damaging.knockbackAmount, damaging.transform.position);
            }
        }
    }
    
    public void TakeDamage(int damage)
    {
        currHealth -= damage;
        StartCoroutine(HitFlash(invincibleDuration));
        Debug.Log(name + " took " + damage +", with [" + currHealth + "] hp left");

        if (currHealth <= 0)
        {
            Die();
        }
    }

    public void TakeKnockback(float knockback, Vector3 source)
    {
        Vector2 aimVector = source - transform.position;
        Vector2 aimDirection = aimVector.normalized;
        Debug.LogWarning("knocking back " + aimDirection * knockback);
        rb.AddForce(-aimDirection * knockback * 100);
    }

    public void TakeDamageNoInvin(int damage)
    {

        currHealth -= damage;
        StartCoroutine(HitFlash());
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
    
    private IEnumerator HitFlash(float duration = -1f)
    {        
        if (invincibleDuration > 0) // Hitflash should signify iframes
        {
            invincibleTimer = duration == -1 ? invincibleDuration : duration; // default case: use invuln time
            while (invincibleTimer > 0)
            {
                sr.color = HITFLASH_COLOR;
                yield return new WaitForSeconds(0.1f);
                sr.color = Color.white;
                yield return new WaitForSeconds(0.1f);
            }
        }
        else 
        {
            for (int i = 0; i < 2; i++)
            {
                sr.color = HITFLASH_COLOR;
                yield return new WaitForSeconds(0.1f);
                sr.color = Color.white;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
