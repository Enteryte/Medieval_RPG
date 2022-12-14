using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffManager : MonoBehaviour
{
    public static DebuffManager instance;

    public ThirdPersonController tPC;
    public PlayerValueManager pVM;

    [HideInInspector] public float loweredMaxStamina;
    [HideInInspector] public float loweredNormalStrength;

    public bool lowerStaminaDebuff = false;
    public bool slowPlayerDebuff = false;
    public bool lowerStrengthDebuff = false;
    public bool bleedingDebuff = false;

    public Coroutine lowerStaminaCoro;
    public Coroutine slowPlayerCoro;
    public Coroutine lowerStrengthCoro;
    public Coroutine bleedingCoro;

    public float bleedingTimes = 12;
    public float currBleedingTimes = 0;

    public int bleedingDamage = 0;

    public void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Bleeding();
        }
    }

    public void SlowPlayer()
    {
        //slowedMoveSpeed = tPC.MoveSpeed - speedMoveReduceAmount;

        slowPlayerDebuff = true;

        ThirdPersonController.instance.MoveSpeed = 0.7f;

        slowPlayerCoro = StartCoroutine(TimeTillSlowDebuffIsOver());
    }

    public void LowerMaxStamina(float staminaReduceAmount)
    {
        lowerStaminaDebuff = true;

        loweredMaxStamina = pVM.normalStamina - staminaReduceAmount;

        pVM.staminaSlider.maxValue = loweredMaxStamina;

        lowerStaminaCoro = StartCoroutine(TimeTillStaminaDebuffIsOver());
    }

    public void LowerStrength(float strengthReduceAmount)
    {
        lowerStrengthDebuff = true;

        loweredNormalStrength = pVM.normalStrength - strengthReduceAmount;

        tPC._animator.speed = 0.5f;

        lowerStrengthCoro = StartCoroutine(TimeTillLowerStrengthDebuffIsOver());
    }

    public void Bleeding()
    {
        bleedingDebuff = true;

        bleedingCoro = StartCoroutine(TimeTillBleedingDebuffIsOver());
    }

    IEnumerator TimeTillStaminaDebuffIsOver()
    {
        float timer = 0;

        while (timer < 15)
        {
            timer += Time.deltaTime;

            yield return null;
        }

        lowerStaminaDebuff = false;

        pVM.staminaSlider.maxValue = pVM.normalStamina;

        lowerStaminaCoro = null;
    }

    IEnumerator TimeTillSlowDebuffIsOver()
    {
        float timer = 0;

        while (timer < 15)
        {
            timer += Time.deltaTime;

            yield return null;
        }

        if (InventoryManager.instance.currHoldingWeight <= InventoryManager.instance.maxHoldingWeight)
        {
            tPC._animator.speed = 1;

            ThirdPersonController.instance.MoveSpeed = ThirdPersonController.instance.normalMoveSpeed;
        }

        slowPlayerDebuff = false;

        slowPlayerCoro = null;
    }

    IEnumerator TimeTillLowerStrengthDebuffIsOver()
    {
        float timer = 0;

        while (timer < 15)
        {
            timer += Time.deltaTime;

            yield return null;
        }

        lowerStrengthDebuff = false;

        lowerStrengthCoro = null;
    }

    IEnumerator TimeTillBleedingDebuffIsOver()
    {
        if (UseItemManager.instance.foodHealingCoro != null)
        {
            UseItemManager.instance.StopFoodHealingWhenGotDamage();
        }

        currBleedingTimes += 1;

        pVM.currHP -= bleedingDamage;
        pVM.healthSlider.value = pVM.currHP;

        yield return new WaitForSecondsRealtime(1);

        if (currBleedingTimes == bleedingTimes)
        {
            currBleedingTimes = 0;

            bleedingDebuff = false;

            bleedingCoro = null;
        }
        else
        {
            StartCoroutine(TimeTillBleedingDebuffIsOver());
        }
    }
}
