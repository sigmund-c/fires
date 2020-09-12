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

    public GameObject projectilePrefab;
    Rigidbody2D rigidbody2d;

    Vector2 mouseDirection = new Vector2(1,0);

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
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

        if(Input.GetMouseButtonDown(1)) //Right click
        {
            Launch();
        }
    }

    void FixedUpdate()
    {
        mouseDirection = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
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

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        BulletProjectile projectile = projectileObject.GetComponent<BulletProjectile>();
        projectile.Launch(mouseDirection, 300);

        this.ChangeHealth(-1);
    }
}
