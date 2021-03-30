using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
	public PlayerManager playerManager; 

    void Start()
    {
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = playerManager.GetPosition();
        transform.position = new Vector3(playerPos.x, playerPos.y, -7);
    }
}
