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

    [Header("Bools")]
    public bool higherSpeedBuff = false;
    public bool higherDamageBuff;

    public bool lowerStaminaDebuff = false;
    public bool slowPlayerDebuff = false;
    public bool lowerStrengthDebuff = false;
    public bool lowerArmorDebuff = false;
    public bool bleedingDebuff = false;

    [Header("Images")]
    public Image higherSpeedImg;
    public Image higherDamageImg;

    public Image lowerStaminaImg;
    public Image slowPlayerImg;
    public Image lowerStrengthImg;
    public Image lowerArmorImg;
    public Image bleedingImg;

    [Header("Coroutines")]
    public Coroutine higherSpeedCoro;
    public Coroutine higherDamageCoro;

    public Coroutine lowerStaminaCoro;
    public Coroutine slowPlayerCoro;
    public Coroutine lowerStrengthCoro;
    public Coroutine lowerArmorCoro;
    public Coroutine bleedingCoro;

    [Header("Other Values")]
    // Bleeding
    public float bleedingTimes = 12;
    public float currBleedingTimes = 0;

    public float bleedingDamage = 0;

    // Speed Buff
    public float currSBBuffTime;
    public float timerSB;

    // Damage Buff
    public float currDBBuffTime;
    public float timerDB;

    // Slow Debuff
    public float currSDDebuffTime;
    public float timerSD;

    // Stamina Debuff
    public float currStDDebuffTime;
    public float timerStD;

    public float currStaminaReduceAmount;

    // Strength Debuff
    public float currShDDebuffTime;
    public float timerShD;

    public float currStrengthReduceAmount;

    // Armor Debuff
    public float currADDebuffTime;
    public float timerAD;

    public float currArmorReduceAmount;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    LowerMaxStamina(60);
        //    LowerStrength(2);
        //    Bleeding();
        //    SlowPlayer();
        //    LowerMaxArmor(40);
        //}
    }

    public void SpeedUpPlayer(float buffTime, bool resetTimer)
    {
        higherSpeedBuff = true;

        if (resetTimer)
        {
            timerSB = 0;
        }

        higherSpeedCoro = StartCoroutine(TimeTillSpeedBuffIsOver(buffTime));
    }

    public void SetPlayerDamageHigher(float buffTime, bool resetTimer)
    {
        higherDamageBuff = true;

        if (resetTimer)
        {
            timerDB = 0;
        }

        higherDamageCoro = StartCoroutine(TimeTillDamageBuffIsOver(buffTime));
    }

    public void SlowPlayer(float debuffTime, bool resetTimer)
    {
        //slowedMoveSpeed = tPC.MoveSpeed - speedMoveReduceAmount;

        slowPlayerDebuff = true;

        if (resetTimer)
        {
            timerSD = 0;
        }

        ThirdPersonController.instance.MoveSpeed = 0.7f;

        slowPlayerCoro = StartCoroutine(TimeTillSlowDebuffIsOver(debuffTime));
    }

    public void LowerMaxStamina(float staminaReduceAmount, float debuffTime, bool resetTimer)
    {
        lowerStaminaDebuff = true;

        currStaminaReduceAmount = staminaReduceAmount;

        if (resetTimer)
        {
            timerStD = 0;
        }

        loweredMaxStamina = pVM.normalStamina - staminaReduceAmount;

        pVM.staminaSlider.maxValue = loweredMaxStamina;

        lowerStaminaCoro = StartCoroutine(TimeTillStaminaDebuffIsOver(debuffTime));
    }

    public void LowerStrength(float strengthReduceAmount, float debuffTime, bool resetTimer)
    {
        lowerStrengthDebuff = true;

        currStrengthReduceAmount = strengthReduceAmount;

        if (resetTimer)
        {
            timerShD = 0;
        }

        loweredNormalStrength = pVM.normalStrength - strengthReduceAmount;

        tPC._animator.speed = 0.5f;

        lowerStrengthCoro = StartCoroutine(TimeTillLowerStrengthDebuffIsOver(debuffTime));
    }

    public void LowerMaxArmor(float armorReduceAmount, float debuffTime, bool resetTimer)
    {
        lowerArmorDebuff = true;

        currArmorReduceAmount = armorReduceAmount;

        if (resetTimer)
        {
            timerAD = 0;
        }

        loweredNormalArmor = pVM.normalArmor - armorReduceAmount;

        //pVM.staminaSlider.maxValue = loweredMaxStamina;

        pVM.currArmor = loweredNormalArmor;

        lowerArmorCoro = StartCoroutine(TimeTillArmorDebuffIsOver(debuffTime));
    }

    public void Bleeding(float bleedingTimes, float bleedingDamage, bool resetTimer)
    {
        bleedingDebuff = true;

        if (resetTimer)
        {
            currBleedingTimes = 0;
        }

        bleedingCoro = StartCoroutine(TimeTillBleedingDebuffIsOver(bleedingTimes, bleedingDamage * DifficultyHandler.instance.bleedingMultiplier));
    }

    IEnumerator TimeTillSpeedBuffIsOver(float buffTime)
    {
        currSBBuffTime = buffTime;

        higherSpeedImg.gameObject.transform.parent.parent.gameObject.SetActive(true);

        while (timerSB < currSBBuffTime)
        {
            timerSB += Time.deltaTime;

            higherSpeedImg.fillAmount = Mathf.Lerp(0, 1, timerSB / currSBBuffTime);

            yield return null;
        }

        higherSpeedBuff = false;

        //pVM.staminaSlider.maxValue = pVM.normalStamina;

        higherSpeedCoro = null;

        higherSpeedImg.gameObject.transform.parent.parent.gameObject.SetActive(false);
    }

    IEnumerator TimeTillDamageBuffIsOver(float buffTime)
    {
        currDBBuffTime = buffTime;

        higherDamageImg.gameObject.transform.parent.parent.gameObject.SetActive(true);

        while (timerDB < currDBBuffTime)
        {
            currDBBuffTime += Time.deltaTime;

            higherDamageImg.fillAmount = Mathf.Lerp(0, 1, timerDB / currDBBuffTime);

            yield return null;
        }

        higherDamageBuff = false;

        //pVM.staminaSlider.maxValue = pVM.normalStamina;

        higherDamageCoro = null;

        higherDamageImg.gameObject.transform.parent.parent.gameObject.SetActive(false);
    }

    IEnumerator TimeTillStaminaDebuffIsOver(float debuffTime)
    {
        currStDDebuffTime = debuffTime;
 
        lowerStaminaImg.gameObject.transform.parent.parent.gameObject.SetActive(true);

        while (timerStD < currStDDebuffTime)
        {
            timerStD += Time.deltaTime;

            lowerStaminaImg.fillAmount = Mathf.Lerp(0, 1, timerStD / currStDDebuffTime);

            yield return null;
        }

        lowerStaminaDebuff = false;

        pVM.staminaSlider.maxValue = pVM.normalStamina;

        lowerStaminaCoro = null;

        lowerStaminaImg.gameObject.transform.parent.parent.gameObject.SetActive(false);
    }

    IEnumerator TimeTillSlowDebuffIsOver(float debuffTime)
    {
        currSDDebuffTime = debuffTime;

        slowPlayerImg.gameObject.transform.parent.parent.gameObject.SetActive(true);

        while (timerSD < currSDDebuffTime)
        {
            timerSD += Time.deltaTime;

            slowPlayerImg.fillAmount = Mathf.Lerp(0, 1, timerSD / currSDDebuffTime);

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

    IEnumerator TimeTillLowerStrengthDebuffIsOver(float debuffTime)
    {
        currShDDebuffTime = debuffTime;

        lowerStrengthImg.gameObject.transform.parent.parent.gameObject.SetActive(true);

        while (timerShD < currShDDebuffTime)
        {
            timerShD += Time.deltaTime;

            lowerStrengthImg.fillAmount = Mathf.Lerp(0, 1, timerShD / currShDDebuffTime);

            yield return null;
        }

        lowerStrengthImg.gameObject.transform.parent.parent.gameObject.SetActive(false);

        lowerStrengthDebuff = false;

        lowerStrengthCoro = null;
    }

    IEnumerator TimeTillArmorDebuffIsOver(float debuffTime)
    {
        currADDebuffTime = debuffTime;

        lowerArmorImg.gameObject.transform.parent.parent.gameObject.SetActive(true);

        while (timerAD < currADDebuffTime)
        {
            timerAD += Time.deltaTime;

            lowerArmorImg.fillAmount = Mathf.Lerp(0, 1, timerAD / currADDebuffTime);

            yield return null;
        }

        lowerArmorDebuff = false;

        //pVM.staminaSlider.maxValue = pVM.normalStamina;
        pVM.currArmor = pVM.normalArmor;

        lowerArmorCoro = null;

        lowerArmorImg.gameObject.transform.parent.parent.gameObject.SetActive(false);
    }

    IEnumerator TimeTillBleedingDebuffIsOver(float bleedingTimes, float bleedingDamage)
    {
        this.bleedingTimes = bleedingTimes;
        this.bleedingDamage = bleedingDamage;

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
            currBleedingTimes = 0;

            StartCoroutine(TimeTillBleedingDebuffIsOver(bleedingTimes, bleedingDamage));
        }
    }

    public void StopAllBuffs()
    {
        if (higherSpeedCoro != null)
        {
            StopCoroutine(higherSpeedCoro);
        }

        if (higherDamageCoro != null)
        {
            StopCoroutine(higherDamageCoro);
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
