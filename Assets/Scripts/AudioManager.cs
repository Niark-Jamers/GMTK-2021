using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource  backgroundPlayer;
    public AudioSource  sfxPlayer;

    public static AudioManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void PlayOnShot(AudioClip clip, float volume = 1)
    {
        instance.sfxPlayer.PlayOneShot(clip, volume);
    }

    void Start()
    {
        var background = FindObjectOfType<BackgroundMusic>();

        if (backgroundPlayer.clip != background.background)
        {
            backgroundPlayer.Stop();
            backgroundPlayer.clip = background.background;
            backgroundPlayer.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
