using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIntensityTween : MonoBehaviour //광원효과의 조명 세기 조절.
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
