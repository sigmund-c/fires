using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Platform "AI" that moves Left/Right until hits a wall (tag == "Ground"), then "Bounces" to the other direction
public class Platform_LR_Bounce : MonoBehaviour
{
    public float moveSpeed;

    private bool movingLeft = true;

    public LayerMask ignoreLayer;
    private Transform groundDetectorLeft;
    private Transform groundDetectorRight;

    // Start is called before the first frame update
    void Start()
    {
        groundDetectorLeft = transform.Find("GroundDetectorLeft");
        groundDetectorRight = transform.Find("GroundDetectorRight");
    }

    // Update is called once per frame
    void Update()
    {
        if (movingLeft)
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        } else
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
        }

        RaycastHit2D wallInfo;
        if (movingLeft)
        {
            wallInfo = Physics2D.Raycast(groundDetectorLeft.position, Vector2.left, 0.1f, ~ignoreLayer);
        }
        else
        {
            wallInfo = Physics2D.Raycast(groundDetectorRight.position, Vector2.right, 0.1f, ~ignoreLayer);
        }
        if (wallInfo.collider == true && wallInfo.collider.tag != "Player" && wallInfo.collider.tag != "PlayerComponent")
        {
            movingLeft = !movingLeft;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // "Stick" player that is on the platform, moving along the platform
        if (collision.gameObject.tag == "Player")
        {
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.collider.transform.SetParent(null);
        }
    }
    
}
