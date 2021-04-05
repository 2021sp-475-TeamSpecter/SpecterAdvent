using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelDash : MonoBehaviour
{

    public float damage = 10;

    public float dashSpeed = 500; 
    public float sightRange = 3;
    public bool isDashing = false;
    public bool isSlowingDown = false; 
    public float slowedSpeed = 10;

    public float lastHitTime = 0;
    public float cooldownTime = 2;

    public Transform playerDetector; 
    public float playerHitDetectionDist = 0.3f;

    public PlayerManager playerManager;
    public EnemyPatrol enemyPatrol; 
    public Rigidbody2D rigidbody;
    public Animator animator; 
    LayerMask mask; 
    float lastAttack; 

    public ParticleSystem hitEffect; 

    void Start()
    {
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        enemyPatrol = GetComponent<EnemyPatrol>();
        rigidbody = GetComponent<Rigidbody2D>();
        // Enemy can only see players and obstacles
        // Things like bullets/enemies are ignored 
        mask = LayerMask.GetMask("Player") | LayerMask.GetMask("Ground");
    }

    void FixedUpdate()
    {
        // determine if enemy can see player
        Vector3 directionLeft = (playerDetector.position - transform.position).normalized; 

        // Determine if enemy can see player or obstacle 
        RaycastHit2D hit = Physics2D.Raycast(
            playerDetector.position, 
            directionLeft, 
            sightRange, 
            mask
        );

        if (hit.collider != null 
            && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player")
            && Time.time > lastHitTime + cooldownTime)
        {

            // check if hit 
            RaycastHit2D playerHit = Physics2D.Raycast(
                playerDetector.position,
                directionLeft,
                playerHitDetectionDist
            );

            if (playerHit.collider != null 
                && playerHit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                // Play hit effect
                hitEffect.Play();
                // hurt player 
                playerManager.TakeDamage(damage, DamageType.Melee);
                animator.Play("WheelStation");
                // stop moving 
                rigidbody.velocity = new Vector3(0,0,0);
                // go on cooldown 
                isDashing = false; 
                lastHitTime = Time.time; 
                return; 
            }

            // Debug.Log("Charging!");
            isDashing = true;
            isSlowingDown = false; 
            enemyPatrol.canMove = false; 

            // dash
            Vector3 force = directionLeft * dashSpeed; 
            rigidbody.AddForce(force);
            animator.Play("WheelRoll");

        }
        else if (isDashing || isSlowingDown)
        {
            // Debug.Log("Slowing down");
            isDashing = false; 
            isSlowingDown = true; 

            // check if enemy slowed down enough
            if (rigidbody.velocity.magnitude <= slowedSpeed)
            {
                isSlowingDown = false; 
            }

        }
        // return to patrol if not on cooldown
        else if (Time.time > lastHitTime + cooldownTime)
        {
            // resume patrol
            enemyPatrol.canMove = true;
            animator.Play("WheelWalk"); 
        }

    }
}
