﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageable : Damageable
{
    public int maxOverheal = 2;
    public int currOverheal;

    private Coroutine regen;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (healthContainer == null)
        {
            healthContainer = GameObject.Find("HealthContainer").GetComponent<HealthContainer>();
        }
        currOverheal = 0;

        SetSliderMax(maxHealth, maxOverheal);
        regen = StartCoroutine(Regen());
        // lastCheckpoint = transform.position;
    }

    override public void TakeDamage(int damage)
    {
        if (currOverheal > 0)
        {
            if (damage > currOverheal)
            {
                currHealth -= damage - currOverheal;
            }
            currOverheal -= damage;
        } else
        {
            currHealth -= damage;
        }
        StartCoroutine(HitFlash(invincibleDuration));
        PlaySound();
        //Utils.SpawnInfoText(transform.position, damage.ToString(), Vector2.up * 2,InfoTextType.DamageText);
        SliderTakeDamage(damage);
        if (regen != null)
        {
            StopCoroutine(regen);
        }
        regen = StartCoroutine(Regen());

        if (currHealth <= 0)
        {
            Die();
        }
    }

    override public void TakeDamageNoInvin(int damage)
    {

        if (currOverheal > 0)
        {
            currOverheal -= damage;
        }
        else
        {
            currHealth -= damage;
        }
        //StartCoroutine(HitFlash()); No invincibility frames
        SliderTakeDamage(damage);
        if (regen != null)
        {
            StopCoroutine(regen);
        }
        regen = StartCoroutine(Regen());

        if (currHealth <= 0)
        {
            Die();
        }
    }

    override public void RestoreHealth(int restore)
    {
        currHealth += restore;
        if (currHealth > maxHealth)
        {
            currOverheal = Mathf.Min(currHealth - maxHealth, maxOverheal);
            currHealth = maxHealth;
        }
        SliderHealDamage(restore);
    }

    private IEnumerator Regen()
    {
        yield return new WaitForSeconds(0.5f); // small penalty for first regen, also to wait for damage health animation to finish;
        while (currHealth < maxHealth)
        {
            healthContainer.Regen();
            yield return new WaitForSeconds(1.5f); // Regen duration dependent on animation
            if (currHealth < maxHealth) // Prevent overflow in case of healing during regen
            {
                RestoreHealth(1);
            }
        }
    }

    // Override, will respawn
    public override void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        this.gameObject.SetActive(false);
    }
}
