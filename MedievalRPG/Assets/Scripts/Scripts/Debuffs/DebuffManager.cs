using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// -----------------> WIP: Armor Debuff fehlt noch.
public class DebuffManager : MonoBehaviour
{
    public static DebuffManager instance;

    public ThirdPersonController tPC;
    public PlayerValueManager pVM;

    [HideInInspector] public float loweredMaxStamina;
    [HideInInspector] public float loweredNormalStrength;
    [HideInInspector] public float loweredNormalArmor;

    public bool lowerStaminaDebuff = false;
    public bool slowPlayerDebuff = false;
    public bool lowerStrengthDebuff = false;
    public bool lowerArmorDebuff = false;
    public bool bleedingDebuff = false;

    public Image lowerStaminaImg;
    public Image slowPlayerImg;
    public Image lowerStrengthImg;
    public Image lowerArmorImg;
    public Image bleedingImg;

    public Coroutine lowerStaminaCoro;
    public Coroutine slowPlayerCoro;
    public Coroutine lowerStrengthCoro;
    public Coroutine lowerArmorCoro;
    public Coroutine bleedingCoro;

    public float bleedingTimes = 12;
    public float currBleedingTimes = 0;

    public int bleedingDamage = 0;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            LowerMaxStamina(60);
            LowerStrength(2);
            Bleeding();
            SlowPlayer();
            LowerMaxArmor(40);
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

    public void LowerMaxArmor(float armorReduceAmount)
    {
        lowerArmorDebuff = true;

        loweredNormalArmor = pVM.normalArmor - armorReduceAmount;

        //pVM.staminaSlider.maxValue = loweredMaxStamina;

        pVM.currArmor = loweredNormalArmor;

        lowerArmorCoro = StartCoroutine(TimeTillArmorDebuffIsOver());
    }

    public void Bleeding()
    {
        bleedingDebuff = true;

        bleedingCoro = StartCoroutine(TimeTillBleedingDebuffIsOver());
    }

    IEnumerator TimeTillStaminaDebuffIsOver()
    {
        float timer = 0;

        lowerStaminaImg.gameObject.transform.parent.parent.gameObject.SetActive(true);

        while (timer < 15)
        {
            timer += Time.deltaTime;

            lowerStaminaImg.fillAmount = Mathf.Lerp(0, 1, timer / 15);

            yield return null;
        }

        lowerStaminaDebuff = false;

        pVM.staminaSlider.maxValue = pVM.normalStamina;

        lowerStaminaCoro = null;

        lowerStaminaImg.gameObject.transform.parent.parent.gameObject.SetActive(false);
    }

    IEnumerator TimeTillSlowDebuffIsOver()
    {
        float timer = 0;

        slowPlayerImg.gameObject.transform.parent.parent.gameObject.SetActive(true);

        while (timer < 15)
        {
            timer += Time.deltaTime;

            slowPlayerImg.fillAmount = Mathf.Lerp(0, 1, timer / 15);

            yield return null;
        }

        if (InventoryManager.instance.currHoldingWeight <= InventoryManager.instance.maxHoldingWeight)
        {
            tPC._animator.speed = 1;

            ThirdPersonController.instance.MoveSpeed = ThirdPersonController.instance.normalMoveSpeed;

            slowPlayerImg.gameObject.transform.parent.parent.gameObject.SetActive(false);
        }

        slowPlayerDebuff = false;

        slowPlayerCoro = null;
    }

    IEnumerator TimeTillLowerStrengthDebuffIsOver()
    {
        float timer = 0;

        lowerStrengthImg.gameObject.transform.parent.parent.gameObject.SetActive(true);

        while (timer < 15)
        {
            timer += Time.deltaTime;

            lowerStrengthImg.fillAmount = Mathf.Lerp(0, 1, timer / 15);

            yield return null;
        }

        lowerStrengthImg.gameObject.transform.parent.parent.gameObject.SetActive(false);

        lowerStrengthDebuff = false;

        lowerStrengthCoro = null;
    }

    IEnumerator TimeTillArmorDebuffIsOver()
    {
        float timer = 0;

        lowerArmorImg.gameObject.transform.parent.parent.gameObject.SetActive(true);

        while (timer < 15)
        {
            timer += Time.deltaTime;

            lowerArmorImg.fillAmount = Mathf.Lerp(0, 1, timer / 15);

            yield return null;
        }

        lowerArmorDebuff = false;

        //pVM.staminaSlider.maxValue = pVM.normalStamina;
        pVM.currArmor = pVM.normalArmor;

        lowerArmorCoro = null;

        lowerArmorImg.gameObject.transform.parent.parent.gameObject.SetActive(false);
    }

    IEnumerator TimeTillBleedingDebuffIsOver()
    {
        bleedingImg.gameObject.transform.parent.parent.gameObject.SetActive(true);

        if (UseItemManager.instance.foodHealingCoro != null)
        {
            UseItemManager.instance.StopFoodHealingWhenGotDamage();
        }

        bleedingImg.fillAmount += (1 / bleedingTimes);

        currBleedingTimes += 1;

        Debug.Log(pVM.CurrHP);
        pVM.CurrHP -= bleedingDamage;
        Debug.Log(pVM.CurrHP);
        pVM.healthSlider.value = pVM.CurrHP;

        yield return new WaitForSecondsRealtime(1);

        if (currBleedingTimes == bleedingTimes)
        {
            currBleedingTimes = 0;

            bleedingImg.gameObject.transform.parent.parent.gameObject.SetActive(false);

            bleedingDebuff = false;

            bleedingCoro = null;
        }
        else
        {
            StartCoroutine(TimeTillBleedingDebuffIsOver());
        }
    }

    public void StopAllDebuffs()
    {
        if (bleedingCoro != null)
        {
            StopCoroutine(bleedingCoro);
        }

        if (lowerArmorCoro != null)
        {
            StopCoroutine(lowerArmorCoro);
        }

        if (lowerStaminaCoro != null)
        {
            StopCoroutine(lowerStaminaCoro);
        }

        if (lowerStrengthCoro != null)
        {
            StopCoroutine(lowerStrengthCoro);
        }

        if (slowPlayerCoro != null && InventoryManager.instance.currHoldingWeight <= InventoryManager.instance.maxHoldingWeight)
        {
            StopCoroutine(slowPlayerCoro);
        }
    }
}
