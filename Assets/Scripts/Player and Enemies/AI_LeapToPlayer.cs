using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_LeapToPlayer : MonoBehaviour
{
    public float jumpStrength = 10f;
    private float angle = 63.0f;
    private Vector2 jumpAngle;

    private Animator anim;
    private Rigidbody2D rb;
    private Transform target = null;
    public bool isJumping = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        jumpAngle = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null && !isJumping)
        {
            anim.SetTrigger("Attack");
            UpdateSpriteRotation();
            if (target.transform.position.x - this.transform.position.x > 0) // player is to the right, jump right
            {
                rb.AddForce(jumpAngle * jumpStrength, ForceMode2D.Impulse);
            } else // jump left
            {
                rb.AddForce(jumpAngle * jumpStrength * new Vector2(-1, 1), ForceMode2D.Impulse);
            }
            isJumping = true;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (isJumping)
            {
                anim.SetTrigger("HitGround");
            }

            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0f;
            isJumping = false;
        }
    }
    

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            target = col.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            Debug.LogWarning("Ecit");
            target = null;
        }
    }


    private void UpdateSpriteRotation()
    {
        if (Vector2.Angle(Vector2.left, target.position - this.transform.position) < 90) // moving left
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }
        else // moving right
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
