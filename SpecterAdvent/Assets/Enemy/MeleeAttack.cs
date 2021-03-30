using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{

    public float attackDamage = 5;
    public float attackDistance = 1; 
    public float attackSpeed = 2;
    private float lastAttackTime; 

    public PlayerManager playerManager; 

    public Collider2D collider; 

    void Start()
    {
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();

        collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        // determine if enemy is touching player and not on cooldown
        bool isTouching = collider.IsTouching(playerManager.GetCollider());
        if (isTouching && Time.time > lastAttackTime + attackSpeed)
        {
            // Attack player 
            playerManager.TakeDamage(attackDamage);
            // Setup cooldown
            lastAttackTime = Time.time;
        }
    }

}
