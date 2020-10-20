using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Same as AI_Patrol, but doesnt check for ground tiles
public class AI_PointsFlying : MonoBehaviour
{
    public float moveSpeed = 5f;
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
                if (pointsQueue.Count != 0)
                {
                    UpdateSpriteRotation();
                }
            }
        }
        else
        {
            refillQueue();
            UpdateSpriteRotation();
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
        }
        else
        {
            movingInAscending = true;
            movePoints.ForEach(pointsQueue.Enqueue);
        }
    }

    private void UpdateSpriteRotation()
    {
        Vector2 target = pointsQueue.Peek().position;
        if (Vector2.Angle(Vector2.left, target - (Vector2)this.transform.position) < 90) // moving left
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        } else // moving right
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
