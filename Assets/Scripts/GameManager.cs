using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Scene Management")]
    public string[] sceneList;
    string curScene;
    int sceneNumber;

    bool pause = false;

    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Debug.Log("Warning: multiple " + this + " in scene!"); }
        DontDestroyOnLoad(this);

        curScene = SceneManager.GetActiveScene().name;
        for (int i = 0; i < sceneList.Length; i++)
        {
            if (sceneList[i] == curScene)
            {
                sceneNumber = i;
                break;
            }
        }
    }

    void Update()
    {
        
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(sceneList[sceneNumber + 1]);
    }



    public void Pause()
    {
        pause = !pause;
    }
}
