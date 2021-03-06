using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAttack : MonoBehaviour
{

    public GameObject bulletPrefab;
    public Transform bulletSpawnLocation;
    public float bulletSpeed = 1; 
    public float attackSpeed = 1;
    public float bulletDamage = 10;  
    public GameObject player;

    public LayerMask mask; 

    public float lastAttack; 

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        RaycastHit2D hit = Physics2D.Raycast(transform.position + directionToPlayer.normalized, directionToPlayer, 100.0f, mask);
        // Only shoot at player if they are in line of sight
        if (hit.collider != null 
            && hit.collider.gameObject.name == player.name 
            && Time.time > attackSpeed + lastAttack
            /*&& !GetComponent<EnemyController>().isDying*/)
        {
            Shoot();
            lastAttack = Time.time;
        }
    }

    void Shoot()
    {
        // Spawn bullet
        // GameObject bullet = Instantiate(bulletPrefab, bulletSpawnLocation.position, bulletSpawnLocation.rotation);
        // bullet.GetComponent<EnemyBullet>().setBulletDamage(attackDamage);
        // Send bullet
        // Rigidbody2D bulletrigid = bullet.GetComponent<Rigidbody2D>();
        // bulletrigid.AddForce(bulletSpawnLocation.up * bulletForce, ForceMode2D.Impulse);
        // Play sound
        // shootSFX.Play();
    }
}
