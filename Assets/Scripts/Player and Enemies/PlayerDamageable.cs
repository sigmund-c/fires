using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageable : Damageable
{
    private Vector3 lastCheckpoint;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        lastCheckpoint = transform.position;
    }


    // Override, will respawn
    protected override void Die()
    {
        Debug.Log(name + " died");
        transform.position = lastCheckpoint;
        RestoreHealth(maxHealth - currHealth);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Checkpoint")
        {
            lastCheckpoint = col.transform.position;
        }
    }
}
