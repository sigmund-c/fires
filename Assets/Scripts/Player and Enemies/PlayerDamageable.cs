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
    public override void Die()
    {
        Debug.Log(name + " died");
        // transform.position = lastCheckpoint;
        if (PersistentManager.instance != null)
            PersistentManager.Reload();
        else
        {
            transform.position = lastCheckpoint;
            Camera.main.transform.position = lastCheckpoint;
            RestoreHealth(maxHealth - currHealth);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Checkpoint")
        {
            PersistentManager.checkpoint = col.transform.position;
            print("checkpoint saved at : " + PersistentManager.checkpoint);
            lastCheckpoint = col.transform.position;
        }
    }
}
