using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    public PlayerData playerData;
    public Text deathsText; 

    void Start()
    {
        playerData = GameObject.Find("PlayerData").GetComponent<PlayerData>();

        deathsText.text = playerData.numDeaths.ToString();

    }
    
    public void ButtonMoveScene(string level)
    {
        SceneManager.LoadScene(level);
    }
}
