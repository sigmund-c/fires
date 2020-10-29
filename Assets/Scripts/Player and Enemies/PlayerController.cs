using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public enum AirJumpBehaviour { PreserveMomentum, CancelOnAim, CancelOnDash };


// Notes on fire swimming:
// We need a separate fire detection hitbox and physical hitbox for the player. If we only had one for both, whenever we change that hitbox layer from Character to CharacterSwimming,
// it would register as a CollisionExit event (that the player has left the source of fire), and hence make the player go out of swim mode, only for it to collide with fire again
// and go back to swim mode and so on repeatedly
public class PlayerController : MonoBehaviour
{
    //Collectible variables
    //Health
    private Damageable damageable;

    //JumpBoost
    public float boostDuration = 30.0f;
    public float boostMultiplier = 1.2f; //Use this in movement mechanics

    private bool isBoosted;
    private float boostTimer;

    //Movement variables
    public float baseJumpAmount = 0.3f;
    public float maxJumpAmountFromCharge = 1f; // from charging, excludes base amount
    public float maxChargeTime = 1f;
    public int maxJumps = -1; // -1 means unlimited
    public AirJumpBehaviour airJumpBehaviour;
    public GameObject launchBar;

    // we use GetMouseButton instead of GetMouseButtonDown to start charging so you can charge immediately after touching a surface by holding it down.
    // but this restarts the charge immediately after cancelling using M2, so we add a cooldown
    public float chargeCooldown = 0.2f;
    private float chargeCooldownTimer;

    private float chargeStartTime;
    private bool isCharging;
    private bool playChargingSound = true;
    public int collidingObjects = 0;
    public int jumpTimes = 0;
    private LaunchBar activeLaunchBar;

    private int numBurningObjsTouched;

    //Projectiles
    public GameObject projectilePrefab;

    private Rigidbody2D rb;
    private Transform sprite;
    private SpriteRenderer sr;

    private ContactFilter2D burningFilter;

    private bool inSwimMode;
    private GameObject fireHitbox;

    public bool canSwim; // NOTE: remember to change physics collision matrix accordingly
    public float regularDrag;
    public float swimmingDrag;
    public float originalGravity = 2f;
    public float aimingTimeScale = 0.5f;

    private bool touchingGround;
    private Animator animator;

    private EffectsStorage effectsStorage;

    public float recoilAmount = 10f;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = transform.GetChild(0);
        sr = sprite.GetComponent<SpriteRenderer>();
        damageable = GetComponent<Damageable>();
        burningFilter = new ContactFilter2D();
        burningFilter.useTriggers = true;
        burningFilter.SetLayerMask(LayerMask.GetMask("Burning")); // DO NOT use LayerMask.NameToLayer here -- it returns an int instead of bitmask
        // print("burning filter: " + LayerMask.LayerToName(burningFilter.layerMask));
        fireHitbox = transform.GetChild(1).gameObject;
        animator = sprite.GetComponent<Animator>();
        effectsStorage = GetComponent<EffectsStorage>();
        effectsStorage.PlayEffect(2); // spawn SFX

