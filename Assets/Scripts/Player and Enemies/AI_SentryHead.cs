using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_SentryHead : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float timeBetweenShots = 1f;
    public bool isUpsideDown = false;
    private float nextShot;

    private Transform target = null;

    private void Start()
    {
        nextShot = timeBetweenShots;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // 2D equivalent of tranform.LookAt()
            if (!isUpsideDown)
            {
                transform.up = target.position - transform.position;
            } else
            {
                transform.up = transform.position - target.position;
            }

            if (nextShot > 0)
            {
                nextShot -= Time.deltaTime;
            } else
            {
                ShootProjectile();
                nextShot = timeBetweenShots;
            }
        }
    }

    private void ShootProjectile()
    {
        Vector2 aimDirection = target.position - transform.position;
        GameObject projectileInst = Instantiate(projectilePrefab, (Vector2)transform.position + aimDirection.normalized * 1.5f, Quaternion.FromToRotation(Vector3.up, aimDirection));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            target = col.transform;
            nextShot = timeBetweenShots; //first shot is slower
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            target = null;
        }
    }
}
