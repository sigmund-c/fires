using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CrystalBossManager : MonoBehaviour
{
    AI_CrystalBoss boss;
    public TilemapRenderer tilemap;
    public TilemapCollider2D tilemapColl;
    private bool firstTrigger = false;
    public HealthContainer bossHealth;

    void Start()
    {
        boss = GetComponentInChildren<AI_CrystalBoss>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!firstTrigger)
        {
            firstTrigger = true;
            tilemap.enabled = true;
            tilemapColl.enabled = true;
            boss.StartBoss();
            GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentAudio>().ChangeMusic(1);
            bossHealth.gameObject.SetActive(true);
        }
        
    }
}
