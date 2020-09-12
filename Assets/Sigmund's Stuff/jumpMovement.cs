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
        if (Input.GetKeyDown("w") && !isJumping)
        {
            jumpStart = Time.time;
            sprite.localScale = new Vector3(1, 0.5f, 1);
        }
        if (Input.GetKeyUp("w") && !isJumping)
        {
            Jump(Time.time - jumpStart);
        }
        if (Input.GetKey("w") && Time.time - jumpStart > 2f && !isJumping)
        {
            Jump(Time.time - jumpStart);
        }
    }

    void Jump(float jumpAmount)
    {
        if (jumpAmount > 2)
        {
            jumpAmount = 2f;
        }


        isJumping = true;
        sprite.localScale = new Vector3(1, 1, 1);
        if (Input.GetKey("a") && !Input.GetKey("d"))
        {
            rb.AddForce(Quaternion.Euler(0, 0, 30) * transform.up * jumpAmount * 350);
            Debug.Log("jump left");
        } else if (!Input.GetKey("a") && Input.GetKey("d"))
        {
            rb.AddForce(Quaternion.Euler(0, 0, -30) * transform.up * jumpAmount * 350);
            Debug.Log("jump right");
        } else
        {
            rb.AddForce(transform.up * jumpAmount * 350);
        }

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "ground")
        {
            isJumping = false;
        }
    }
}
