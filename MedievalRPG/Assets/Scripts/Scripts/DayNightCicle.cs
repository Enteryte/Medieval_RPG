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

    [Header("Day")]
    [SerializeField] private float dayDestinationTemperature;
    [SerializeField] private float dayDestinationIntensityV;
    [SerializeField] private float dayDestinationIntensityL;
    [SerializeField] private float dayDestinationWeigh;

    [SerializeField] private float dayStartTemperature;
    [SerializeField] private float dayStartIntensityV;
    [SerializeField] private float dayStartIntensityL;
    [SerializeField] private float dayStartWeigh;

    [SerializeField] private float dayStartRotAngleX;
    [SerializeField] private float dayStartRotAngleY;

    [SerializeField] private float dayDestRotAngleX;
    [SerializeField] private float dayDestRotAngleY;

    [Header("Night")]
    [SerializeField] private float nightDestinationTemperature;
    [SerializeField] private float nightDestinationIntensityV;
    [SerializeField] private float nightDestinationIntensityL;
    [SerializeField] private float nightDestinationWeigh;

    [SerializeField] private float nightStartTemperature;
    [SerializeField] private float nightStartIntensityV;
    [SerializeField] private float nightStartIntensityL;
    [SerializeField] private float nightStartWeigh;

    [SerializeField] private float nightStartRotAngleX;
    [SerializeField] private float nightStartRotAngleY;

    [SerializeField] private float nightDestRotAngleX;
    [SerializeField] private float nightDestRotAngleY;

    [Header("Objects")]
    [SerializeField]
    private Light dayLight;
    [SerializeField]
    private HDAdditionalLightData lightData;
    [SerializeField]
    private Volume vol;
    [SerializeField]
    private GameObject lightObject;

    private HDRISky vIntensity;

    public bool stopTime = true;
    public float currentDayTime = 0;
    public float timeInHoursAndMinutes = 0;

    private void Start()
    {
        vol.profile.TryGet<HDRISky>(out HDRISky sky);
        vIntensity = sky;
    }

    /// <summary>
    /// Setzt die uhrzeit auf den angegebenen Wert. 0 = 00:00Uhr, 86400 = 24:00Uhr.
    /// </summary>
    /// <param name="time"></param>
    public void SetTime(float time)
    {
        currentDayTime = time * 60 * 60;
    }

    private void PassTime()
    {
        if (stopTime == false)
        {
            Daytime();
            if (timeInHoursAndMinutes <= 6.00f)
            {
                NightToMorning();
            }
            if (timeInHoursAndMinutes <= 12.00f && timeInHoursAndMinutes > 6.00f)
            {
                MorningToMidday();
            }
            if (timeInHoursAndMinutes <= 18.00f && timeInHoursAndMinutes > 12.00f)
            {
                MiddayToEvening();
            }
            if (timeInHoursAndMinutes > 18.00f)
            {
                EveningToNight();
            }

            if (currentDayTime >= 86400)
            {
                currentDayTime = 0;
            }
        }

    }

    /// <summary>
    /// Berechnet die vergangenen Sekunden in Game und Konvertiert den Int in eine Tageszeit
    /// </summary>
    private void Daytime()
    {
        float h = 0;
        float m = 0;

        if (timeInHoursAndMinutes >= 6.00f && timeInHoursAndMinutes < 18.00f)
        {
            currentDayTime += 86400 / (lengthOfDay * 60) * Time.deltaTime;
        }
        else
        {
            currentDayTime += 86400 / (lengthOfNight * 60) * Time.deltaTime;
        }

        h = (int)((currentDayTime / 60) / 60);
        m = (float)Mathf.Round((60 * ((((currentDayTime / 60) / 60) - h) / 100)) * 100f) / 100f;
        timeInHoursAndMinutes = h + m;
    }

    private void NightToMorning()
    {
        dayLight.colorTemperature = nightDestinationTemperature - (currentDayTime * ((nightDestinationTemperature - dayStartTemperature) / (86400 / 4)));
        lightData.intensity = nightDestinationIntensityL - (currentDayTime * ((nightDestinationIntensityL - dayStartIntensityL) / (86400 / 4)));
        vIntensity.multiplier.value = nightDestinationIntensityV - (currentDayTime * ((nightDestinationIntensityV - dayStartIntensityV) / (86400 / 4)));
        vol.weight = nightDestinationWeigh - (currentDayTime * ((nightDestinationWeigh - dayStartWeigh) / (86400 / 4)));

        float currRotAngleX = nightDestRotAngleX - (currentDayTime * ((nightDestRotAngleX - dayStartRotAngleX) / (86400 / 4)));
        float currRotAngleY = nightDestRotAngleY - (currentDayTime * ((nightDestRotAngleY - dayStartRotAngleY) / (86400 / 4)));
        Vector3 eulers = new Vector3(currRotAngleX, currRotAngleY, 0);
        lightObject.transform.rotation = Quaternion.Euler(eulers);

        #region Kommentar
        /*float temperatureValue = 0;
        temperatureValue += ((destinationTemperature - startTemperature) / (lengthOfDay * 60)) * Time.deltaTime;
        dayLight.colorTemperature += temperatureValue;

        float lIntensitiValue = 0;
        lIntensitiValue += ((destinationIntensityL - startIntensityL) / (lengthOfDay * 60)) * Time.deltaTime;
        lightData.intensity += lIntensitiValue;

        float vIntensitiValue = 0;
        vIntensitiValue += ((destinationIntensityV - startIntensityV) / (lengthOfDay * 60)) * Time.deltaTime;
        vIntensity.multiplier.value += vIntensitiValue;

        float speed = 180 / (lengthOfDay * 60);
        lightObject.transform.Rotate(new Vector3(1, 1, 0), speed * Time.deltaTime);

        if (lightData.intensity <= destinationIntensityL)
        {
            lightObject.transform.rotation = new Quaternion(0, 90, 0, 0);
        }*/
        #endregion
    }
    private void MorningToMidday()
    {
        dayLight.colorTemperature = dayStartTemperature + (((currentDayTime - (86400 / 4))) * ((dayDestinationTemperature - dayStartTemperature) / (86400 / 4)));
        lightData.intensity = dayStartIntensityL + (((currentDayTime - (86400 / 4))) * ((dayDestinationIntensityL - dayStartIntensityL) / (86400 / 4)));
        vIntensity.multiplier.value = dayStartIntensityV + (((currentDayTime - (86400 / 4))) * ((dayDestinationIntensityV - dayStartIntensityV) / (86400 / 4)));
        vol.weight = dayStartWeigh + (((currentDayTime - (86400 / 4))) * ((dayDestinationWeigh - dayStartWeigh) / (86400 / 4)));

        float currRotAngleX = dayStartRotAngleX + ((currentDayTime - (86400 / 4)) * ((dayDestRotAngleX - dayStartRotAngleX) / (86400 / 4)));
        float currRotAngleY = dayStartRotAngleY + ((currentDayTime - (86400 / 4)) * ((dayDestRotAngleY - dayStartRotAngleY) / (86400 / 4)));
        Vector3 eulers = new Vector3(currRotAngleX, currRotAngleY, 0);
        lightObject.transform.rotation = Quaternion.Euler(eulers);

        #region Kommentar
        /*float temperatureValue = 0;
        temperatureValue += ((destinationTemperature - startTemperature) / (lengthOfDay * 60)) * Time.deltaTime;
        dayLight.colorTemperature += temperatureValue;

        float lIntensitiValue = 0;
        lIntensitiValue += ((destinationIntensityL - startIntensityL) / (lengthOfDay * 60)) * Time.deltaTime;
        lightData.intensity += lIntensitiValue;

        float vIntensitiValue = 0;
        vIntensitiValue += ((destinationIntensityV - startIntensityV) / (lengthOfDay * 60)) * Time.deltaTime;
        vIntensity.multiplier.value += vIntensitiValue;

        float speed = 180 / (lengthOfDay * 60);
        lightObject.transform.Rotate(new Vector3(1, 1, 0), speed * Time.deltaTime);

        if (lightData.intensity <= destinationIntensityL)
        {
            lightObject.transform.rotation = new Quaternion(0, 90, 0, 0);
        }*/
        #endregion
    }
    private void MiddayToEvening()
    {
        dayLight.colorTemperature = dayDestinationTemperature - ((currentDayTime - ((86400 / 4) * 2)) * ((dayDestinationTemperature - nightStartTemperature) / (86400 / 4)));
        lightData.intensity = dayDestinationIntensityL - ((currentDayTime - ((86400 / 4) * 2)) * ((dayDestinationIntensityL - nightStartIntensityL) / (86400 / 4)));
        vIntensity.multiplier.value = dayDestinationIntensityV - ((currentDayTime - ((86400 / 4) * 2)) * ((dayDestinationIntensityV - nightStartIntensityV) / (86400 / 4)));
        vol.weight = dayDestinationWeigh - ((currentDayTime - ((86400 / 4) * 2)) * ((dayDestinationWeigh - nightStartWeigh) / (86400 / 4)));

        float currRotAngleX = dayDestRotAngleX - ((currentDayTime - ((86400 / 4) * 2)) * ((dayDestRotAngleX - nightStartRotAngleX) / (86400 / 4)));
        float currRotAngleY = dayDestRotAngleY - ((currentDayTime - ((86400 / 4) * 2)) * ((dayDestRotAngleY - nightStartRotAngleY) / (86400 / 4)));
        Vector3 eulers = new Vector3(currRotAngleX, currRotAngleY, 0);
        lightObject.transform.rotation = Quaternion.Euler(eulers);

        #region Kommentar
        /*float temperatureValue = 0;
        temperatureValue += ((destinationTemperature - startTemperature) / (lengthOfDay * 60)) * Time.deltaTime;
        dayLight.colorTemperature += temperatureValue;

        float lIntensitiValue = 0;
        lIntensitiValue += ((destinationIntensityL - startIntensityL) / (lengthOfDay * 60)) * Time.deltaTime;
        lightData.intensity += lIntensitiValue;

        float vIntensitiValue = 0;
        vIntensitiValue += ((destinationIntensityV - startIntensityV) / (lengthOfDay * 60)) * Time.deltaTime;
        vIntensity.multiplier.value += vIntensitiValue;

        float speed = 180 / (lengthOfDay * 60);
        lightObject.transform.Rotate(new Vector3(1, 1, 0), speed * Time.deltaTime);

        if (lightData.intensity <= destinationIntensityL)
        {
            lightObject.transform.rotation = new Quaternion(0, 90, 0, 0);
        }*/
        #endregion
    }
    private void EveningToNight()
    {
        dayLight.colorTemperature = nightStartTemperature + ((currentDayTime - ((86400 / 4) * 3)) * ((nightDestinationTemperature - nightStartTemperature) / (86400 / 4)));
        lightData.intensity = nightStartIntensityL + ((currentDayTime - ((86400 / 4) * 3)) * ((nightDestinationIntensityL - nightStartIntensityL) / (86400 / 4)));
        vIntensity.multiplier.value = nightStartIntensityV + ((currentDayTime - ((86400 / 4) * 3)) * ((nightDestinationIntensityV - nightStartIntensityV) / (86400 / 4)));
        vol.weight = nightStartWeigh + ((currentDayTime - ((86400 / 4) * 3)) * ((nightDestinationWeigh - nightStartWeigh) / (86400 / 4)));

        float currRotAngleX = nightStartRotAngleX + ((currentDayTime - ((86400 / 4) * 3)) * ((nightDestRotAngleX - nightStartRotAngleX) / (86400 / 4)));
        float currRotAngleY = nightStartRotAngleY + ((currentDayTime - ((86400 / 4) * 3)) * ((nightDestRotAngleY - nightStartRotAngleY) / (86400 / 4)));
        Vector3 eulers = new Vector3(currRotAngleX, currRotAngleY, 0);
        lightObject.transform.rotation = Quaternion.Euler(eulers);
    }

    private void Update()
    {
        PassTime();
    }
}
