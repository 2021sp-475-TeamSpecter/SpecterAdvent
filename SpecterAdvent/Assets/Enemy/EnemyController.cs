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

    // subtracts a given amount of health from enemy 
    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0f);
    }

    // Handles the procedure for the enemy dying 
    void Die()
    {
        Destroy(gameObject);
    }
}
