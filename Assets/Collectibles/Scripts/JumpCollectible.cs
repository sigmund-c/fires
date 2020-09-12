using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCollectible : Collectible
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    new void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller != null)
        {
            controller.BoostMovement();
        }

        base.OnTriggerEnter2D(other);
    }
}
