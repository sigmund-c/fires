using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AirJumpBehaviour { PreserveMomentum, CancelOnAim, CancelOnDash };

public class PlayerController : MonoBehaviour
{
    //Collectible variables
    //Health
    public int maxHealth = 50;
    public int health { get { return currentHealth; }}
    
    private int currentHealth;

    //JumpBoost
    public float boostDuration = 10.0f;
    public float boostMultiplier = 1.2f; //Use this in movement mechanics
    
    private bool isBoosted;
    private float boostTimer;

    //Movement variables
    public float maxJumpCharge = 1f;
    public float baseJumpChange = 0.3f;
    public int maxJumps = 1; // 0 means any time.
    public AirJumpBehaviour airJumpBehaviour;
    public GameObject launchBar;
    
    private float chargeStartTime;
    private bool isCharging;
    private int jumpTimes = 0;
    private LaunchBar activeLaunchBar;

    //Projectiles
    public GameObject projectilePrefab;

    private Rigidbody2D rb;
    private Transform sprite;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = transform.GetChild(1);
        currentHealth = maxHealth/2;
    }

    void Update()
    {
        Vector3 mousePosition = Utils.MouseWorldPosition();
        Vector3 aimDirection = (mousePosition - transform.position).normalized;

        handleTimers();

        if (!isCharging)
        {
            if (Input.GetMouseButtonDown(1)) // right click
            {
                LaunchProjectile(aimDirection);
            }
            else if (Input.GetMouseButtonDown(0))
            {
                if (maxJumps != 0 && jumpTimes < maxJumps)
                {
                    isCharging = true;
                    chargeStartTime = Time.time;
                    sprite.localScale = new Vector3(1, 0.5f, 1);

                    if (airJumpBehaviour == AirJumpBehaviour.CancelOnAim)
                    {
                        rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
                        rb.gravityScale = 0;
                    }

                     // Instantiate LaunchBar
                    activeLaunchBar = Instantiate(launchBar, transform.position, Quaternion.FromToRotation(Vector3.right, aimDirection), transform).GetComponent<LaunchBar>();
                }
                else 
                {
                    // possibly play an "invalid" sound
                }                
            }
        }
        else // already charging
        {
            /*if (Input.GetMouseButton(0) && Time.time - chargeStartTime > maxJumpCharge && isCharging) // Jump when held too long
            {
                FinishCharge(Time.time - chargeStartTime, aimDirection);
            }
            else*/ if (Input.GetMouseButton(0)) 
            {
                // When holding down button, increase bar
                if (activeLaunchBar != null)
                {
                    float percent = Mathf.Min((Time.time - chargeStartTime) / maxJumpCharge, 1f);
                    activeLaunchBar.SetSize(percent);
                    activeLaunchBar.UpdateDirection(Quaternion.FromToRotation(Vector3.right, aimDirection));
                }
            }
            else if (Input.GetMouseButtonUp(0)) // Jump when release
            {
                FinishCharge(Time.time - chargeStartTime, aimDirection);

            }

            if (Input.GetMouseButtonDown(1)) // cancel charge
            {
                FinishCharge();
            }
        }
    }

    void handleTimers()
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

    void FinishCharge(float jumpAmount = 0 , Vector2 dir = default(Vector2))
    {
        isCharging = false;
        if (activeLaunchBar != null)
        {
            Destroy(activeLaunchBar.gameObject);
        }

        sprite.localScale = new Vector3(1, 1, 1);

        if (jumpAmount != 0)
        {
            if (jumpTimes > 0) // jumpTimes only increases after 
            {
                Debug.Log("tset");
                jumpTimes++;
            }
            Jump(jumpAmount, dir);
        }
        rb.gravityScale = 1;
    }

    void Jump(float jumpAmount, Vector2 dirc)
    {
        if (jumpAmount > maxJumpCharge)
        {
            jumpAmount = maxJumpCharge;
        }
        
        if (airJumpBehaviour == AirJumpBehaviour.CancelOnDash)
        {
            rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        }

        if (isBoosted)
        {
            rb.AddForce(dirc * 15 * (jumpAmount + baseJumpChange) * boostMultiplier, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(dirc * 15 * (jumpAmount + baseJumpChange), ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            // Check if the "wall" is less than 135 degrees from down, preventing ceiling jumps
            Debug.Log(Vector2.Angle(Vector2.down, transform.position - col.transform.position));
            if (Vector2.Angle(Vector2.down, transform.position - col.transform.position) < 135) 
            {
                jumpTimes = 0;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        // Starts the jump counter only after leaving the ground (by jumping, or sliding off the ground)
        if (col.gameObject.tag == "Ground")
        {
            jumpTimes = 1;
        }
    }

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        // Debug.Log(currentHealth + "/" + maxHealth);
    }

    public void BoostMovement()
    {
        isBoosted = true;
        boostTimer = boostDuration;
    }

    void LaunchProjectile(Vector3 aimDirection)
    {
        // Vector3 direction = Vector3.up;
        // if (Mathf.Abs(aimDirection.y) > Mathf.Abs(aimDirection.x))
        // {
        //     if (aimDirection.y > 0) direction = Vector3.up;
        //     if (aimDirection.y < 0) direction = Vector3.down;
        // }
        // else
        // {
        //     if (aimDirection.x > 0) direction = Vector3.right;
        //     if (aimDirection.x < 0) direction = Vector3.left;
        // }

        // GameObject projectileObject = Instantiate(projectilePrefab,
        //     rb.position + (Vector2)aimDirection.normalized * 0.5f,
        //     Quaternion.FromToRotation(direction, Vector3.up));

        // Projectile projectile = projectileObject.GetComponent<Projectile>();
        // projectile.Launch((Vector2)aimDirection, 200);

        GameObject projectileInst = Instantiate(projectilePrefab, rb.position + (Vector2)aimDirection.normalized * 0.5f, Quaternion.FromToRotation(Vector3.up, aimDirection));

        ChangeHealth(-1);
    }
}
