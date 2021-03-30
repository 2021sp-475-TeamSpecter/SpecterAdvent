using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    public float movementSpeed = 1; 
    public float maxSpeed = 10; 
    public Transform enemyGFX;
    // How close an enemy needs to be to the waypoint before switching to the next  
    public float nextWaypointDistance = 1;


    Path path;
    int currentWaypoint = 0; 
    bool reachedEndOfPath = false; 

    Seeker seeker; 
    Rigidbody2D rigidbody; 

    PlayerManager playerManager; 

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rigidbody = GetComponent<Rigidbody2D>();
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();

        // continuously calc path 
        InvokeRepeating("UpdatePath", 0f, .5f);
        
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rigidbody.position, playerManager.GetPosition(), OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }


    void FixedUpdate()
    {
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true; 
            return;
        }
        else
        {
            reachedEndOfPath = false; 
        }

        // direction to next waypoint in path 
        Vector2 direction = ((Vector2) path.vectorPath[currentWaypoint] - rigidbody.position).normalized;
        Vector2 force = direction * movementSpeed * Time.deltaTime; 

        // move enemy to next waypoint 
        rigidbody.AddForce(force);

        // limit velocity
        // longer paths between waypoints accumulate more force 
        rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, maxSpeed);

        float distance = Vector2.Distance(rigidbody.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++; 
        }

        // Point enemy towards desired direction 
        if (force.x >= 0.05f)
        {
            enemyGFX.localScale = new Vector3(Mathf.Abs(enemyGFX.localScale.x), enemyGFX.localScale.y, enemyGFX.localScale.z);
        }
        else if (force.x <= -0.05f)
        {
            enemyGFX.localScale = new Vector3(-Mathf.Abs(enemyGFX.localScale.x), enemyGFX.localScale.y, enemyGFX.localScale.z);
        }
    }
}
