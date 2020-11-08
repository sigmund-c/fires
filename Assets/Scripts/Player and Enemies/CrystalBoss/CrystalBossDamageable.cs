using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalBossDamageable : Damageable
{
    public GameObject finalCrystal;

    override public void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        healthContainer.gameObject.SetActive(false);

        GameObject.FindGameObjectWithTag("Persistent").GetComponent<PersistentAudio>().ChangeMusic(0);
        Instantiate(finalCrystal, transform.position, Quaternion.identity).GetComponent<NextLevelCore>().sceneName = "MenuScene";
        Destroy(gameObject);
    }
}
