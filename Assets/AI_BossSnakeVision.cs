using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_BossSnakeVision : MonoBehaviour
{
    private AI_BossSnake ai;
    private bool triggered = false;

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

            if (!triggered)
            {
                triggered = true;

                ai.StartAI();
                GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentAudio>().ChangeMusic(1);
            }
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
