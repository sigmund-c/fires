using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Platform "AI" that moves Left/Right until hits a wall (tag == "Ground"), then "Bounces" to the other direction
public class Platform_LR_Bounce : MonoBehaviour
{
    public float moveSpeed;

    private bool movingLeft = true;

    // Start is called before the first frame update
    void Start()
    {

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

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // "Stick" player that is on the platform, moving along the platform
        if (collision.gameObject.tag == "Player")
        {
            collision.collider.transform.SetParent(transform);
        } else if (collision.gameObject.tag == "Ground")
        {
            movingLeft = !movingLeft;
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
