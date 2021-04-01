using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeThrow : MonoBehaviour
{

    public float damage = 20; 
    public float sightRange = 1; 
    public float attackSpeed = 1;  
    public float pickaxeSpeed = 1; 
    public float pickaxeDuration = 2; 
    float lastAttack; 
    public Transform throwPoint; 
    public GameObject pickaxePrefab; 
    public PlayerManager playerManager; 
    public EnemyPatrol enemyPatrolMechanics; 
    public Transform enemyGFX; 
    public Animator animator;
    public bool hasPickaxe = true; 

    LayerMask mask; 

    void Start()
    {
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        enemyPatrolMechanics = GetComponent<EnemyPatrol>();
        // Enemy can only see players and obstacles
        // Things like bullets/enemies are ignored 
        mask = LayerMask.GetMask("Player") | LayerMask.GetMask("Ground");
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

            // if not on attack cooldown and has pickaxe 
            if (Time.time > attackSpeed + lastAttack
                && hasPickaxe)
            {
                
                StartCoroutine(Throw(directionToPlayer));

                hasPickaxe = false; 

                lastAttack = Time.time;

            }

        }
        // cannot see player 
        // allow moving when pickaxe is returned 
        else if (hasPickaxe)
        {
            // allow moving 
            enemyPatrolMechanics.canMove = true; 
        }
    }

    IEnumerator Throw(Vector3 directionToPlayer)
    {
        // Throw pickaxe 
        animator.SetTrigger("Throw");
        // Wait until pickaxe leaves hand
        yield return new WaitForSeconds(0.5f);
        // Spawn pickaxe
        GameObject pickaxe = Instantiate(pickaxePrefab, throwPoint.position, throwPoint.rotation);
        pickaxe.GetComponent<Pickaxe>().SendPickaxe(
            gameObject,
            directionToPlayer.normalized * pickaxeSpeed,
            pickaxeDuration, 
            damage
        );
        // Play sound
        // shootSFX.Play();
    }

    public void ReturnPickaxe()
    {
        hasPickaxe = true; 
        lastAttack = Time.time; 
    }

    // Draws a gizmo for the sight range of the enemy
    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);    
    }

}
