using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseItemManager : MonoBehaviour
{
    public static UseItemManager instance;

    public ThirdPersonController tPC;
    public PlayerValueManager pVM;

    public ItemBaseProfile test;

    public bool ateFood = false;
    public bool hasStrengthBuff = false;
    public bool hasSpeedBuff = false;

    public float higherStrengthValue;
    public float higherSpeedValue;

    public Coroutine foodHealingCoro;
    public Coroutine strengthBuffCoro;
    public Coroutine speedBuffCoro;

    public void Awake()
    {
        instance = this; 
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            UseBookOrNote(test);
        }

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

    public void UseBookOrNote(ItemBaseProfile iBP)
    {
        if (MissionManager.instance.allCurrAcceptedMissions.Count > 1)
        {
            for (int i = 0; i < MissionManager.instance.allCurrAcceptedMissions.Count; i++)
            {
                if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks.Length > 1)
                {
                    for (int y = 0; y < MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks.Length; y++)
                    {
                        if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.missionTaskType == MissionTaskBase.MissionTaskType.read
                            && MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.noteOrBookToReadIBP == iBP)
                        {
                            MissionManager.instance.CompleteMissionTask(MissionManager.instance.allCurrAcceptedMissions[i], MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB);
                        }
                    }
                }
                else
                {
                    if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.missionTaskType == MissionTaskBase.MissionTaskType.read
                            && MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.noteOrBookToReadIBP == iBP)
                    {
                        MissionManager.instance.CompleteMissionTask(MissionManager.instance.allCurrAcceptedMissions[i], MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB);
                    }
                }
            }
        }
        else
        {
            if (MissionManager.instance.allCurrAcceptedMissions[0].allMissionTasks.Length > 1)
            {
                for (int y = 0; y < MissionManager.instance.allCurrAcceptedMissions[0].allMissionTasks.Length; y++)
                {
                    if (MissionManager.instance.allCurrAcceptedMissions[0].allMissionTasks[y].mTB.missionTaskType == MissionTaskBase.MissionTaskType.read
                        && MissionManager.instance.allCurrAcceptedMissions[0].allMissionTasks[y].mTB.noteOrBookToReadIBP == iBP)
                    {
                        MissionManager.instance.CompleteMissionTask(MissionManager.instance.allCurrAcceptedMissions[0], MissionManager.instance.allCurrAcceptedMissions[0].allMissionTasks[y].mTB);
                    }
                }
            }
            else
            {
                if (MissionManager.instance.allCurrAcceptedMissions[0].allMissionTasks[0].mTB.missionTaskType == MissionTaskBase.MissionTaskType.read
                        && MissionManager.instance.allCurrAcceptedMissions[0].allMissionTasks[0].mTB.noteOrBookToReadIBP == iBP)
                {
                    MissionManager.instance.CompleteMissionTask(MissionManager.instance.allCurrAcceptedMissions[0], MissionManager.instance.allCurrAcceptedMissions[0].allMissionTasks[0].mTB);
                }
            }
        }

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
        PlayerValueManager.instance.healthSlider.value = PlayerValueManager.instance.CurrHP;

        PlayerValueManager.instance.healthSlider.value += iBP.potionBuffValue;
        PlayerValueManager.instance.CurrHP = PlayerValueManager.instance.healthSlider.value;
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
        if (!hasSpeedBuff)
        {
            hasSpeedBuff = true;

            speedBuffCoro = StartCoroutine(TimeTillSpeedBuffIsOver(iBP));
        }
    }

    public void UseStrengthPotion(ItemBaseProfile iBP)
    {
        if (!hasStrengthBuff)
        {
            hasStrengthBuff = true;

            strengthBuffCoro = StartCoroutine(TimeTillStrengthBuffIsOver(iBP));
        }
    }

    IEnumerator TimeTillFoodHealed(ItemBaseProfile iBP) // ! -> Könnte sein, dass es auch weiterheilt, wenn der Spieler Damage bekommt. Ggf. sobald er DMG bekommt -> Coroutine abbrechen und Heilung beenden.
    {
        var time = 0f;
        float startHealth = pVM.CurrHP;

        while (pVM.CurrHP < startHealth + iBP.foodHealValue)
        {
            time += Time.deltaTime / 7;
            pVM.CurrHP = Mathf.Lerp(startHealth, startHealth + iBP.foodHealValue, time);
            pVM.healthSlider.value = pVM.CurrHP;

            yield return null;
        }

        if (pVM.CurrHP > pVM.normalHP)
        {
            pVM.CurrHP = pVM.normalHP;
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

    IEnumerator TimeTillStrengthBuffIsOver(ItemBaseProfile iBP)
    {
        higherStrengthValue = PlayerValueManager.instance.normalStrength + iBP.potionBuffValue;

        yield return new WaitForSecondsRealtime(10);

        hasStrengthBuff = false;

        strengthBuffCoro = null;
    }

    IEnumerator TimeTillSpeedBuffIsOver(ItemBaseProfile iBP)
    {
        tPC._animator.speed = 1.5f;

        yield return new WaitForSecondsRealtime(10);

        tPC._animator.speed = 1f;

        hasSpeedBuff = false;

        speedBuffCoro = null;
    }
}
