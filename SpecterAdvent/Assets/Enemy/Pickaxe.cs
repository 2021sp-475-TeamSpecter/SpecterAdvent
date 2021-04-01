using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DamageType;

public class Pickaxe : MonoBehaviour
{

    public float damage = 5; 

    public float timeCreated;
    public float maxLifetime = 2f;
    public int numHits = 0; 

    public PlayerManager playerManager; 

    public Rigidbody2D rigidbody; 
    public GameObject thrower; 
    public Vector3 sendingVelocity;
    public Vector3 returningVelocity;
    public Collider2D collider; 

    void Start()
    {
        collider = GetComponent<Collider2D>();
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // return pickaxe if it doesnt hit anything 
        if(Time.time > (maxLifetime/3) + timeCreated && numHits == 0)
        {
            SendBack();
            numHits = 1; 
        }
        // pickaxe does not reach thrower
        else if(Time.time > maxLifetime + timeCreated + 0.25)
        {
            ReturnPickaxe();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Collisions with the Player 
        if(col.transform.CompareTag("Player"))
        {
            playerManager.TakeDamage(damage, DamageType.Projectile);
            SendBack();
        }
        
        // Collide with walls and obstacles
        else if (col.gameObject.layer == LayerMask.NameToLayer("Ground")) 
        {
            SendBack();
        }

        // Returned 
        else if (GameObject.ReferenceEquals(col.gameObject, thrower)) 
        {
            //GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            //Destroy(effect, 1f);
            ReturnPickaxe();
        }
    }

    public void ReturnPickaxe()
    {
        Destroy(gameObject);
        // make sure thrower didn't die while pickaxe was away 
        if (thrower != null)
            thrower.GetComponent<PickaxeThrow>().ReturnPickaxe();
    }

    public void SendBack()
    {
        if (thrower == null)
            ReturnPickaxe();
        // travel back to thrower
        // thrower should be in the same place 
        rigidbody.velocity = returningVelocity; 
    }

    public void SendPickaxe(GameObject thrower_, Vector3 velocity_, float duration, float damage_)
    {
        thrower = thrower_;
        damage = damage_; 
        // set duration 
        timeCreated = Time.time;
        maxLifetime = duration;
        // send pickaxe 
        sendingVelocity = velocity_;
        returningVelocity = -velocity_;
        // apply to velocity 
        rigidbody.velocity = sendingVelocity; 
    }
}
