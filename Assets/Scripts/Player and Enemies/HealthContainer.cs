using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthContainer : MonoBehaviour
{
    public int maxHealth = 3;
    public int currHealth;

    public int maxOverheal = 0;
    public int currOverheal;

    public GameObject oneHealth;
    private Animation[] healths;
    public GameObject oneOverheal;
    private Animation[] overheals;

    void Start()
    {
        healths = new Animation[maxHealth];
        for(int i = 0; i < maxHealth; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(100 * i, 0); // each container is 100 to the right of the previous
            healths[i] = GameObject.Instantiate(oneHealth, pos, Quaternion.identity, this.transform).GetComponent<Animation>();
        }

        overheals = new Animation[maxOverheal];
        for (int i = 0; i < maxOverheal; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(100 * (i + maxHealth), 0);
            overheals[i] = GameObject.Instantiate(oneOverheal, pos, Quaternion.identity, this.transform).GetComponent<Animation>();
        }

        currHealth = maxHealth;
        currOverheal = 0;
    }


    public void Initialize(int maxHealth, int maxOverheal)
    {
        foreach(Animation health in healths)
        {
            GameObject.Destroy(health.gameObject);
        }
        foreach (Animation overheal in overheals)
        {
            GameObject.Destroy(overheal.gameObject);
        }

        this.maxHealth = maxHealth;
        this.maxOverheal = maxOverheal;
        Start();
    }

    // Deals one damage, returns remaining Health
    public int takeDamage()
    {
        if (currOverheal > 0)
        {
            overheals[currOverheal - 1].Play("healthDamage");
            currOverheal--;

            return currHealth;
        }
        healths[currHealth - 1].Play("healthDamage");
        currHealth--;

        return currHealth;
    }

    // Deals some damage, returns remaining Health
    public int takeDamage(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            if (currHealth < 1)
            {
                break;
            }
            takeDamage();
        }

        return currHealth;
    }

    //  Heals one health, returns new health
    public int healDamage()
    {
        if (currHealth >= maxHealth) // restore overheal if health is full
        {
            if (currOverheal >= maxOverheal) // return if overheal is also full
            {
                return currHealth;
            } else
            {
                overheals[currOverheal].Play("healthFull");
                currOverheal++;
            }
        } else
        {
            healths[currHealth].Play("healthFull");
            currHealth++;
        }

        return currHealth;
    }

    //  Heals some health, returns new health
    public int healDamage(int heal)
    {
        for (int i = 0; i < heal; i++)
        {
            healDamage();
        }

        return currHealth;
    }

    // If the player takes more damage before fully regen, cancel the regen animation
    public void Regen()
    {
        if (currHealth < maxHealth)
        {
            // make sure the previous regen animations are cancelled
            for (int i = currHealth + 1; i < maxHealth; i++)
            {
                healths[i].Stop();
                healths[i].Play("healthEmpty");
            }
            healths[currHealth].PlayQueued("healthRegen");
        }
    }

}
