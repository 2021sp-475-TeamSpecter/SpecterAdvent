using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class DialogManager : MonoBehaviour
{

    public string[] lines; 
    public int currLine = 0;
    public Text text; 
    public PlayableDirector director; 
    public GameObject dialogBox; 

    void Update()
    {
        if (dialogBox.activeSelf)
            director.Pause();
    }

    public void NextDialog()
    {
        if (currLine < lines.Length)
        {
            text.text = lines[currLine];
            currLine += 1;
        }
        else
        {
            // deactivate dialog box 
            dialogBox.SetActive(false);
            // Play ending of cutscene 
            director.time += 1f;
            director.Resume();
        }
    }

}
