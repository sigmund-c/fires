﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Team { Player, Enemy, Neutal, None }

public class Damageable : MonoBehaviour
{
    public static Color HITFLASH_COLOR = new Color(1f, 0.58f, 0.58f);
    public int maxHealth = 10;
    public int currHealth = 10;
    public float invincibleDuration = 1f;
    public bool takeKnockback = false;
    public Slider healthSlider;
    public Team team = Team.Neutal;

    public Team immuneTo = Team.None;

    protected float invincibleTimer = 0; // invincible frames from taking damage
    protected Collider2D colliderObj;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected AudioSource audioSource;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        colliderObj = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
            sr = GetComponentInChildren<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        SetSliderMax(maxHealth);
    }

    protected void Update()
    {
        if (invincibleTimer > 0)
        {
            invincibleTimer -= Time.deltaTime;
        }
    }

    public void OnCollisionEnter2D(Collision2D col)
    {
        if (invincibleTimer > 0)
        {
            return;
        }

        Damaging damaging = col.gameObject.GetComponent<Damaging>();

        // Default immunity to same team; additional immunity to immuneTo
        if (damaging != null && damaging.team != team && damaging.team != immuneTo)
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
        PlaySound();
        //Utils.SpawnInfoText(transform.position, damage.ToString(), Vector2.up * 2,InfoTextType.DamageText);
        SetSliderHealth(currHealth);

        if (currHealth <= 0)
        {
            Die();
        }
    }

    public void TakeKnockback(float knockback, Vector3 source)
    {
        Vector2 aimVector = source - transform.position;
        Vector2 aimDirection = aimVector.normalized;
        rb.AddForce(-aimDirection * knockback * 100);
    }

    public void TakeDamageNoInvin(int damage)
    {

        currHealth -= damage;
        //StartCoroutine(HitFlash()); No invincibility frames
        Debug.Log(name + " took " + damage + ", with [" + currHealth + "] hp left");
        SetSliderHealth(currHealth);

        if (currHealth <= 0)
        {
            Die();
        }
    }

    public void RestoreHealth(int restore)
    {
        currHealth += restore;
        currHealth = Mathf.Min(currHealth, maxHealth);
        Debug.Log(name + " restored " + restore + ", with [" + currHealth + "] hp left");
        SetSliderHealth(currHealth);
    }

    public virtual void Die()
    {
        Debug.Log(name + " died");
        Destroy(gameObject);
    }
    
    protected IEnumerator HitFlash(float duration = -1f)
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

        colliderObj.enabled = false; // Re-check for OnCollisionEnter
        colliderObj.enabled = true;
    }

    protected void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    // ============== UI ===================
    protected void SetSliderMax(int health)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = health;
            healthSlider.value = health;
        }
    }

    protected void SetSliderHealth(int health)
    {
        if (healthSlider != null)
        {
            healthSlider.value = health;
        }
    }
}
