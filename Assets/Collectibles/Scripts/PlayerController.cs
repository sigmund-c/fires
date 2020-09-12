using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int maxHealth = 50;
    public float boostDuration = 10.0f;
    public float boostMultiplier = 1.5f; //Use this in movement mechanics

    int currentHealth;
    public int health { get { return currentHealth; }}

    bool isBoosted;
    float boostTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isBoosted)
        {
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0)
            {
                isBoosted = false;
            }
        }
    }

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
    }

    public void BoostMovement()
    {
        isBoosted = true;
        boostTimer = boostDuration;
    }
}
