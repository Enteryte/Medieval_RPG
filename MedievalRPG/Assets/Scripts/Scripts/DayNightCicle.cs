using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class DayNightCicle : MonoBehaviour
{
    [SerializeField]
    private float lengthOfDay = 30; //Zeit in Minuten
    [SerializeField]
    private float lengthOfNight = 30; //Zeit in Minuten
    [SerializeField]
    private float destinationTemperature;
    [SerializeField]
    private float destinationIntensityV;
    [SerializeField]
    private float destinationIntensityL;
    [SerializeField]
    private Light dayLight;
    [SerializeField]
    private HDAdditionalLightData lightData;
    [SerializeField]
    private Volume vol;
    [SerializeField]
    private GameObject lightObject;
    [SerializeField]
    private Animator anim;

    private float startTemperature;
    private float startIntensityV;
    private float startIntensityL;
    private float currentIntensityL;
    private bool day = true;
    private HDRISky vIntensity;

    public bool stopTime = true;

    private void Start()
    {
        vol.profile.TryGet<HDRISky>(out HDRISky sky);
        vIntensity = sky;

        startIntensityV = vIntensity.multiplier.value;
        startTemperature = dayLight.colorTemperature;
        startIntensityL = lightData.intensity;
        currentIntensityL = startIntensityL;

        anim.speed /= lengthOfDay * 60; 
    }

    private void PassTime()
    {
        if(stopTime == false)
        {
            if(day)
            {
                float temperatureValue = 0;
                temperatureValue += ((destinationTemperature - startTemperature) / (lengthOfDay * 60)) * Time.deltaTime;
                dayLight.colorTemperature += temperatureValue;
                
                float vIntensitiValue = 0;
                vIntensitiValue += ((destinationIntensityV - startIntensityV) / (lengthOfDay * 60)) * Time.deltaTime;
                vIntensity.multiplier.value += vIntensitiValue;

                float lIntensitiValue = 0;
                lIntensitiValue += ((destinationIntensityL - startIntensityL) / (lengthOfDay * 60)) * Time.deltaTime;
                lightData.intensity += lIntensitiValue;

                if (lightData.intensity <= destinationIntensityL)
                {
                    stopTime = true;
                }
            }
        }
    }

    private void Update()
    {
        PassTime();
    }
}
