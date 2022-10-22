using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseItemManager : MonoBehaviour
{
    public static UseItemManager instance;

    public ItemBaseProfile test;

    public void Awake()
    {
        instance = this; 
    }

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    UseHealPotion(test);
        //}
    }

    public void UseEscapeRope()
    {
        RespawnManager.instance.RespawnPlayer(RespawnManager.instance.usedEscapeRopeSpawnTrans);
    }

    public void UseFoodItem(ItemBaseProfile iBP) // Slow hp reg
    {

    }

    public void UseHealPotion(ItemBaseProfile iBP) // Instant hp reg
    {
        PlayerValueManager.instance.healthSlider.maxValue = PlayerValueManager.instance.normalHP;
        PlayerValueManager.instance.healthSlider.value = PlayerValueManager.instance.currHP;

        PlayerValueManager.instance.healthSlider.value += iBP.potionBuffValue;
        PlayerValueManager.instance.currHP = PlayerValueManager.instance.healthSlider.value;
    }

    public void UseStaminaPotion(ItemBaseProfile iBP) // Instant stamina reg
    {
        PlayerValueManager.instance.staminaSlider.maxValue = PlayerValueManager.instance.normalStamina;
        PlayerValueManager.instance.staminaSlider.value = PlayerValueManager.instance.currStamina;

        PlayerValueManager.instance.staminaSlider.value += iBP.potionBuffValue;
        PlayerValueManager.instance.currStamina = PlayerValueManager.instance.staminaSlider.value;
    }

    public void UseSpeedPotion(ItemBaseProfile iBP)
    {

    }

    public void UseStrengthPotion(ItemBaseProfile iBP)
    {

    }
}
