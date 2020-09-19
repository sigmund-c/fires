using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpMovement : MonoBehaviour
{
    private static float maxJump = 2f;
    float jumpStart;
    float jumpCharge;

    bool isCharging;
    bool isJumping;

    Rigidbody2D rb;
    Transform sprite;
    Vector2 shootingTriangle = new Vector2(0, 0);

    public GameObject launchBar;
    private LaunchBar activeLaunchBar;

    // Start is called before the first frame update
    void Start()
    {
        isJumping = false;
        rb = GetComponent<Rigidbody2D>();
        sprite = transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) && !isJumping)
        {
            isCharging = true;

            jumpStart = Time.time;
            sprite.localScale = new Vector3(1, 0.5f, 1);
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Vector2 mousePosition = ray.GetPoint(3);
            Vector2 playerPosition = rb.transform.position;
            shootingTriangle = (mousePosition - playerPosition).normalized;

            // Instantiate LaunchBar
            activeLaunchBar = Instantiate(launchBar, (Vector2)this.transform.position + shootingTriangle*2, 
                                            Quaternion.FromToRotation(new Vector2(1,0), shootingTriangle), this.transform)
                              .GetComponent<LaunchBar>();
        }
        if (Input.GetMouseButton(0)) // When holding down button, increase bar
        {
            if (activeLaunchBar != null)
            {
                float percent = (Time.time - jumpStart) / maxJump;
                activeLaunchBar.SetSize(percent);
            }
        }
        if (Input.GetMouseButtonUp(0) && isCharging) // Jump when release
        {
            isCharging = false;
            Jump(Time.time - jumpStart, shootingTriangle);
            if (activeLaunchBar != null)
            {
                Destroy(activeLaunchBar.gameObject);
            }
        }
        if (Input.GetMouseButton(0) && Time.time - jumpStart > maxJump && isCharging) // Jump when held too long
        {
            isCharging = false;
            Jump(Time.time - jumpStart, shootingTriangle);
            if (activeLaunchBar != null)
            {
                Destroy(activeLaunchBar.gameObject);
            }
        }
        

    }

    void Jump(float jumpAmount, Vector2 dirc)
    {
        if (jumpAmount > 2)
        {
            jumpAmount = 2f;
        }
        
        sprite.localScale = new Vector3(1, 1, 1);
        rb.AddForce(dirc * 10 * jumpAmount, ForceMode2D.Impulse);

        /*
        if (Input.GetKey("a") && !Input.GetKey("d"))
        {
            rb.AddForce(new Vector2(1f, 5f),ForceMode2D.Impulse);
            Debug.Log("jump left");
        } else if (!Input.GetKey("a") && Input.GetKey("d"))
        {
            rb.AddForce(Quaternion.Euler(0, 0, -45) * transform.up * jumpAmount * 350);
            Debug.Log("jump right");
        } else
        {
            rb.AddForce(transform.up * jumpAmount * 350);
        }*/

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "ground")
        {
            isJumping = false;
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "ground")
        {
            isJumping = true;
        }
    }
}
