using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    // Health 
    [Header( "Health" )]
    public float maxHealth = 100;
    public float currentHealth = 100;
    public RectTransform healthBar; 
    // Health decrease over time
    public float decreaseTime = 1;
    public float prevTime; 

    [Header( "Movement" )]
    public float movementSpeed = 1;
    public Rigidbody2D rigidbody;
    public Transform spriteTransform; 
    public GameObject player;


    void Start()
    {
        currentHealth = maxHealth;
        rigidbody = GetComponent<Rigidbody2D>();
        // Keep track of player
        player = GameObject.Find("Player"); 
        
    }

    void Update()
    {
        // adjust healthbar 
        // healthbar is 200 wide 
        // convert health level to bar width 
        float healthPercent = currentHealth / maxHealth; 
        float barLevel = healthPercent * 200;
        healthBar.sizeDelta = new Vector2(barLevel, healthBar.sizeDelta.y);
        
        // gradually decrease health 
        if (Time.time > prevTime + decreaseTime)
        {
            prevTime = Time.time;
            currentHealth -= 5;
        }

        // move towards player
        Vector3 directionToPlayer = player.transform.position - transform.position;
        rigidbody.velocity = directionToPlayer.normalized * movementSpeed;

    }
}
