using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public PlayerData playerData; 
    public Text deathsText;
    public Text enemiesLeftText; 

    public PlayerManager playerManager;
    public Transform playerSpawn; 
    public int numDeaths = 0;
    
    public GameObject enemies; 
    public bool areEnemiesDead = false; 

    public GameObject exit; 
    public string nextScene; 

    void Start()
    {
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        playerData = GameObject.Find("PlayerData").GetComponent<PlayerData>();
        numDeaths = playerData.numDeaths;
    }

    void Update()
    {

        deathsText.text = numDeaths.ToString();
        enemiesLeftText.text = (enemies.transform.childCount).ToString();

        // check if player died
        if (playerManager.isDead)
        {
            Debug.Log("Game over!");
            numDeaths += 1; 
            playerManager.Respawn(playerSpawn.position);
        }        

        if (enemies.transform.childCount == 0 && !areEnemiesDead)
        {
            // unlock exit 
            Debug.Log("All enemies dead");
            exit.SetActive(true);
            areEnemiesDead = true;
        }

        // check if player exitted level 
        if (exit.GetComponent<Exit>().playerExited)
        {
            // save data
            playerData.SaveData(numDeaths);
            // load next scene 
            SceneManager.LoadScene(nextScene);
        }
    }
}
