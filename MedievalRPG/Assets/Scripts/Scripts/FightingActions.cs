using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FightingActions : MonoBehaviour
{
    public static FightingActions instance;

    public GameObject equippedWeaponR;
    public GameObject equippedWeaponL;
    public GameObject arrow;
    public GameObject holdArrow;


    private StarterAssets.ThirdPersonController TPC;
    private DoDamage weaponScript;
    private Animator anim;
    private int attackCount = 0;
    private int chanceToRepeatIdle = 10; //chance die Idle zu wechseln 
    private bool holdBlock = false;
    private bool aims = true;

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        TPC = this.gameObject.GetComponent<StarterAssets.ThirdPersonController>();
        GetWeapon(); //Nur für testzwecke, später löschen
    }

    public void ResetBool()
    {
        attackCount = 0;
    }

    public void GetWeapon()
    {
        if (equippedWeaponR != null)
        {
            weaponScript = equippedWeaponR.GetComponent<DoDamage>();
        }

        if (equippedWeaponR != null && equippedWeaponR.CompareTag("GreatSword"))
        {
            OnEquipGreatsword();
        }
        if (equippedWeaponL != null && equippedWeaponL.CompareTag("Bow"))
        {
            OnEquipBow();
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

    private void OnEquipBow()
    {
        if (equippedWeaponR != null)
        {
            //De-Equip left Weapon
        }

        anim.SetTrigger("BowIdle");
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
        if (equippedWeaponR != null && equippedWeaponR.CompareTag("GreatSword"))
        {
            if (rand == 0)
            {
                anim.SetBool("GSIdleVar1", true);
                anim.SetBool("GSIdleVar2", false);
            }
            if (rand == 1)
            {
                anim.SetBool("GSIdleVar1", false);
                anim.SetBool("GSIdleVar2", true);
            }
            if (rand > 1)
            {
                anim.SetBool("GSIdleVar1", false);
                anim.SetBool("GSIdleVar2", false);
            }
        }
        if (equippedWeaponL != null && equippedWeaponL.CompareTag("Bow"))
        {
            if (rand == 0)
            {
                anim.SetTrigger("BowIdleVar1");
            }
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
        if (equippedWeaponR == null && equippedWeaponL == null)
        {
            return;
        }

        if (equippedWeaponL.CompareTag("Bow"))
        {
            TPC.HandleBowAimingCameras(TPC._bowAimingZoomVCamera, TPC._bowAimingVCamera, TPC._normalVCamera);
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
        if (equippedWeaponL.CompareTag("Bow"))
        {
            anim.SetTrigger("BowAim");
            aims = !aims;
            
            if(aims == false)
            {
                TPC.HandleBowAimingCameras(TPC._bowAimingVCamera, TPC._bowAimingZoomVCamera, TPC._normalVCamera);
                //holdArrow.SetActive(true);
            }
            if (aims == true)
            {
                TPC.HandleBowAimingCameras(TPC._normalVCamera, TPC._bowAimingZoomVCamera, TPC._bowAimingVCamera);
                //holdArrow.SetActive(false);
            }
        }
    }

    private void OnZoomShoot()
    {
        TPC.HandleBowAimingCameras(TPC._normalVCamera, TPC._bowAimingZoomVCamera, TPC._bowAimingVCamera);
        Instantiate(arrow);
    }
}
