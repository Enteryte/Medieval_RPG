using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerValueManager : MonoBehaviour
{
    public static PlayerValueManager instance;

    public float money;

    [Header("Health")]
    public float normalHP;
    public float currHP;
    public Slider healthSlider;

    [Header("Stamina")]
    public float normalStamina;
    public float currStamina;
    public Slider staminaSlider;

    public float timeTillRefillStamina;
    public float currWaitingTime;
    public float refillSpeed;

    [Header("Strength")]
    public float normalStrength;
    public float currStrength;

    public void Awake()
    {
        instance = this;

        staminaSlider.maxValue = normalStamina;
        staminaSlider.value = currStamina;

        healthSlider.maxValue = normalHP;
        healthSlider.value = currHP;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!DebuffManager.instance.lowerStaminaDebuff)
        {
            if (currStamina < normalStamina && currWaitingTime < timeTillRefillStamina)
            {
                currWaitingTime += Time.deltaTime;
            }
            else if (currWaitingTime >= timeTillRefillStamina)
            {
                staminaSlider.value += Mathf.Lerp(currStamina, normalStamina, Time.deltaTime * 1f);
                currStamina = staminaSlider.value;

                if (currStamina >= normalStamina)
                {
                    currWaitingTime = 0;
                }
            }
        }
        else
        {
            if (currStamina < DebuffManager.instance.loweredMaxStamina && currWaitingTime < timeTillRefillStamina)
            {
                currWaitingTime += Time.deltaTime;
            }
            else if (currWaitingTime >= timeTillRefillStamina)
            {
                staminaSlider.value += Mathf.Lerp(currStamina, DebuffManager.instance.loweredMaxStamina, Time.deltaTime * 1f);
                currStamina = staminaSlider.value;

                if (currStamina >= DebuffManager.instance.loweredMaxStamina)
                {
                    currWaitingTime = 0;
                }
            }
        }
    }

    public void RemoveStamina(float staminaAmountToRemove)
    {
        currStamina -= staminaAmountToRemove;
        staminaSlider.value = currStamina;

        currWaitingTime = 0;
    }
}
