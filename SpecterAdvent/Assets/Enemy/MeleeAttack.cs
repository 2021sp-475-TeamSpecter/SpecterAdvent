using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{

    public float attackDamage = 5;
    public float attackDistance = 1; 
    public float attackSpeed = 2;
    private float lastAttackTime; 

    public GameObject player; 
    public Collider2D playerCollider;
    public PlayerController2D playerController; 

    public Collider2D collider; 

    void Start()
    {
        player = GameObject.Find("Player");
        playerCollider = player.GetComponent<Collider2D>();
        playerController = player.GetComponent<PlayerController2D>();

        collider = GetComponent<Collider2D>();
    }

    void Update()
    {
        // determine if enemy is touching player and not on cooldown
        bool isTouching = collider.IsTouching(playerCollider);
        if (isTouching && Time.time > lastAttackTime + attackSpeed)
        {
            // Attack player 
            playerController.TakeDamage(attackDamage);
            // Setup cooldown
            lastAttackTime = Time.time;
        }
    }

}
