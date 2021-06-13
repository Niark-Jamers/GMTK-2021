using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PixelPerfectTransition : MonoBehaviour
{
    PixelPerfectCamera perfectCamera;

    public float time = 1f;
    public float offset = 0.5f;

    public bool inTransition;

    // Start is called before the first frame update
    void Start()
    {
        perfectCamera = FindObjectOfType<PixelPerfectCamera>();

        if (inTransition)
            StartCoroutine(Transition());
        else
            StartCoroutine(Transition2());
    }

    IEnumerator Transition()
    {
        float x = perfectCamera.refResolutionX;
        float y = perfectCamera.refResolutionY;
        float p = perfectCamera.assetsPPU;
        float t = Time.time;
        while (Time.time - t < time)
        {
            float i01 = (Time.time - t) / time;

            float level = Mathf.Clamp(Mathf.Lerp(1, p, i01 + offset), 1, p);
            perfectCamera.refResolutionX = (int)(x / level);
            perfectCamera.refResolutionY = (int)(y / level);
            perfectCamera.assetsPPU = (int)p - (int)level; 

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator Transition2()
    {
        float x = perfectCamera.refResolutionX;
        float y = perfectCamera.refResolutionY;
        float p = perfectCamera.assetsPPU;
        float t = Time.time;
        while (Time.time - t < time)
        {
            float i01 = (Time.time - t) / time;
            Debug.Log(i01);

            float level = Mathf.Clamp(Mathf.Lerp(p, 1, i01 + offset), 1, p);
            perfectCamera.refResolutionX = (int)(x / level);
            perfectCamera.refResolutionY = (int)(y / level);
            perfectCamera.assetsPPU = (int)p - (int)level; 
            yield return new WaitForEndOfFrame();
        }
        perfectCamera.refResolutionX = (int)x;
        perfectCamera.refResolutionY = (int)y;
        perfectCamera.assetsPPU = (int)p;
    }
}
