using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBounds : MonoBehaviour
{
    
    public PlayerManager playerManager;

    public void Start()
    {
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        // destroy any objects that leave the world bounds 
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // kill player
            playerManager.Die();
        }
        else
        {
            Destroy(col.gameObject);
        }
    }

}
