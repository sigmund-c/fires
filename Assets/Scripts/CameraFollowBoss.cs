using UnityEngine;
using System.Collections;

public class CameraFollowBoss : CameraFollow
{
    private Vector3 bossPos;

    new void Start()
    {
        base.Start();

        if (GameObject.Find("BossSnake") != null)
        {
            bossPos = GameObject.Find("BossSnake").transform.position;
        } else if (GameObject.Find("BossCrystal") != null)
        {
            bossPos = GameObject.Find("BossCrystal").transform.position;
        }
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (shakeDuration <= 0)
        {
            DecideTarget();
            Vector3 mid = bossPos + target / 2;
            transform.position = Vector3.MoveTowards(transform.position, mid + new Vector3(0, 0, -10f), 1f);
            
        }
    }
 
}
