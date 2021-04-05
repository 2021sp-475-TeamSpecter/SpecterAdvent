using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Door : MonoBehaviour
{
    public Transform[] doorPositions;
    public Tilemap ground; 

    public GameObject enemies; 

    void Start()
    {
        ground = GameObject.Find("Ground").GetComponent<Tilemap>();
        enemies = GameObject.Find("Enemies");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // if other is player and all enemies are dead 
        if (col.gameObject.layer == LayerMask.NameToLayer("Player")
            && enemies.transform.childCount == 0)
        {
            // remove door 
            foreach (var pos in doorPositions)
            { 
                ground.SetTile(ground.WorldToCell(pos.position), null);
            }
        }
    }
}
