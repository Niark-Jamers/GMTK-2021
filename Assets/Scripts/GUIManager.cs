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

    public AudioClip clickClip;
    public AudioClip openClip;
    public AudioClip closeClip;

    public void Awake()
    {
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
            AudioManager.PlayOnShot(clickClip);
            Time.timeScale = 0;
            GameManager.Instance.PowerRoulette();
        }
    }

    public void StartRoulette(List<string> choices)
    {
        textRoulette1.text = choices[1];
        textRoulette2.text = choices[2];
        textRoulette3.text = choices[3];
        roulettePanel.SetActive(true);
    }

    public void RouletteChoice(int nb)
    {
        AudioManager.PlayOnShot(clickClip);
        roulettePanel.SetActive(false);
        GameManager.Instance.AddNewPower(nb);
        Time.timeScale = 1;
    }

    public void TastyTest()
    {
        Debug.Log("prout");
    }
}
