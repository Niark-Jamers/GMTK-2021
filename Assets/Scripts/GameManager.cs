using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Scene Management")]
    public string[] sceneList;
    string curScene;
    int sceneNumber;

    public enum Mods
    {
        laser,
        fire,
        ice,
        zigzag,
        explosion,
        bounce,
        Multi,
        deter,
    }

    [System.Serializable]
    public class GUIPowers
    {
        public string name;
        public Sprite image;
        public Mods mod;
    }

    public Sprite determinationSprite;

    [Header("Power Management")]
    public List<GUIPowers> allPowerList;
    public List<GUIPowers> curPowerList;
    private List<GUIPowers> tmpAllPowerList;
    private List<GUIPowers> roulette;
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
        Time.timeScale = 0;
        FindObjectOfType<Player>().freeMovements = true;
        roulette = new List<GUIPowers>();
        tmpAllPowerList = (allPowerList).ToList();
        while (roulette.Count < 4)
        {
            var pow = tmpAllPowerList[Random.Range(0, tmpAllPowerList.Count)];
            if (tmpAllPowerList.Count < 3)
            {
                roulette.Add(new GUIPowers{ name = "DETERMINATION", mod = Mods.deter, image = determinationSprite} );
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

    public event System.Action newPowerAdded;
    public void AddNewPower(int nb)
    {
        curPowerList.Add(roulette[nb]);

        switch (roulette[nb].mod)
        {
            case Mods.bounce:
                FindObjectOfType<Link>().p.bMod.bounce = true;
                break;
            case Mods.deter:
                // Haha
                break;
            case Mods.explosion:
                FindObjectOfType<Link>().p.bMod.explosion = true;
                break;
            case Mods.fire:
                FindObjectOfType<Link>().p.bMod.fire = true;
                break;
            case Mods.ice:
                FindObjectOfType<Link>().p.bMod.ice = true;
                break;
            case Mods.laser:
                FindObjectOfType<Link>().p.bMod.laser = true;
                break;
            case Mods.Multi:
                FindObjectOfType<Link>().p.multiShot += 2;
                break;
            case Mods.zigzag:
                FindObjectOfType<Link>().p.bMod.zigzag = true;
                break;
        }

        newPowerAdded?.Invoke();
    }


    public void Exit()
    {
        Application.Quit();
    }
}
