using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damaging : MonoBehaviour
{
    public Team team;
    public int damage = 5;
    public float knockbackAmount = 3;


    public void OnTriggerEnter2D(Collider2D col)
    {
        Damageable damageable = col.gameObject.GetComponent<Damageable>();

        if (damageable != null) {
            damageable.TakeHit(this);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        Damageable damageable = col.gameObject.GetComponent<Damageable>();

        if (damageable != null)
        {
            damageable.TakeHit(this);
        }
    }
}
