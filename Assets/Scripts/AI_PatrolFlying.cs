﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Same as AI_Patrol, but doesnt check for ground tiles
public class AI_PatrolFlying : MonoBehaviour
{
    public float moveSpeed;
    public float wallDetectionDist = 0.1f;

    public Transform groundDetector;

    private bool movingLeft = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        
        RaycastHit2D wallInfo = Physics2D.Raycast(groundDetector.position, Vector2.left, wallDetectionDist);
        if (wallInfo.collider == true && wallInfo.collider.gameObject.tag == "Ground")
        {
            Rotate();
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
