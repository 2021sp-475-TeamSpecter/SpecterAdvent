using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{

    public int numDeaths = 0; 

    public static PlayerData Instance;

    void Awake ()   
       {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy (gameObject);
        }
      }

    void Start()
    {
        
    }
    
    public void SaveData(int deaths)
    {
        numDeaths = deaths;
    }

}
