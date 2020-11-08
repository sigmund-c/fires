using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicle_CrystalBoss : MonoBehaviour
{
    new Collider2D collider;

    void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Damageable dmg = col.gameObject.GetComponent<Damageable>();
        if (dmg != null)
        {
            dmg.TriggerCollision(collider);
        }
    }
}
