using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEnemy : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawnLocation;
    public float bulletSpeed = 1; 
    public float attackSpeed = 1;
    public float attackDamage = 10;  
    public float sightRange = 1;

    public EnemyPatrol enemyPatrol; 
    public PlayerManager playerManager;
    public Transform enemyGFX; 
    LayerMask mask;
    float lastAttack; 

    public Animator animator; 

    void Start()
    {
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        mask = LayerMask.GetMask("Player") | LayerMask.GetMask("Ground");
        enemyPatrol = GetComponent<EnemyPatrol>();
    }

    void Update()
    {
        Vector3 directionToPlayer = (playerManager.GetPosition() - transform.position).normalized; 

        // Determine if enemy can see player or obstacle 
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position, 
            directionToPlayer, 
            sightRange, 
            mask
        );

        // if enemy can see player
        if (hit.collider != null
            && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // stop patrol
            enemyPatrol.canMove = false; 
            animator.Play("BallEnemyIdle");

            // face player 
            // left 
            if (directionToPlayer.x >= 0.05f)
            {
                enemyGFX.localScale = new Vector3(-Mathf.Abs(enemyGFX.localScale.x), enemyGFX.localScale.y, enemyGFX.localScale.z);
            }
            else if (directionToPlayer.x <= -0.05f)
            {
                enemyGFX.localScale = new Vector3(Mathf.Abs(enemyGFX.localScale.x), enemyGFX.localScale.y, enemyGFX.localScale.z);
            }  

            // if not on attack cooldown
            if (Time.time > attackSpeed + lastAttack)
            {
                // attack 
                Shoot();
                lastAttack = Time.time;
            }
        }
        // enemy cannot see player
        else
        {
            // resume patrol
            enemyPatrol.canMove = true; 
            animator.Play("BallEnemyWalk");
        }
    }

    void Shoot()
    {
        // Spawn bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnLocation.position, bulletSpawnLocation.rotation);
        bullet.GetComponent<EnemyBullet>().setBulletDamage(attackDamage);
        // Send bullet
        Rigidbody2D bulletrigid = bullet.GetComponent<Rigidbody2D>();
        bulletrigid.AddForce(bulletSpawnLocation.up * bulletSpeed, ForceMode2D.Impulse);
        // Play sound
        // shootSFX.Play();
    }

    // Draws a gizmo for the sight range of the turret
    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange+1);    
    }
}
