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
            Destroy(gameObject);
        instance = this;
    }

    public static void PlayOnShot(AudioClip clip, float volume)
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
