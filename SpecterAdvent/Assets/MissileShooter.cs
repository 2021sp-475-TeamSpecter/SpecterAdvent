using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileShooter : MonoBehaviour
{
    public float damage = 5; 
    public float sightRange = 1; 
    public float attackSpeed = 1;  
    float lastAttack; 
    public Transform firePoint1; 
    public Transform firePoint2; 
    public GameObject bulletPrefab;
    public float bulletSpeed = 1; 

    public PlayerManager playerManager; 
    public EnemyPatrol enemyPatrolMechanics; 
    public Transform enemyGFX; 
    public Animator animator;
    
    bool topFired = false; 

    LayerMask mask; 

    void Start()
    {
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        enemyPatrolMechanics = GetComponent<EnemyPatrol>();
        // Enemy can only see players and obstacles
        // Things like bullets/enemies are ignored 
        mask = LayerMask.GetMask("Player") | LayerMask.GetMask("Ground");
        lastAttack = Time.time; 
    }

    void Update()
    {
        // only shoot if player is in range
        Vector3 directionToPlayer = playerManager.GetPosition() - transform.position; 

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

            // stop moving 
            enemyPatrolMechanics.canMove = false; 

            animator.SetTrigger("Shoot");

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
                if (topFired)
                {
                    Shoot(directionToPlayer, firePoint2);
                    topFired = false;
                }
                else
                {
                    Shoot(directionToPlayer, firePoint1);
                    topFired = true; 
                }
                lastAttack = Time.time;
            }

        }
        // cannot see player 
        else
        {
            // allow moving 
            enemyPatrolMechanics.canMove = true; 

            animator.SetTrigger("Idle");
        }
    }

    void Shoot(Vector3 directionToPlayer, Transform firePoint)
    {
        // Spawn missile
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Missile>().SetDamage(damage);
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
