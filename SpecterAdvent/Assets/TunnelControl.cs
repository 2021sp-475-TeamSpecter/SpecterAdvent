using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelControl : MonoBehaviour
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
    public Transform enemyGFX;  
    public GameObject bulletPrefab;
    public float bulletSpeed = 1; 
    public Transform firePoint1;
    public float attackSpeed = 1; 
    LayerMask mask; 
    float lastAttack; 
    private int rand;

    //public ParticleSystem hitEffect; 

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
        	/*rand = Random.Range(0,2);

        	if (rand == 0)
        	{

        	}
        	else
        	{

        	}*/
        	            // stop moving 
            //enemyPatrolMechanics.canMove = false;
            enemyPatrol.canMove = false; 

            animator.Play("ShootTunnel");

            // face player 
            // left 
            if (directionLeft.x >= 0.05f)
            {
                enemyGFX.localScale = new Vector3(-Mathf.Abs(enemyGFX.localScale.x), enemyGFX.localScale.y, enemyGFX.localScale.z);
            }
            else if (directionLeft.x <= -0.05f)
            {
                enemyGFX.localScale = new Vector3(Mathf.Abs(enemyGFX.localScale.x), enemyGFX.localScale.y, enemyGFX.localScale.z);
            }  

            // if not on attack cooldown 
            if (Time.time > attackSpeed + lastAttack)
            {
                    Shoot(directionLeft, firePoint1);

                lastAttack = Time.time;
            }
        }
                    // cannot see player 
        else
        {
            // allow moving 
            //enemyPatrolMechanics.canMove = true;
            enemyPatrol.canMove = true; 

            animator.SetTrigger("Idle");
        }
/*
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
                //hitEffect.Play();
                // hurt player 
                playerManager.TakeDamage(damage, DamageType.Melee);
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
            animator.Play("ChargeTunnel"); 

            // dash
            Vector3 force = directionLeft * dashSpeed; 
            rigidbody.AddForce(force);

        }
        else if (isDashing || isSlowingDown)
        {
            // Debug.Log("Slowing down");
            isDashing = false; 
            isSlowingDown = true;
            animator.Play("IdleTunnel"); 

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
            animator.Play("IdleTunnel"); 
        }*/

    }
    void Shoot(Vector3 directionToPlayer, Transform firePoint)
    {
        // Spawn missile
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<TunnelShot>().SetDamage(damage);
        // orient missile
        if (directionToPlayer.x >= 0.05f)
        {
            bullet.transform.localScale = new Vector3(-Mathf.Abs(bullet.transform.localScale.x), bullet.transform.localScale.y, bullet.transform.localScale.z);
        }
        else if (directionToPlayer.x <= -0.05f)
        {
            bullet.transform.localScale = new Vector3(Mathf.Abs(bullet.transform.localScale.x), bullet.transform.localScale.y, bullet.transform.localScale.z);
        }  
        // Send missile
        Rigidbody2D bulletrigid = bullet.GetComponent<Rigidbody2D>();
        if (directionToPlayer.x >= 0.05f)
        {
            bulletrigid.AddForce(firePoint.right * bulletSpeed, ForceMode2D.Impulse);
        }
        else if (directionToPlayer.x <= -0.05f)
        {
            bulletrigid.AddForce(firePoint.right * -bulletSpeed, ForceMode2D.Impulse);
        }  
        // Play sound
        // shootSFX.Play();
    }

    // Draws a gizmo for the sight range of the turret
    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);    
    }
}