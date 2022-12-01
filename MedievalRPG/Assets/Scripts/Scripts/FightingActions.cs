using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FightingActions : MonoBehaviour
{
    public static FightingActions instance;

    public GameObject equippedWeaponR;
    public GameObject equippedWeaponL;

    private DoDamage weaponScript;
    private Animator anim;
    private int attackCount = 0;
    private bool holdBlock = false;

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        //GetWeapon(); //Nur für testzwecke, später löschen
    }

    public void ResetBool()
    {
        attackCount = 0;
    }

    public void GetWeapon()
    {
        weaponScript = equippedWeaponR.GetComponent<DoDamage>();
    }

    public void EnableDisableWeapon()
    {
        weaponScript.isActive = !weaponScript.isActive;
    }

    public void AttackedBool()
    {
        attackCount++;
    }

    private void OnLightAttack()
    {
        if (equippedWeaponR == null)
        {
            return;
        }

        if(equippedWeaponR.CompareTag("SwordOnehanded"))
        {
            weaponScript.heavyAttack = false;
            weaponScript.lightAttack = true;
            anim.SetInteger("AttackCount", attackCount);
            anim.SetTrigger("LightAttackSword");
        }
        //if (equippedWeaponR.CompareTag("Axe"))
        //{
        //    weaponScript.heavyAttack = false;
        //    weaponScript.lightAttack = true;
        //    anim.SetTrigger("LightAttackAxe");
        //}
        //Debug.Log(attackCount);
    }

    private void OnHeavyAttackZoom()
    {
        if (equippedWeaponR == null)
        {
            return;
        }

        if (equippedWeaponR.CompareTag("SwordOnehanded"))
        {
            weaponScript.lightAttack = false;
            weaponScript.heavyAttack = true;
            anim.SetInteger("AttackCount", attackCount);
            anim.SetTrigger("HeavyAttackSword");
        }
        //if (equippedWeaponR.CompareTag("Axe"))
        //{
        //    weaponScript.lightAttack = false;
        //    weaponScript.heavyAttack = true;
        //    anim.SetInteger("AttackCount", attackCount);
        //    anim.SetTrigger("HeavyAttackAxe");
        //}
        //Debug.Log(attackCount);
    }

    private void OnBlockAimTorch()
    {
        if (equippedWeaponL == null)
        {
            return;
        }

        if (equippedWeaponL.CompareTag("Shield"))
        {
            holdBlock = !holdBlock;
            anim.SetBool("HoldBlock", holdBlock);
        }
        if(equippedWeaponL.CompareTag("Torch"))
        {
            anim.SetTrigger("AttackTorch");
        }
    }

    private void OnZoomShoot()
    {

    }
}
