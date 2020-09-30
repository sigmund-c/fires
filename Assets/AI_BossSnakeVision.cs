using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_BossSnakeVision : MonoBehaviour
{
    private AI_BossSnake ai;

    // Start is called before the first frame update
    void Start()
    {
        ai = transform.GetComponentInParent<AI_BossSnake>();
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            ai.target = col.transform;
            ai.StartAI();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            ai.target = null;
        }
    }
}
