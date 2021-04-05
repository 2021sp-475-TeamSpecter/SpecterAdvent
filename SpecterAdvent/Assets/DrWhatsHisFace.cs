using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DamageType; 

public class DrWhatsHisFace : MonoBehaviour
{

    public float damage = 5f; 
    public float attackSpeed = 0.5f;
    public Transform firePoint;
    public float fireHurtRange = 0.75f; 
    public float sightRange = 1;
    public float movementSpeed = 2;

    public EnemyController controller; 
    public EnemyPatrol enemyPatrol; 
    public GameObject enemyContainer; 
    public Rigidbody2D rigidbody; 
    public Animator animator; 
    public PlayerManager playerManager;
    public Transform enemyGFX; 
    LayerMask mask;
    float lastAttack; 

    public int stage = 0; 

    public Transform[] spawnPoints1; 
    public Transform[] spawnPoints2; 
    public Transform[] spawnPoints3; 
    public GameObject[] enemyPrefabs1; 
    public GameObject[] enemyPrefabs2; 
    public GameObject[] enemyPrefabs3; 

    void Start()
    {
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        mask = LayerMask.GetMask("Player") | LayerMask.GetMask("Ground");
        enemyPatrol = GetComponent<EnemyPatrol>();
    }

    void Update()
    {

        // advance stage when health is < 75%
        if (stage == 0 && controller.currentHealth < controller.maxHealth * 0.75)
        {
            stage = 1; 
            // spawn helper enemies
            for (int i = 0; i < spawnPoints1.Length; ++i)
            {
                // spawn enemy
                GameObject enemy = Instantiate(enemyPrefabs1[i], spawnPoints1[i].position, spawnPoints1[i].rotation);
                // parent to enemy container
                enemy.transform.parent = enemyContainer.transform; 
            }
        }
        // advance stage when health is < 50%
        else if (stage == 1 && controller.currentHealth < controller.maxHealth * 0.5)
        {
            stage = 2; 
            // spawn helper enemies
            for (int i = 0; i < spawnPoints2.Length; ++i)
            {
                // spawn enemy
                GameObject enemy = Instantiate(enemyPrefabs2[i], spawnPoints2[i].position, spawnPoints2[i].rotation);
                // parent to enemy container
                enemy.transform.parent = enemyContainer.transform; 
            }
        }
        // advance stage when health is < 25%
        else if (stage == 2 && controller.currentHealth < controller.maxHealth * 0.25)
        {
            stage = 3; 
            // spawn helper enemies
            for (int i = 0; i < spawnPoints3.Length; ++i)
            {
                // spawn enemy
                GameObject enemy = Instantiate(enemyPrefabs3[i], spawnPoints3[i].position, spawnPoints3[i].rotation);
                // parent to enemy container
                enemy.transform.parent = enemyContainer.transform; 
            }
        }

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
            animator.Play("wizardAttack");

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

            // move towards player 
            if (directionToPlayer.x >= 0.9f) // right
            {
                rigidbody.velocity = new Vector3(movementSpeed, 0, 0);
            }
            else if (directionToPlayer.x <= -0.9f) // left 
            {
                rigidbody.velocity = new Vector3(-movementSpeed, 0, 0);
            }
            else 
            {
                rigidbody.velocity = new Vector3(0, 0, 0);
            }

            // if not on attack cooldown
            if (Time.time > attackSpeed + lastAttack)
            {
                // attack 
                float distance = Vector3.Distance(playerManager.GetPosition(), firePoint.position);
                if (distance < fireHurtRange)
                {
                    playerManager.TakeDamage(damage, DamageType.Melee);
                }
                lastAttack = Time.time;
            }
        }
        // enemy cannot see player
        else
        {
            // resume patrol
            enemyPatrol.canMove = true; 
            animator.Play("wizardMove");
        }
    }

    // Draws a gizmo for the sight range of the turret
    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);   
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(firePoint.position, fireHurtRange); 
    }
}
