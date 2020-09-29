using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Platform "AI" that moves Left/Right until hits a wall (tag == "Ground"), then "Bounces" to the other direction
public class Platform_Points : MonoBehaviour
{
    public float moveSpeed;
    public List<Transform> movePoints;

    private Queue<Transform> pointsQueue = new Queue<Transform>();
    private bool movingInAscending;

    // Start is called before the first frame update
    void Start()
    {
        // fill initial Queue in ascending order
        movingInAscending = true;
        movePoints.ForEach(pointsQueue.Enqueue);
    }

    // Update is called once per frame
    void Update()
    {
        if (pointsQueue.Count > 0)
        {
            Vector2 target = pointsQueue.Peek().position;
            if (Vector2.Distance(transform.position, target) > 0.05f)
            {
                transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            }
            else
            {
                pointsQueue.Dequeue();
            }
        } else
        {
            refillQueue();
        }

    }

    private void refillQueue()
    {
        if (movingInAscending) // If moving in ascending order, refill in opposite order
        {
            movingInAscending = false;
            for (int i = movePoints.Count - 1; i >= 0; i--)
            {
                pointsQueue.Enqueue(movePoints[i]);
            }
        } else
        {
            movingInAscending = true;
            movePoints.ForEach(pointsQueue.Enqueue);
        }
    }
}
