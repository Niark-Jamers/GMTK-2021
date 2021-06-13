using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera    virtualCamera;
    CinemachineBasicMultiChannelPerlin  noise;

    static CameraManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public static void Shake(float amplitude = 3, float duration = 0.1f)
    {
        instance.noise.m_AmplitudeGain = amplitude;

        instance.StopCoroutine(UpdateAmplitude());
        instance.StartCoroutine(UpdateAmplitude());

        IEnumerator UpdateAmplitude()
        {
            float t = Time.time;
            while (Time.time - t < duration)
            {
                instance.noise.m_AmplitudeGain = Mathf.Lerp(amplitude, 0, (Time.time - t) / duration);
                yield return new WaitForEndOfFrame();
            }
        }

        instance.noise.m_AmplitudeGain = 0;
    }

    void Update()
    {
        
    }
}
