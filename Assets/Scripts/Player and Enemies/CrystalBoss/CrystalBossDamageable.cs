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
        GameObject final = Instantiate(finalCrystal, transform.position, Quaternion.identity);
        final.GetComponent<NextLevelCore>().sceneName = "EndingScene";
        final.GetComponent<Animation>().Play("shiftDown");
        Destroy(gameObject);
    }
}
