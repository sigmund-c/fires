using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamageable : Damageable
{
    public GameObject doubleJumpPowerup;

    private float blackenTime = 1f;
    private AI_BossSnake ai;
    private EffectsStorage effectsStorage;
    SpriteRenderer bodySr;

    private bool hasDied = false;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        ai = GetComponentInParent<AI_BossSnake>();
        effectsStorage = GetComponentInParent<EffectsStorage>();
        bodySr = transform.parent.Find("BossSnakeSprite").GetComponent<SpriteRenderer>();
    }


    // Override, will respawn
    public override void Die()
    {
        StartCoroutine(DeathAndRespawn());
    }

    IEnumerator DeathAndRespawn()
    {
        GetComponent<Collider2D>().enabled = false;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().FocusOn(transform.position, blackenTime + 6f);
        effectsStorage.PlayEffect(1); //Death SFX

        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (enemy.transform.root != transform.root && enemy.GetComponent<Damageable>() != null)
            {
                enemy.GetComponent<Damageable>().Die();
            }
        } 

        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        GetComponent<Collider2D>().enabled = false; // hitbox
        ai.StopAI();
        StartCoroutine(Blacken(blackenTime));
        yield return new WaitForSeconds(blackenTime);


        Utils.SpawnScaledParticleSystem(ParticleType.Ashes, transform.parent); // BurnToAshes
        yield return new WaitForSeconds(0.5f);

        yield return new WaitForSeconds(1f); // for particles to naturally disappear

        effectsStorage.PlayEffect(2); // Respawn SFX
        GetComponentInParent<Animation>().Play("SnakeRespawn");
        yield return new WaitForSeconds(4f); // Respawn length
        sr.color = new Color(1, 1, 1, 1);
        bodySr.color = new Color(1, 1, 1, 1);
        GetComponentInParent<Animation>().Play("CheckpointFloat");


        RestoreHealth(maxHealth - currHealth);
        GetComponent<Collider2D>().enabled = true; // hitbox

        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().FocusOn(transform.position + Vector3.up * 20, 3);
        yield return new WaitForSeconds(1.5f);
        if (!hasDied)
        {
            Instantiate(doubleJumpPowerup, transform.position, Quaternion.identity);
            hasDied = true;
        }
        yield return new WaitForSeconds(1.5f);
        GetComponent<Collider2D>().enabled = true;
        ai.StartAI();
    }
    

    IEnumerator Blacken(float duration = 1f)
    {
        float start = Time.time;
        Color original = sr.color;
        while (true && sr.color != Color.black)
        {
            sr.color = Color.Lerp(original, Color.black, Mathf.Lerp(0, 1, (Time.time - start) / duration));
            bodySr.color = Color.Lerp(original, Color.black, Mathf.Lerp(0, 1, (Time.time - start) / duration));
            // print("Amt: " + Time.time-start/duration);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
