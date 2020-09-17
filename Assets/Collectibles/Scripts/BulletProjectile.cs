using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : Projectile
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Launch(Vector2 direction, float force)
    {
        this.rigidbody.AddForce(direction * force);
    }
}
