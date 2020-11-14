using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_SentryHead : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float timeBetweenShots = 1f;
    public bool isUpsideDown = false;
    public float animFireDelay;
    public float projectileVelocity;

    private float nextShot;

    private Transform target = null;

    private Animator headAnimator;
    private Animator bodyAnimator;

    private void Start()
    {
        nextShot = timeBetweenShots;
        headAnimator = transform.GetChild(0).GetComponent<Animator>();
        bodyAnimator = transform.parent.GetChild(0).GetComponent<Animator>();
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
        headAnimator.SetTrigger("Shoot");
        bodyAnimator.SetTrigger("Shoot");
        StartCoroutine("SpawnProjectile");
        
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
            transform.up = Vector3.up;
            StopCoroutine("SpawnProjectile");
        }
    }

    IEnumerator SpawnProjectile()
    {
        yield return new WaitForSeconds(animFireDelay);
        Vector2 aimDirection = target.position - transform.position;
        GameObject projectileInst = Instantiate(projectilePrefab, (Vector2)transform.position + aimDirection.normalized * 1.5f, Quaternion.FromToRotation(Vector3.up, aimDirection));
        projectileInst.GetComponent<Projectile>().velocity = projectileVelocity;
    }
}
