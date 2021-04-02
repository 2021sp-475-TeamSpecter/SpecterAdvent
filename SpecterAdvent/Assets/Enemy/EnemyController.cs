using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DamageType; 

public class EnemyController : MonoBehaviour
{

    // Health  
    [Header( "Health" )]
    public float maxHealth = 100;
    public float currentHealth = 100;
    public RectTransform healthBar; 

    [Header("Resistance")]
    public DamageType[] resistances; 
    public float[] percentBlocked; 

    [Header("Drops")]
    public GameObject healthDropPrefab;
    public float amountOfHealth = 10; 
    public float numDrops = 5; 
    public float scaleLow = 1;
    public float scaleHigh = 3; 
    public float angleLow = 45;
    public float angleHigh = 135;
    public float speedLow = 1;
    public float speedHigh = 3;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        // adjust healthbar 
        // healthbar is 200 wide 
        // convert health level to bar width 
        float healthPercent = currentHealth / maxHealth; 
        float barLevel = healthPercent * 200;
        healthBar.sizeDelta = new Vector2(barLevel, healthBar.sizeDelta.y);
        
        // enemy ran out of health
        if (currentHealth <= 0)
        {
            Die();
        }

    }

    // subtracts a given amount of health from enemy scaling the damage
    // based on resistances to types of attacks 
    public void TakeDamage(float damage, DamageType damageType)
    {
        float adjustedDamage = damage;
        // adjust damage based on enemy's resistances  
        for (int i = 0; i < resistances.Length; ++i)
        { 
            if (resistances[i] == damageType)
                adjustedDamage *= 1 - percentBlocked[i];
        }
        // take off health
        // ensure health does not go below 0 
        currentHealth = Mathf.Max(currentHealth - adjustedDamage, 0f);
    }

    // Handles the procedure for the enemy dying 
    void Die()
    {
        Destroy(gameObject);
        // Spawn drops
        if (healthDropPrefab != null)
        {
            for (int i = 0; i < numDrops; ++i)
            {
                // spawn
                GameObject healthDrop = Instantiate(healthDropPrefab, transform.position, transform.rotation);
                // set health amount
                healthDrop.GetComponent<HealthItem>().amountOfHealth = amountOfHealth;
                // set random scale
                float randScale = Random.Range(scaleLow, scaleHigh);
                healthDrop.transform.localScale = new Vector3(randScale, randScale, 0);
                // shoot in rand direction
                float randAngle = Random.Range(angleLow, angleHigh);
                Vector3 direction = Quaternion.Euler(0,0,randAngle) * Vector3.right;
                // with random velocity
                float randSpeed = Random.Range(speedLow, speedHigh); 
                healthDrop.GetComponent<Rigidbody2D>().velocity = direction * randSpeed;
            }
            
        }
    }
}
