    using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MiniCrystalAction
{
    Offense,
    Defense
}

public class MiniCrystal : MonoBehaviour
{
    public float orbitSpeed = 10f;
    public float orbitRadius = 0f;
    private bool isSpinning = false;

    private MiniCrystalAction curState = MiniCrystalAction.Defense;
    private Vector2 parentPos;

    public float changeRadiusSpeed = 3f;
    private Transform spriteTransform;

    private Animator anim;
    public LaserController laser;
    private Damageable damageable;

    // Start is called before the first frame update
    void Start()
    {
        parentPos = transform.parent.position;

        spriteTransform = GetComponentInChildren<SpriteRenderer>().transform;
        anim = GetComponentInChildren<Animator>();
        laser = GetComponentInChildren<LaserController>();
        damageable = GetComponent<Damageable>();
        damageable.immuneTo = Team.Player;
    }

    // Update is called once per frame
    void Update()
    {
        //spriteTransform.rotation = Quaternion.Inverse(transform.rotation);
        /*
        switch (curState)
        {
            case MiniCrystalAction.Offense:
                HandleOffenseState();
                break;

            case MiniCrystalAction.Defense:
                HandleDefenseState();
                break;
        }*/


        if (isSpinning)
        {
            transform.Rotate(0, 0, orbitSpeed * Time.deltaTime);
        }
    }

    public void VulnerableFor(float seconds)
    {
        StartCoroutine(Vulnerable(seconds));
    }

    IEnumerator Vulnerable(float seconds)
    {
        SetVulnerable();

        yield return new WaitForSeconds(seconds);

        SetInvulnerable();
    }

    public void setRadius(float newRadius)
    {
        orbitRadius = newRadius;
        StartCoroutine(ChangeRadius());
    }

    public void ShootLaser(float seconds)
    {
        laser.ShootLaser(Vector3.up, seconds);
    }

    IEnumerator ChangeRadius()
    {
        float distance = Vector2.Distance(transform.position, parentPos);
        while (Mathf.Abs(distance - orbitRadius) > 0.05f)
        {
            //Debug.Log(distance);
            if (distance > orbitRadius)
                transform.position = Vector2.MoveTowards(transform.position, parentPos, changeRadiusSpeed * Time.deltaTime);
            else
                transform.position = Vector2.MoveTowards(transform.position, parentPos, -changeRadiusSpeed * Time.deltaTime);

            distance = Vector2.Distance(transform.position, parentPos);
            yield return null;
        }

    }

    public void SetVulnerable()
    {
        anim.SetTrigger("Vulnerable");
        StartCoroutine(VulnerableAfterAnim());
    }

    IEnumerator VulnerableAfterAnim()
    {
        yield return new WaitForSeconds(1);
        damageable.immuneTo = Team.None;
    }

    public void SetInvulnerable()
    {
        anim.SetTrigger("Invulnerable");
        StartCoroutine(InvulnerableAfterAnim());
    }

    IEnumerator InvulnerableAfterAnim()
    {
        yield return new WaitForSeconds(2);
        damageable.immuneTo = Team.Player;
    }
}
