using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpMovement : MonoBehaviour
{
    float jumpStart;
    float jumpCharge;

    bool isJumping;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        isJumping = false;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("w") && !isJumping)
        {
            jumpStart = Time.time;
            transform.localScale = new Vector3(1, 0.5f, 1);
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
        transform.localScale = new Vector3(1, 1, 1);
        if (Input.GetKey("d") && !Input.GetKey("a"))
        {
            rb.AddForce(Quaternion.Euler(0, 0, 30) * transform.up * jumpAmount * 350);
        } else if (!Input.GetKey("d") && Input.GetKey("a"))
        {
            rb.AddForce(Quaternion.Euler(0, 0, -30) * transform.up * jumpAmount * 350);
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
