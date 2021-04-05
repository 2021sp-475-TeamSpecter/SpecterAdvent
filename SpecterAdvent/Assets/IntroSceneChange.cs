using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneChange : MonoBehaviour
{
    public string nextScene;

    void OnEnable()
    {
        SceneManager.LoadScene(nextScene);
    }
}
