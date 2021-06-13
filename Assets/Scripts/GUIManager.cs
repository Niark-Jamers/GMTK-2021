using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{

    public static GUIManager Instance { get; private set; }

    public Slider lifeBar;
    public GameObject pausePanel;
    public GameObject roulettePanel;
    public Image imageRoulette1;
    public Text textRoulette1;
    public Image imageRoulette2;
    public Text textRoulette2;
    public Image imageRoulette3;
    public Text textRoulette3;
    public bool pause;

    public Image itemImage1;
    public Image itemImage2;
    public Image itemImage3;

    public AudioClip clickClip;
    public AudioClip openClip;
    public AudioClip closeClip;

    [Header("Hearts")]
    public Image    heart1;
    public Image    heart2;
    public Image    heart3;
    Color heartColor;

    [Header("Fade In")]
    public GameObject   levelFadeIn;
    
    [Header("Winner")]
    public GameObject   winner;

    public void Awake()
    {
        heartColor = heart1.color;
        if (Instance == null) { Instance = this; } else { Destroy(this); }
    }
    public void SetLife(float lifeBetween01)
    {
        lifeBar.value = lifeBetween01;
    }


    public void Pause()
    {
        if (pause)
            AudioManager.PlayOnShot(closeClip);
        else
            AudioManager.PlayOnShot(openClip);
        pausePanel.SetActive(!pausePanel.activeSelf);
        if (Time.timeScale == 0) { Time.timeScale = 1; } else { Time.timeScale = 0; }
        pause = !pause;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            Time.timeScale = 0;
            GameManager.Instance.PowerRoulette();
        }
    }

    public void StartRoulette(List<GameManager.GUIPowers> choices)
    {
        AudioManager.PlayOnShot(openClip);
        textRoulette1.text = choices[1].name;
        textRoulette2.text = choices[2].name;
        textRoulette3.text = choices[3].name;
        itemImage1.sprite = choices[1].image;
        itemImage2.sprite = choices[2].image;
        itemImage3.sprite = choices[3].image;
        roulettePanel.SetActive(true);
    }

    public void RouletteChoice(int nb)
    {
        AudioManager.PlayOnShot(clickClip);
        FindObjectOfType<Player>().freeMovements = false;
        roulettePanel.SetActive(false);
        GameManager.Instance.AddNewPower(nb);
        Time.timeScale = 1;
    }

    public void UpdateLife(int hp)
    {
        heart3.color = hp > 2 ? heartColor : Color.black;
        heart2.color = hp > 1 ? heartColor : Color.black;
        heart1.color = hp > 0 ? heartColor : Color.black;
    }

    public void TastyTest()
    {
        Debug.Log("prout");
    }

    public void LoadNextLevel()
    {
        levelFadeIn.GetComponent<Animator>().enabled = true;
    }

    public void WinScreen()
    {
        winner.GetComponent<Animator>().enabled = true;
    }
}
