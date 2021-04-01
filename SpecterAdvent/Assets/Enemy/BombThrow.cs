using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombThrow : MonoBehaviour
{
    public float damage = 20; 
    public float sightRange = 1; 
    public float attackSpeed = 1;  
    public float bombSpeed = 5; 
    public float bombTimer = 2; 
    float lastAttack; 
    public Transform throwPoint; 
    public GameObject bombPrefab; 
    public PlayerManager playerManager; 
    public EnemyPatrol enemyPatrolMechanics; 
    public Transform enemyGFX; 
    public Animator animator;

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
        // only throw pickaxe if player is in range 
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
            if (Time.time > bombTimer + attackSpeed + lastAttack)
            {
                StartCoroutine(Throw(directionToPlayer));
                lastAttack = Time.time;
            }

        }
        // cannot see player 
        else
        {
            // allow moving 
            enemyPatrolMechanics.canMove = true; 
        }
    }

    IEnumerator Throw(Vector3 directionToPlayer)
    {
        // Throw pickaxe 
        animator.SetTrigger("Throw");
        // Wait until bomb leaves hand
        yield return new WaitForSeconds(0.5f);
        // Spawn pickaxe
        GameObject bomb = Instantiate(bombPrefab, throwPoint.position, throwPoint.rotation);
        bomb.GetComponent<Bomb>().SendBomb(
            gameObject,
            directionToPlayer.normalized * bombSpeed,
            bombTimer, 
            damage
        );
        // Play sound
        // shootSFX.Play();
    }

    // Draws a gizmo for the sight range of the enemy
    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);    
    }
}
