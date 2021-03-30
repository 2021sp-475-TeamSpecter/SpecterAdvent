using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastControl : MonoBehaviour
{

    public float damage = 5; 

    public void Shooter(bool isTurned)
    {
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();

        if (isTurned)
        {
            rb2d.velocity = new Vector2(-3,0);
            transform.localScale = new Vector3(-1,1,1);
        }
        else
        {
            rb2d.velocity = new Vector2(3,0);
            transform.localScale = new Vector3(1,1,1);
        }

        Destroy(gameObject, 2);
    	
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Collisions with the Enemy 
        if(col.transform.CompareTag("Enemy"))
        {
            col.GetComponent<EnemyController>().TakeDamage(damage);
            //GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            //Destroy(effect, 1f);
            Destroy(gameObject);
        }
        
        // Collide with walls and obstacles
        else if (col.gameObject.layer == LayerMask.NameToLayer("Ground")) 
        {
            //GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            //Destroy(effect, 1f);
            Destroy(gameObject);
        }
    }


    public void SetBulletDamage(float newDmg)
    {
        damage = newDmg;
    }
}
