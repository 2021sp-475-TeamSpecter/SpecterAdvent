using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DamageType;

public class Bomb : MonoBehaviour
{
   
    public float damage = 20; 
    public float explosionRadius = 2; 

    public float timeCreated;
    public float maxLifetime = 2f;

    public PlayerManager playerManager; 

    public Rigidbody2D rigidbody; 
    public GameObject thrower; 

    public ParticleSystem explosionEffect; 

    public bool hasExploded = false;

    void Start()
    {
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // wait for bomb timer to expire
        if(Time.time > maxLifetime + timeCreated && !hasExploded)
        {
            StartCoroutine(Explode());
            hasExploded = true; 
        }
    }

    IEnumerator Explode()
    {
        // get all in explosion radius 
        // it seems like the layer mask does not work 
        // so at the moment this grabs all things with colliders 
        // within the explosion radius 
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            transform.position, 
            explosionRadius
        );

        // damage entities in the range
        foreach (var col in colliders)
        {
            // vary damage relative to how close the player is 
            float dist = Vector3.Distance(col.gameObject.transform.position, transform.position);
            float adjustedDamage = Mathf.Max(damage - ((dist / explosionRadius) * damage), 0);
            // if target is player
            if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                playerManager.TakeDamage(adjustedDamage, DamageType.Explosive);
            }
            // if target is enemy
            else if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                col.gameObject.GetComponent<EnemyController>().TakeDamage(adjustedDamage, DamageType.Explosive);
            }
        }

        // hide bomb graphic
        GetComponent<SpriteRenderer>().enabled = false; // out of sight 
        GetComponent<Collider2D>().enabled = false; // prevent collisions
        GetComponent<Rigidbody2D>().isKinematic = true; // prevent gravity

        // explode
        explosionEffect.Play();

        // wait for explosion to end
        yield return new WaitForSeconds(1.5f);
        
        // get rid of bomb 
        Destroy(gameObject);
    }

    public void SendBomb(GameObject thrower_, Vector3 velocity_, float timer, float damage_)
    {
        thrower = thrower_;
        damage = damage_; 
        // set duration 
        timeCreated = Time.time;
        maxLifetime = timer;
        // apply to velocity 
        rigidbody.velocity = velocity_; 
    }

    // Draws a gizmo for the explosion radius
    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);    
    }
}
