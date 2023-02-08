using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonBossStats : MonoBehaviour
{
    #region Properties
    public float CurrentHP
    {
        get { return currentHP; }
        set
        {
            if (invincible == false)
            {

                if (value <= 0)
                {
                    currentHP = 0;
                    if(KI.phase1)
                    {
                        ChangePhase();
                    }
                    else
                    {
                        Die();
                    }
                }

                if (currentHP - value > 0)
                {
                    currentHP = value;
                }

                if (value > currentHP)
                {
                    currentHP = value;
                }
                
                hpSlider.fillAmount = currentHP / maxHP;
            }
        }
    }
    #endregion

    [SerializeField] private SkeletonBossKI KI;
    [SerializeField] private Animator anim;
    [SerializeField] private float maxHP;
    [SerializeField] private float maxHPPhase2;
    [SerializeField] private float refillTime;

    private Image hpSlider;
    private float currentHP;
    private float t = 0;

    public bool invincible = false;
    
    private void Start()
    {
        hpSlider = GameObject.FindGameObjectWithTag("BossHpLeiste").GetComponent<Image>();
        CurrentHP = maxHP;
    }

    private void ChangePhase()
    {
        anim.SetTrigger("ActivateSecondPhase");



    }

    public void HealBoss()
    {
        while (CurrentHP < maxHPPhase2)
        {
            invincible = true;
            CurrentHP = Mathf.Lerp(CurrentHP, maxHPPhase2, t);
            t += refillTime * Time.deltaTime;
        }
        KI.phase1 = false;
        invincible = false;
    }

    private void Die()
    {
        invincible = true;
        anim.SetTrigger("Death");
    }
}
