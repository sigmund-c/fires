using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageable : Damageable
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        // lastCheckpoint = transform.position;
    }


    // Override, will respawn
    public override void Die()
    {
        Debug.Log(name + " died");
        if (PersistentManager.instance != null)
            PersistentManager.Reload();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Checkpoint")
        {
            PersistentManager.checkpoint = col.transform.position;
            print("checkpoint saved at : " + PersistentManager.checkpoint);
        }
    }
}
