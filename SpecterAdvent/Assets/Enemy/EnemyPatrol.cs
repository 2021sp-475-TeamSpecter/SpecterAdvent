using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{

    public float sightRange = 1; 
    public float movementSpeed = 1; 
    public Rigidbody2D rigidbody; 
    public bool canMove = true; 
    
    public Transform groundDetector; 
    public float groundDetectionDist = 0.5f; 
    public Transform wallDetector; 
    public float wallDetectionDist = 0.3f; 
    public bool movingRight = true; 

    public PlayerManager playerManager; 
    public Transform enemyGFX; 
    
    LayerMask mask; 

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        // Enemy can only see players and obstacles
        // Things like bullets/enemies are ignored 
        mask = LayerMask.GetMask("Player") | LayerMask.GetMask("Ground");
    }

    void Update()
    {
        
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

            RaycastHit2D wallHit = Physics2D.Raycast(
                wallDetector.position,
                -Vector2.right,
                wallDetectionDist
            );

            // no more ground or encountered a wall
            if (groundHit.collider == null
                || groundHit.collider.gameObject.layer != LayerMask.NameToLayer("Ground")
                || (wallHit.collider != null && wallHit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")))
            {
                // switch to left
                if (movingRight)
                {
                    // face left 
                    enemyGFX.localScale = new Vector3(-Mathf.Abs(enemyGFX.localScale.x), enemyGFX.localScale.y, enemyGFX.localScale.z);
                    // move left 
                    rigidbody.velocity = new Vector3(movementSpeed, 0, 0);
                    movingRight = false; 
                }
                // switch to right 
                else
                {
                    // face right 
                    enemyGFX.localScale = new Vector3(Mathf.Abs(enemyGFX.localScale.x), enemyGFX.localScale.y, enemyGFX.localScale.z);
                    // move right 
                    rigidbody.velocity = new Vector3(-movementSpeed, 0, 0);
                    movingRight = true; 
                }
            }

        }
    }
}
