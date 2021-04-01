using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{

    public float amountOfHealth = 10;
    public PlayerManager playerManager; 

    void Start()
    {
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();            
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // determine if other is player
        if (other.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerManager.AddHealth(amountOfHealth);
            Destroy(gameObject);
        }
    }
}
