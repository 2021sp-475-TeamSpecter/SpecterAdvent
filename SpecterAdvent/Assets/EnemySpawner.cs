using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public Transform[] spawnPoints;
    public GameObject[] enemyPrefabs;

    public GameObject enemyContainer;

    void Start()
    {
        enemyContainer = GameObject.Find("Enemies");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // check that it was the player that collided
        if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            for (int i = 0; i < spawnPoints.Length; ++i)
            {
                // spawn enemy
                GameObject enemy = Instantiate(enemyPrefabs[i], spawnPoints[i].position, spawnPoints[i].rotation);
                // parent to enemy container
                enemy.transform.parent = enemyContainer.transform; 
            }

            // make sure you cant make them spawn again
            Destroy(gameObject);
        }

    }
}
