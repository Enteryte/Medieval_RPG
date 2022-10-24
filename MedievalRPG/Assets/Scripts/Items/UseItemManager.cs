using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseItemManager : MonoBehaviour
{
    public static UseItemManager instance;

    public PlayerValueManager pVM;

    public ItemBaseProfile test;

    public bool ateFood = false;

    public Coroutine foodHealingCoro;

    public void Awake()
    {
        instance = this; 
    }

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    UseFoodItem(test);
        //}

        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    StopFoodHealingWhenGotDamage();
        //}

        //if (pVM.currHP < pVM.normalHP && ateFood)
        //{
        //    currWaitingTime += Time.deltaTime * eatenFoodAmount;
        //}
        //else if (currWaitingTime >= timeTillRefillStamina)
        //{
        //    staminaSlider.value += Mathf.Lerp(currStamina, normalStamina, Time.deltaTime * 1f);
        //    currStamina = staminaSlider.value;

        //    if (currStamina >= normalStamina)
        //    {
        //        currWaitingTime = 0;
        //    }
        //}
    }

    public void UseEscapeRope()
    {
        RespawnManager.instance.RespawnPlayer(RespawnManager.instance.usedEscapeRopeSpawnTrans);
    }

    public void UseFoodItem(ItemBaseProfile iBP) // Slow hp reg
    {
        if (!ateFood)
        {
            ateFood = true;

            foodHealingCoro = StartCoroutine(TimeTillFoodHealed(iBP));
        }
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

    IEnumerator TimeTillFoodHealed(ItemBaseProfile iBP) // ! -> Könnte sein, dass es auch weiterheilt, wenn der Spieler Damage bekommt. Ggf. sobald er DMG bekommt -> Coroutine abbrechen und Heilung beenden.
    {
        var time = 0f;
        float startHealth = pVM.currHP;

        while (pVM.currHP < startHealth + iBP.foodHealValue)
        {
            time += Time.deltaTime / 7;
            pVM.currHP = Mathf.Lerp(startHealth, startHealth + iBP.foodHealValue, time);
            pVM.healthSlider.value = pVM.currHP;

            yield return null;
        }

        if (pVM.currHP > pVM.normalHP)
        {
            pVM.currHP = pVM.normalHP;
        }

        ateFood = false;

        foodHealingCoro = null;
    }

    public void StopFoodHealingWhenGotDamage()
    {
        StopCoroutine(foodHealingCoro);

        ateFood = false;

        foodHealingCoro = null;
    }
}
