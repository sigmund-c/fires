using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


// Use for tilemaps where each tile the player touches gets destroyed.
// Might add special effects later.
public class BurnableTile : MonoBehaviour
{
    public GameObject burnEffect;

    private Tilemap tilemap;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        Vector3Int playerPos = tilemap.WorldToCell(player.transform.position);
        if (tilemap.GetTile(playerPos) != null)
        {
            tilemap.SetTile(playerPos, null);
            GameObject.Instantiate(burnEffect, playerPos, Quaternion.identity);
            if (tilemap.GetTile(playerPos) != null)
            {

            }
            // Add burning effects here
        }
    }

    /*
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            tilemap.SetTile(tilemap.WorldToCell(col.transform.position), null);
        }
    }*/
}
