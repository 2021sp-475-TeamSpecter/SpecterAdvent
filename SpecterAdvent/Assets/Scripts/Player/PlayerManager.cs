using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DamageType;

public class PlayerManager : MonoBehaviour
{

    public GameObject Zero;
    public Collider2D ZeroCollider;
    public GameObject X; 
    public Collider2D XCollider; 
    int CharacterSelect;

    public bool isDead = false; 

    void Start()
    { 
    	CharacterSelect = 1;
        // get colliders
        ZeroCollider = Zero.GetComponent<Collider2D>();
        XCollider = X.GetComponent<Collider2D>();
        // Reset characters
        X.GetComponent<XController>().Respawn();
        Zero.GetComponent<PlayerController2D>().Respawn();
        // Set X to be active first 
        X.SetActive(true);
        Zero.SetActive(false);
    }

    void Update()
    {

        if (isDead) return;

        // Check if one of the characters died 
        if (Zero.GetComponent<PlayerController2D>().IsDead()
            || X.GetComponent<XController>().IsDead())
        {
            isDead = true; 
        }

        // Character Swap 
        if(Input.GetButtonDown("Fire1"))
        {
            SwitchActiveCharacter();
        }
    }

    // switches which character is active and on screen 
    public void SwitchActiveCharacter()
    {
        if(CharacterSelect == 1)
        {
            CharacterSelect = 2;             
            Zero.transform.position = X.transform.position;
            X.SetActive(false);
            Zero.SetActive(true);
        }
        else if (CharacterSelect == 2)
        {
            CharacterSelect = 1;
            X.transform.position = Zero.transform.position;                
            Zero.SetActive(false);
            X.SetActive(true);       
        }
    }

    // gets the position of the player 
    public Vector3 GetPosition()
    {
        // Determine which character is active
        if (Zero.activeSelf)
        {
            return Zero.transform.position;
        }
        else
        {
            return X.transform.position; 
        }
    }

    // returns the current character's collider
    public Collider2D GetCollider()
    {
        // Determine which character is active
        if (Zero.activeSelf)
        {
            return ZeroCollider;
        }
        else
        {
            return XCollider; 
        }
    }

    // subtracts health from the current character
    public void TakeDamage(float damage, DamageType damageType)
    {
        // Determine which character is active
        if (Zero.activeSelf)
        {
            Zero.GetComponent<PlayerController2D>().TakeDamage(damage, damageType);
        }
        else
        {
            X.GetComponent<XController>().TakeDamage(damage, damageType);
        }
    }

    // adds health to player
    public void AddHealth(float amountOfHealth)
    {
        // Determine which character is active
        if (Zero.activeSelf)
        {
            Zero.GetComponent<PlayerController2D>().AddHealth(amountOfHealth);
        }
        else
        {
            X.GetComponent<XController>().AddHealth(amountOfHealth);
        }
    }

    // kills player
    public void Die()
    {
        Zero.GetComponent<PlayerController2D>().currHealth = 0;
        X.GetComponent<XController>().currHealth = 0;
        isDead = true; 
    }

    public void Respawn(Vector3 spawnPoint)
    {   
        // move to respawn point
        X.transform.position = spawnPoint;
        Zero.transform.position = spawnPoint;
        // reset characters 
        X.GetComponent<XController>().Respawn();
        Zero.GetComponent<PlayerController2D>().Respawn();
        
        isDead = false; 
    }

}
