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

    [Header("Power Management")]
    public List<string> allPowerList;
    public List<string> curPowerList;
    private List<string> tmpAllPowerList;
    private List<string> roulette;
    private string tmpPow;


    private void Awake()
    {
        if (Instance == null) { Instance = this; } else { Destroy(this); }
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

    public void PowerRoulette()
    { 
        roulette = new List<string>();
        tmpAllPowerList = new List<string>(allPowerList);
        while (roulette.Count < 4)
        {
            string pow = tmpAllPowerList[Random.Range(0, tmpAllPowerList.Count)];
            if (tmpAllPowerList.Count < 3)
            {
                roulette.Add("DETERMINATION");
            } else if (curPowerList.Contains(pow))
            {
                tmpAllPowerList.Remove(pow);
                continue ;
            } else
            {
                roulette.Add(pow);
            }
        }
        GUIManager.Instance.StartRoulette(roulette);
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(sceneList[sceneNumber + 1]);
    }

    public void AddNewPower(int nb)
    {
        curPowerList.Add(roulette[nb]);
    }


    public void Exit()
    {
        Application.Quit();
    }
}
