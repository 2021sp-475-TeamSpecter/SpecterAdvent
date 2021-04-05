using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelShot : MonoBehaviour
{
    public ParticleSystem hitEffect;
    public Collider2D collider; 

    private float dmg;

    private float timeCreated;
    private float maxLifetime = 5f;
    
    private bool isDying = false; 

    public PlayerManager playerManager; 

    void Start()
    {
        collider = GetComponent<Collider2D>();
        timeCreated = Time.time;
        maxLifetime += timeCreated;
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
    }

    void Update()
    {
        if(Time.time > maxLifetime && !isDying)
            StartCoroutine(Hit());
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Collisions with the Player 
        if(col.transform.CompareTag("Player"))
        {
            //playerManager.TakeDamage(dmg, DamageType.Projectile);
            playerManager.TakeDamage(dmg, DamageType.Projectile);
            StartCoroutine(Hit());
        }
        
        // Collide with walls and obstacles
        else if (col.gameObject.layer == LayerMask.NameToLayer("Ground")) 
        {
            StartCoroutine(Hit());
        }
    }

    IEnumerator Hit()
    {
        isDying = true;

        // disable bullet
        GetComponent<SpriteRenderer>().enabled = false; // out of sight 
        GetComponent<Collider2D>().enabled = false; // prevent collisions
        GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0);
        GetComponent<Rigidbody2D>().isKinematic = true; // prevent gravity

        // hit effect
        hitEffect.Play();

        // Wait for effect to finish
        yield return new WaitForSeconds(1f);

        // get rid of bullet
        Destroy(gameObject);
    }

    public void SetDamage(float newDmg)
    {
        dmg = newDmg;
    }
}
