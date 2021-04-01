using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DamageType;

public class MeleeDamage : MonoBehaviour
{

    public PlayerController2D controller; 
    float damage; 

    void Start ()
    {
        damage = controller.damage; 
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Collisions with the Enemy 
        if(col.transform.CompareTag("Enemy"))
        {
            col.GetComponent<EnemyController>().TakeDamage(damage, DamageType.Melee);
            //GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            //Destroy(effect, 1f);
        }
        
    }

}
