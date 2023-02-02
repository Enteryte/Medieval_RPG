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
                hpSlider.fillAmount = currentHP / maxHP;

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
            }
        }
    }
    #endregion

    [SerializeField] private SkeletonBossKI KI;
    [SerializeField] private float maxHP;
    [SerializeField] private float maxHPPhase2;

    private Image hpSlider;
    private float currentHP;

    public bool invincible = false;
    
    private void Start()
    {
        hpSlider = GameObject.FindGameObjectWithTag("BossHpLeiste").GetComponent<Image>();
        CurrentHP = maxHP;
    }

    private void ChangePhase()
    {
        CurrentHP = maxHPPhase2;
    }

    private void Die()
    {
        invincible = true;
    }
}
