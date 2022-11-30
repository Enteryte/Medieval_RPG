using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FightingActions : MonoBehaviour
{
    public GameObject equippedWeapon;

    private DoDamage weaponScript;
    private Animator anim;
    private int attackCount = 0;
    private bool holdBlock = false;

    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        GetWeapon(); //Nur für testzwecke, später löschen
    }

    public void ResetBool()
    {
        attackCount = 0;
    }

    public void GetWeapon()
    {
        weaponScript = equippedWeapon.GetComponent<DoDamage>();
    }

    public void EnableDisableWeapon()
    {
        weaponScript.isActive = !weaponScript.isActive;
    }

    public void AttackedBool()
    {
        attackCount++;

        if(attackCount >= 4)
        { attackCount = 0; }
    }

    private void OnLightAttack()
    {
        if(equippedWeapon.CompareTag("SwordOnehanded"))
        {
            weaponScript.heavyAttack = false;
            weaponScript.lightAttack = true;
            anim.SetInteger("AttackCount", attackCount);
            anim.SetTrigger("LightAttackSword");
        }
    }

    private void OnHeavyAttackZoom()
    {
        if (equippedWeapon.CompareTag("SwordOnehanded"))
        {
            weaponScript.lightAttack = false;
            weaponScript.heavyAttack = true;
            anim.SetInteger("AttackCount", attackCount);
            anim.SetTrigger("HeavyAttackSword");
        }
    }

    private void OnBlockAim()
    {
        holdBlock = !holdBlock;
        anim.SetBool("HoldBlock", holdBlock);
    }

    private void OnZoomShoot()
    {

    }
}
