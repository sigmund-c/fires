using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool isActivated = false;
    public Material activatedMat;

    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!isActivated)
        {
            isActivated = true;
            ActivateSprite();
        }
    }

    private void ActivateSprite()
    {
        Debug.Log("actives");
        sr.color = new Color(1, 0.5f, 0, 1);
        sr.material = activatedMat;
    }
}
