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
    private int chanceToRepeatIdle = 10; //chance von 2 zu n die Idle zu wechseln 
    public void Awake()
    {
        instance = this;
    }

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
        weaponScript = equippedWeaponR.GetComponent<DoDamage>();

        if(equippedWeaponR.CompareTag("GreatSword"))
        {
            OnEquipGreatsword();
        }
    }

    public void EnableDisableWeapon()
    {
        weaponScript.isActive = !weaponScript.isActive;
    }

    public void AttackedBool()
    {
        attackCount++;
    }

    private void OnEquipGreatsword()
    {
        if (equippedWeaponL != null)
        {
            //De-Equip left Weapon
        }

        anim.SetTrigger("GreatSwordIdle");
    }

    public void OnIdleChange()
    {
        int rand = Random.Range(0, chanceToRepeatIdle);
        if (rand == 0)
        {
            anim.SetBool("GSIdleVar1", true);
            anim.SetBool("GSIdleVar2", false);
        }
        if(rand == 1)
        {
            anim.SetBool("GSIdleVar1", false);
            anim.SetBool("GSIdleVar2", true);
        }
        if(rand > 1)
        {
            anim.SetBool("GSIdleVar1", false); 
            anim.SetBool("GSIdleVar2", false);
        }
    }

    public void AllowStateChange()
    {
        anim.SetBool("MayChange", !anim.GetBool("MayChange"));
    }

    private void OnLightAttack()
    {
        if (equippedWeaponR == null)
        {
            return;
        }

        if (equippedWeaponR.CompareTag("SwordOnehanded"))
        {
            weaponScript.heavyAttack = false;
            weaponScript.lightAttack = true;
            anim.SetInteger("AttackCount", attackCount);
            anim.SetTrigger("LightAttackSword");
        }
        if (equippedWeaponR.CompareTag("Axe"))
        {
            weaponScript.heavyAttack = false;
            weaponScript.lightAttack = true;
            anim.SetInteger("AttackCount", attackCount);
            anim.SetTrigger("LightAttackAxe");
        }
        if (equippedWeaponR.CompareTag("GreatSword"))
        {
            weaponScript.heavyAttack = false;
            weaponScript.lightAttack = true;
            anim.SetTrigger("GreatSwordKick");
        }
        if (equippedWeaponR.CompareTag("Club"))
        {
            weaponScript.heavyAttack = false;
            weaponScript.lightAttack = true;
            anim.SetTrigger("ClubAttack");
        }
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
        if (equippedWeaponR.CompareTag("Axe"))
        {
            weaponScript.lightAttack = false;
            weaponScript.heavyAttack = true;
            anim.SetInteger("AttackCount", attackCount);
            anim.SetTrigger("HeavyAttackAxe");
        }
        if (equippedWeaponR.CompareTag("GreatSword"))
        {
            weaponScript.lightAttack = false;
            weaponScript.heavyAttack = true;
            anim.SetTrigger("GreatSwordSlash");
        }
        if (equippedWeaponR.CompareTag("Club"))
        {
            weaponScript.heavyAttack = true;
            weaponScript.lightAttack = false;
            anim.SetTrigger("ClubAttack");
        }
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
        if (equippedWeaponL.CompareTag("Torch"))
        {
            anim.SetTrigger("AttackTorch");
        }
    }

    private void OnZoomShoot()
    {

    }
}
