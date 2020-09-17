using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpMovement : MonoBehaviour
{
    float jumpStart;
    float jumpCharge;

    bool isJumping;

    Rigidbody2D rb;
    Transform sprite;
    Vector2 shootingTriangle = new Vector2(0, 0);

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
            jumpStart = Time.time;
            sprite.localScale = new Vector3(1, 0.5f, 1);
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Vector2 mousePosition = ray.GetPoint(3);
            Vector2 playerPosition = rb.transform.position;
            shootingTriangle = (mousePosition - playerPosition).normalized;
        }
        if (Input.GetMouseButtonUp(0) && !isJumping)
        {
            Jump(Time.time - jumpStart, shootingTriangle);
        }
        if (Input.GetMouseButton(0) && Time.time - jumpStart > 2f && !isJumping)
        {
            Jump(Time.time - jumpStart, shootingTriangle);
        }

    }

    void Jump(float jumpAmount, Vector2 dirc)
    {
        if (jumpAmount > 2)
        {
            jumpAmount = 2f;
        }

        isJumping = true;
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
}
