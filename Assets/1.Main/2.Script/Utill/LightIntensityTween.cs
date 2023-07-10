using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIntensityTween : MonoBehaviour
{
    public Light directionalLight;
    public void StartDay()
    {
        directionalLight.DOIntensity(0.8f, 7);
    }
    public void StartNight()
    {
        directionalLight.DOIntensity(0.1f, 7);
    }
    void Awake()
    {
        directionalLight = GetComponent<Light>();
    }
}
