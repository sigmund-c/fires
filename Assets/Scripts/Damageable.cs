using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Team { Player, Enemy, Neutal, None }

public class Damageable : MonoBehaviour
{
    public static Color HITFLASH_COLOR = new Color(1f, 0.58f, 0.58f);
    public int maxHealth = 2;
    public int currHealth = 2;
    public float invincibleDuration = 0f;
    public bool takeKnockback = false;
    public HealthContainer healthContainer;
    public Team team;
    public Team immuneTo = Team.None;

    protected float invincibleTimer = 0; // invincible frames from taking damage
    protected Collider2D colliderObj;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;
    protected AudioSource audioSource;
    protected SpriteMask spriteMask;

    protected Sprite shineSprite;
    protected GameObject shineObject;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        colliderObj = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
            sr = GetComponentInChildren<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        spriteMask = GetComponent<SpriteMask>();

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


        if (damaging != null && damaging.team != team)
        {
            if (damaging.team == immuneTo)
            {
                HitShine();
                return;
            }

            TakeDamage(damaging.damage);
            if (takeKnockback)
            {
                TakeKnockback(damaging.knockbackAmount, damaging.transform.position);
            }
        }
    }
    
    virtual public void TakeDamage(int damage)
    {
        currHealth -= damage;
        StartCoroutine(HitFlash(invincibleDuration));
        PlaySound();
        //Utils.SpawnInfoText(transform.position, damage.ToString(), Vector2.up * 2,InfoTextType.DamageText);
        SliderTakeDamage(damage);

        if (currHealth <= 0)
        {
            Die();
        }
    }

    virtual public void TakeDamageNoInvin(int damage)
    {

        currHealth -= damage;
        //StartCoroutine(HitFlash()); No invincibility frames
        SliderTakeDamage(damage);

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

    virtual public void RestoreHealth(int restore)
    {
        currHealth += restore;
        currHealth = Mathf.Min(currHealth, maxHealth);
        SliderHealDamage(restore);
    }

    public virtual void Die()
    {
        if (team == Team.Enemy)
        {
            // When dead, produces a health pickup that moves to players that heals relative to max health
            Instantiate(Utils.enemyHealthDrop, this.transform.position, Quaternion.identity).GetComponent<HealthCollectibleDrop>().HealAmount = maxHealth + 1;
        }
        Destroy(gameObject);
    }
    
    protected void HitShine()
    {
        if (shineObject != null)
        {
            Destroy(shineObject);
        }

        if (spriteMask == null)
        {
            spriteMask = this.gameObject.AddComponent<SpriteMask>();
            spriteMask.sprite = sr.sprite;
        }
        if (shineSprite == null)
        {
            shineSprite = Resources.Load<Sprite>("Sprites/128x128shine.png");
        }

        shineObject = Instantiate(Utils.shineEffect, transform.position + new Vector3(0, sr.bounds.size.y / 2), Quaternion.identity, transform);

        StopCoroutine(HitShineAnim());
        StartCoroutine(HitShineAnim());
    }

    protected IEnumerator HitShineAnim()
    {
        float timeLeft = 1f;
        float height = sr.bounds.size.y;

        while (timeLeft > 0 && shineObject != null)
        {
            shineObject.transform.Translate(Vector3.down * height * 2 * Time.deltaTime);
            timeLeft -= Time.deltaTime;

            yield return null;
        }

        if (shineObject != null)
        {
            Destroy(shineObject);
        }
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
    protected void SetSliderMax(int health, int overheal = 0)
    {
        if (healthContainer == null)
        {
            return;
        }
        
        healthContainer.Initialize(health, overheal);
        
    }

    protected void SliderTakeDamage(int health)
    {
        if (healthContainer == null)
        {
            return;
        }

        int sliderHealth = healthContainer.takeDamage(health);

        if (sliderHealth != currHealth)
        {
            //Debug.LogWarning("slider health != curr health, slider: " + sliderHealth + ", curr: " + currHealth);
        }
    }

    protected void SliderHealDamage(int health)
    {
        if (healthContainer == null)
        {
            return;
        }

        int sliderHealth = healthContainer.healDamage(health);

        if (sliderHealth != currHealth)
        {
            //Debug.LogWarning("slider health != curr health, slider: " + sliderHealth + ", curr: " + currHealth);
        }
    }
}
