using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShootAttack : MonoBehaviour
{

    public GameObject bulletPrefab;
    public Transform bulletSpawnLocation;
    public float bulletSpeed = 1; 
    public float attackSpeed = 1;
    public float attackDamage = 10;  
    public float cannonTurnSpeed = 2;
    public float sightRange = 1;
    public Transform cannonPivot; 

    public PlayerManager playerManager; 

    LayerMask mask; 
    float lastAttack; 

    void Start()
    {
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        // Enemy can only see players and obstacles
        // Things like bullets/enemies are ignored 
        mask = LayerMask.GetMask("Player") | LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 directionToPlayer = player.transform.position - transform.position;
        Vector3 directionToPlayer = playerManager.GetPosition() - transform.position; 

        // Determine if enemy can see player or obstacle 
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position + directionToPlayer.normalized, 
            directionToPlayer, 
            sightRange, 
            mask
        );
        
        // if turret can see player
        if (hit.collider != null
            && hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            float angle = Vector3.Angle(directionToPlayer, transform.right);
            // clamping to interval [0, 180]
            // so cannon does rotate downwards 
            angle = Mathf.Clamp(angle, 0f, 180f);
            // rotate towards player slowly
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle-90);
            cannonPivot.rotation = Quaternion.Slerp(cannonPivot.rotation, targetRotation, Time.deltaTime * cannonTurnSpeed);

            // if not on attack cooldown 
            if (Time.time > attackSpeed + lastAttack)
            {
                // attack 
                Shoot();
                lastAttack = Time.time;
            }

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
