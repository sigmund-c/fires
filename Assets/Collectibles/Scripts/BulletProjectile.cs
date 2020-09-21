using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : Projectile
{
    public void Launch(Vector2 direction, float force)
    {
        this.rigidbody.AddForce(direction * force);
    }
}
