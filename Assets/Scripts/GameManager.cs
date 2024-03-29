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

    public float bulletSpeedMultiplier = 1;
    public float aggroZoneMultiplier = 1;

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
        SceneManager.sceneLoaded += Upgrwhjk;
    }

    void Upgrwhjk(Scene s, LoadSceneMode a)
    {
        foreach (var c in curPowerList)
            AddPower(c.mod);
    }

    void Update()
    {
    }

    public int BounceCount()
    {
        return (curPowerList.FindAll(x => x.mod == Mods.bounce).Count);
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
            } else if (curPowerList.Contains(pow) && pow.mod != Mods.Multi && pow.mod != Mods.bounce)
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

    public void NextLevel(bool newGamePlus = false)
    {
        sceneNumber++;

        if (sceneNumber >= sceneList.Length)
        {
            if (newGamePlus)
            {
                sceneNumber = 2;
                bulletSpeedMultiplier += 0.5f;
                aggroZoneMultiplier += 0.5f;
                SceneManager.LoadScene(sceneList[sceneNumber]);
            }
            else
            {
                Debug.Log("HGIUWIOHGOI");
                GUIManager.Instance.WinScreen();
            }

            return;
        }

        SceneManager.LoadScene(sceneList[sceneNumber]);
    }

    public event System.Action newPowerAdded;
    public void AddNewPower(int nb)
    {
        curPowerList.Add(roulette[nb]);

        AddPower(roulette[nb].mod);

        newPowerAdded?.Invoke();
    }

    void AddPower(Mods power)
    {
        switch (power)
        {
            case Mods.bounce:
                FindObjectOfType<Link>().p.bMod.bounce += 2;
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
    }

    public void Exit()
    {
        Application.Quit();
    }
}
