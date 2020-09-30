using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Patrol : MonoBehaviour
{
    public LayerMask ignoreLayer;
    public float moveSpeed;
    public float maxFallDist = 2f;
    public float wallDetectionDist = 0.1f;

    public Transform groundDetector;

    private bool movingLeft = true;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetector.position, Vector2.down, maxFallDist, ~ignoreLayer);
        if (groundInfo.collider == false) // No ground found
        {
            Rotate();
        } else // Check for obstructions/walls
        {

            RaycastHit2D wallInfo;
            if (movingLeft)
            {
                wallInfo = Physics2D.Raycast(groundDetector.position, Vector2.left, wallDetectionDist, ~ignoreLayer);
            } else
            {
                wallInfo = Physics2D.Raycast(groundDetector.position, Vector2.right, wallDetectionDist, ~ignoreLayer);
            }
            if (wallInfo.collider == true && wallInfo.collider.tag != "Player")
            {
                Debug.LogWarning(wallInfo.collider.name);
                Rotate();
            }
        }
    }

    void Rotate()
    {
        if (movingLeft == true) // Moving Left
        {
            transform.Rotate(0, 180, 0);
            movingLeft = false;
        }
        else // Moving Right
        {
            transform.Rotate(0, 180, 0);
            movingLeft = true;
        }
    }
}
