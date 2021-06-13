using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Experimental.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    Light2D light2D;
    public float scrollSpeed = 1;
    public float amplitude = 0.5f;
    float originalIntensity;
    float seed;
    // Start is called before the first frame update
    void Start()
    {
        light2D = GetComponent<Light2D>();
        originalIntensity = light2D.intensity;
        seed = Random.value * 1000;
    }

    // Update is called once per frame
    void Update()
    {
        light2D.intensity = originalIntensity + amplitude * Mathf.PerlinNoise(Time.time * scrollSpeed, seed);
    }
}
