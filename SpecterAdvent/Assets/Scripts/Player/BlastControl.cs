using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; 
using static DamageType; 

public class BlastControl : MonoBehaviour
{

    public ParticleSystem hitEffect; 

    public Transform hitPoint; 
    public Tilemap breakableTilemap; 
    public bool isRight = true; 

    public float damage = 5; 

    void Start()
    {   
        breakableTilemap = GameObject.Find("BreakableGround").GetComponent<Tilemap>();
    }

    public void Shooter(bool isTurned)
    {
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();

        if (isTurned)
        {
            rb2d.velocity = new Vector2(-3,0);
            transform.localScale = new Vector3(-1,1,1);
            isRight = false;
        }
        else
        {
            rb2d.velocity = new Vector2(3,0);
            transform.localScale = new Vector3(1,1,1);
            isRight = true; 
        }

        Destroy(gameObject, 2);
    	
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Collisions with the Enemy 
        if(col.transform.CompareTag("Enemy"))
        {
            col.GetComponent<EnemyController>().TakeDamage(damage, DamageType.Projectile);
            
            StartCoroutine(Hit());
        }
        
        // Collide with breakable walls 
        else if (col.gameObject.CompareTag("Breakable"))
        {
            // break tile infront of blast 
            breakableTilemap.SetTile(breakableTilemap.WorldToCell(hitPoint.position), null);
            
            // get rid of bullet 
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
        // disable bullet
        GetComponent<SpriteRenderer>().enabled = false; // out of sight 
        GetComponent<Collider2D>().enabled = false; // prevent collisions
        GetComponent<Rigidbody2D>().velocity = new Vector3(0,0,0); // stop movement
        GetComponent<Rigidbody2D>().isKinematic = true; // prevent gravity

        // play hit effect
        float duration = hitEffect.main.duration;
        hitEffect.Play();

        // Wait for effect to finish
        yield return new WaitForSeconds(duration + 0.5f);

        // get rid of bullet
        Destroy(gameObject);
    }

    public void SetBulletDamage(float newDmg)
    {
        damage = newDmg;
    }
}
