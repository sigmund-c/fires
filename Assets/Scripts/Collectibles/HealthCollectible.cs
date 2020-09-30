using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : Collectible
{
    public int HealAmount = 10;
    new void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null)
        {
            controller.RestoreHealth(HealAmount);
        }

        base.OnTriggerEnter2D(other);
    }
}