        if (PersistentManager.instance == null)
        {
            Instantiate(Utils.persistentManager, Vector3.zero, Quaternion.identity);
        }
    }

    void SetSwimMode()
    {
        // print("num burning objs touched: " + numBurningObjsTouched);
        // if (numBurningObjsTouched > 0)
        // {
        //     sr.color = Color.green; // debugging
        //     rb.gravityScale = 0;
        //     rb.velocity = Vector3.zero;
        //     gameObject.layer = Utils.charSwimmingLayer;
        //     // print("swim mode on");
        // }
        // else
        // {
        //     sr.color = Color.white;
        //     rb.gravityScale = 1;
        //     gameObject.layer = Utils.charLayer;
        //     // print("swim mode off");
        // }
    }

    IEnumerator Respawn(Vector3 pos)
    {
        yield return new WaitForSeconds(0.5f);
        transform.position = pos;
        Camera.main.transform.position = pos;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            PersistentManager.Reload();
            // Vector3 prevLoc = PersistentManager.instance.checkpoint;
            // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            // StartCoroutine(Respawn(prevLoc));
        }

        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            int sceneID = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(sceneID + 1);
        }
        else if (Input.GetKeyDown(KeyCode.PageDown))
        {
            int sceneID = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(sceneID - 1);
        }

        Vector3 mousePosition = Utils.MouseWorldPosition();
        Vector3 aimVector = mousePosition - transform.position;
        Vector3 aimDirection = aimVector.normalized;

        handleTimers();

        if (canSwim && inSwimMode) // constantly move towards cursor
        {
            float mag = aimVector.magnitude;
            float amount = Mathf.Min(mag > 3f ? Mathf.Log(mag) : 0f, 2f); // scale with distance from player to cursor logarithmically, cap at 2
            print("Amount: " + amount + " Mouse dist: " + mag);
            rb.AddForce(aimDirection * amount * 0.15f, ForceMode2D.Impulse);
            // TODO - should we set AirJumpBehaviour to PreserveMomentum only when swimming? or all the time?
        }

        // stop all input if hovering over UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (!isCharging)
        {
            if (!inSwimMode && Input.GetMouseButtonDown(1)) // right click
            {
                if (damageable.currHealth > 1) // prevent death from clicking
                {
                    LaunchProjectile(aimDirection);
                }
            }
            else if (Input.GetMouseButton(0) && chargeCooldownTimer == 0f)
            {
                if (maxJumps == -1 || jumpTimes < maxJumps)
                {
                    isCharging = true;
                    chargeStartTime = Time.time;
                    sprite.localScale = new Vector3(1, 0.5f, 1);
                    Time.timeScale = aimingTimeScale;

                    if (playChargingSound)
                    {
                        playChargingSound = false;
                        effectsStorage.PlayEffect(3); // charge SFX
                    }

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
            else*/
            if (Input.GetMouseButton(0))
            {
                // When holding down button, increase bar
                if (activeLaunchBar != null)
                {
                    float percent = Mathf.Min((Time.time - chargeStartTime) / maxChargeTime, 1f);
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

    void FixedUpdate()
    {
        if (!canSwim)
            return;

        List<Collider2D> results = new List<Collider2D>();
        Physics2D.OverlapCircle(transform.position, 0.2f, burningFilter, results);

        // exclude player's own fire hitbox
        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject == fireHitbox)
            {
                results.Remove(results[i]);
                break;
            }
        }

        // debugging
        // if (results.Count > 0)
        // {
        //     string s = "";
        //     foreach (Collider2D c in results)
        //         s += c + ", ";
        //     print(s);
        // }

        if (results.Count > 0 && !inSwimMode)
        {
            inSwimMode = true;
            sr.color = Color.green; // debugging
            rb.gravityScale = 0;
            rb.velocity = Vector3.zero;
            gameObject.layer = Utils.charSwimmingLayer;
            rb.drag = swimmingDrag;
        }
        else if (results.Count == 0 && inSwimMode)
        {
            inSwimMode = false;
            sr.color = Color.white;
            rb.gravityScale = originalGravity;
            gameObject.layer = Utils.charLayer;
            rb.drag = regularDrag;
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

        if (chargeCooldownTimer > 0f)
        {
            chargeCooldownTimer = Mathf.Max(chargeCooldownTimer - Time.deltaTime, 0f);
        }
    }

    void FinishCharge(float jumpChargeTime = 0 , Vector2 dir = default(Vector2))
    {
        isCharging = false;
        playChargingSound = true;
        chargeCooldownTimer = chargeCooldown;
        if (activeLaunchBar != null)
        {
            Destroy(activeLaunchBar.gameObject);
        }

        sprite.localScale = new Vector3(1, 1, 1);
        Time.timeScale = 1f;

        if (jumpChargeTime != 0)
        {
            if (/*!touchingGround*/jumpTimes > 0) // jumpTimes only increases after
            {
                // Debug.Log("tset");
                print("jumpTimes++");
                jumpTimes++;
            }
            Jump(jumpChargeTime, dir);
        }
        if (!inSwimMode) // TODO
            rb.gravityScale = originalGravity;
    }

    void Jump(float jumpChargeTime, Vector2 dirc)
    {
        animator.SetTrigger("Jump");
        effectsStorage.PlayEffect(1); // jump SFX
        if (jumpChargeTime > maxChargeTime)
        {
            jumpChargeTime = maxChargeTime;
        }

        if (airJumpBehaviour == AirJumpBehaviour.CancelOnDash)
        {
            rb.velocity = Vector3.zero;
        }

        float amount = jumpChargeTime / maxChargeTime * maxJumpAmountFromCharge + baseJumpAmount;

        if (isBoosted)
        {
            rb.AddForce(dirc * 15 * amount * boostMultiplier, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(dirc * 15 * amount, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    // void OnCollisionStay2D(Collision2D col)
    {
        // Debug.Log(col.gameObject.name);
        if (col.gameObject.tag == "Ground" || col.gameObject.tag == "BurningObj")
        {
            // touchingGround = true;

            // check angle
            // ContactPoint2D[] contacts = new ContactPoint2D[col.contactCount];
            // col.GetContacts(contacts);
            // print("CONTACTS:");
            // foreach (ContactPoint2D point in contacts)
            // {
            //     print(point);
            // }

            collidingObjects++; // Take into account when colliding with multiple objects before Exiting

            print("jumpTimes = 0");
            jumpTimes = 0;
            Utils.SpawnSparkle(transform);


            // Not working on tilemaps :(
            /*
            // Check if the "wall" is less than 135 degrees from down, preventing ceiling jumps
            Debug.Log(Vector2.Angle(Vector2.down, col.transform.position - transform.position));
            if (Vector2.Angle(Vector2.down, col.transform.position - transform.position) < 135)
            {
                jumpTimes = 0;
            }*/
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        // Starts the jump counter only after leaving the ground (by jumping, or sliding off the ground)
        if (col.gameObject.tag == "Ground" || col.gameObject.tag == "BurningObj")
        {
            print("touchingGround = false");
            // touchingGround = false;
            // print("jumpTimes = 1");
            collidingObjects--;
            if (collidingObjects <= 0) { // Only start counting when not touching anything
                jumpTimes = 1;
            }
        }
    }

    // don't delete first -- in case the curr implementation has issues, may have to use this method
    // void OnTriggerEnter2D(Collider2D col)
    // {
    //     print("Trigger touched " + col + " of layer " + col.gameObject.layer);
    //     if (col.gameObject.tag == "BurningObj")
    //     {
    //         Flammable flammable = col.gameObject.GetComponent<Flammable>();
    //         if (flammable != null && flammable.isBurning)
    //         {
    //             print(">other obj is burning");
    //             numBurningObjsTouched++;
    //             SetSwimMode();
    //         }
    //     }
    // }

    // void OnTriggerExit2D(Collider2D col)
    // {
    //     if (col.gameObject.tag == "BurningObj")
    //     {
    //         Flammable flammable = col.gameObject.GetComponent<Flammable>();
    //         if (flammable != null && flammable.isBurning)
    //         {
    //             print("-----left a burning object" + col + " of layer " + col.gameObject.layer);
    //             numBurningObjsTouched--;
    //             SetSwimMode();
    //         }

    //     }
    // }

    public void TakeDamage(int amount)
    {
        damageable.TakeDamageNoInvin(amount);
        // Debug.Log(currentHealth + "/" + maxHealth);
    }

    public void RestoreHealth(int amount)
    {
        damageable.RestoreHealth(amount);
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
        rb.AddForce(-1 * aimDirection * recoilAmount, ForceMode2D.Impulse); // Add "recoil" knockback

        effectsStorage.PlayEffect(0); // shoot SFX

        TakeDamage(1);
    }
}
