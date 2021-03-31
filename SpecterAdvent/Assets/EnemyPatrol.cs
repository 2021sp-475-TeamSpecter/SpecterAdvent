using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{

    public float sightRange = 1; 
    public float groundDetectionDist = 0.5f; 
    public float movementSpeed = 1; 
    public Rigidbody2D rigidbody; 
    public bool canMove = true; 
    
    public Transform groundDetector; 
    public bool movingRight = true; 

    public PlayerManager playerManager; 
    public Transform enemyGFX; 
    public PickaxeThrow throwScript; 
    
    LayerMask mask; 

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        throwScript = GetComponent<PickaxeThrow>();
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
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
            transform.position + directionToPlayer.normalized, 
            directionToPlayer, 
            sightRange, 
            mask
        );

        // // if enemy can see player
        // if (hit.collider != null
        //     && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        // {
        //     // stop patrolling 
        //     rigidbody.velocity = new Vector3(0,0,0);
        //     // face player 
        //     // left 
        //     if (directionToPlayer.x >= 0.05f)
        //     {
        //         enemyGFX.localScale = new Vector3(-Mathf.Abs(enemyGFX.localScale.x), enemyGFX.localScale.y, enemyGFX.localScale.z);
        //     }
        //     else if (directionToPlayer.x <= -0.05f)
        //     {
        //         enemyGFX.localScale = new Vector3(Mathf.Abs(enemyGFX.localScale.x), enemyGFX.localScale.y, enemyGFX.localScale.z);
        //     }  

        // }
        // // player not visible
        // // resume patrol as long as hes not waiting for the pickaxe 
        // else 
        
        if (canMove)
        {

            // move 
            if (movingRight)
            {
                // face right 
                enemyGFX.localScale = new Vector3(-Mathf.Abs(enemyGFX.localScale.x), enemyGFX.localScale.y, enemyGFX.localScale.z);
                // move right 
                rigidbody.velocity = new Vector3(movementSpeed, 0, 0);
            }
            else
            {
                // face left 
                enemyGFX.localScale = new Vector3(Mathf.Abs(enemyGFX.localScale.x), enemyGFX.localScale.y, enemyGFX.localScale.z);
                // move left 
                rigidbody.velocity = new Vector3(-movementSpeed, 0, 0);
            }

            RaycastHit2D groundHit = Physics2D.Raycast(
                groundDetector.position, 
                Vector2.down, 
                groundDetectionDist
            );

            // no more ground 
            if (groundHit.collider == null
                || groundHit.collider.gameObject.layer != LayerMask.NameToLayer("Ground"))
            {
                if (movingRight)
                {
                    // face right 
                    enemyGFX.localScale = new Vector3(-Mathf.Abs(enemyGFX.localScale.x), enemyGFX.localScale.y, enemyGFX.localScale.z);
                    // move right 
                    rigidbody.velocity = new Vector3(movementSpeed, 0, 0);
                    movingRight = false; 
                }
                else
                {
                    // face left 
                    enemyGFX.localScale = new Vector3(Mathf.Abs(enemyGFX.localScale.x), enemyGFX.localScale.y, enemyGFX.localScale.z);
                    // move left 
                    rigidbody.velocity = new Vector3(-movementSpeed, 0, 0);
                    movingRight = true; 
                }
            }

        }
        // cannot move 
        else 
        {
            rigidbody.velocity = new Vector3(0,0,0);
        }
    }
}
