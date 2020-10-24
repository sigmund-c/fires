using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flammable : MonoBehaviour
{
    public static Color BURNING_COLOUR = new Color(1f, 0.7f, 0.35f);

    public bool isBurning;
    public float ignitionTime; // after contacting a burning object, how long to wait before burning
    public float burningTime; // how long to burn for before turning black
    public float blackenTime = 1f; // time to turn black before turning to ashes
    public bool turnToAshes = true;

    public Triggerable toTrigger;

    private SpriteRenderer sr;
    private Burning burning;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        burning = GetComponent<Burning>();
        if (isBurning)
        {
            sr.color = BURNING_COLOUR;
            burning.enabled = true;
            gameObject.layer = Utils.burningLayer;
            
            if (burningTime != -1)
            {
                StartCoroutine(BurnToAshes());
            }
        }
    }

    // check if a burning object is touching me
    void OnTriggerStay2D (Collider2D other)
    {
        // print("Collision between " + this + " and " + other);
        
        if (isBurning)
            return;
        
        Component otherComponent;

        if (other.gameObject.TryGetComponent(typeof(Burning), out otherComponent))
        {
            if (((Burning)otherComponent).enabled)
                StartCoroutine(Ignite());

            if (toTrigger != null)
            {
                toTrigger.Trigger();
            }
        }

        if (other.gameObject.GetComponent<Projectile>() != null)
        {
            Destroy(other.gameObject); // have to use this event rather than making an event in the projectile class, which would not cause burning to happen
        }
    }

    IEnumerator Ignite()
    {
        isBurning = true;
        // print(gameObject + "started burning, changed layer to burning layer");
        gameObject.layer = Utils.burningLayer;
        
        yield return new WaitForSeconds(Random.Range(0f, 0.3f) + ignitionTime);

        burning.enabled = true;
        sr.color = BURNING_COLOUR;
        if (burningTime != -1)
        {
            burning.burningTime = burningTime + blackenTime - 0.2f;
            StartCoroutine(BurnToAshes());
        }
    }

    IEnumerator BurnToAshes()
    {
        yield return new WaitForSeconds(burningTime);

        StartCoroutine(Blacken(blackenTime));
        yield return new WaitForSeconds(blackenTime);

        GetComponent<Collider2D>().enabled = false; // fire hitbox
        
        if (turnToAshes)
        {
            Utils.SpawnScaledParticleSystem(ParticleType.Ashes, transform);
            yield return new WaitForSeconds(0.5f);
            sr.enabled = false;
            transform.GetChild(0).GetComponent<Collider2D>().enabled = false; // phys hitbox
            
            yield return new WaitForSeconds(1f); // for particles to naturally disappear
            Destroy(gameObject);
        }
    }

    IEnumerator Blacken(float duration = 1f)
    {
        float start = Time.time;
        Color original = sr.color;
        while (true && sr.color != Color.black)
        {
            sr.color = Color.Lerp(original, Color.black,  Mathf.Lerp(0, 1, (Time.time-start) / duration));
            // print("Amt: " + Time.time-start/duration);
            yield return new WaitForSeconds(0.01f);
        }
    }

}
