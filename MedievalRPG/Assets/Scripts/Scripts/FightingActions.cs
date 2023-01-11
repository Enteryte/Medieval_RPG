using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FightingActions : MonoBehaviour
{
    public static FightingActions instance;

    public List<Collider> colls;
    public static GameObject equippedWeaponR;
    public static GameObject equippedWeaponL;
    public GameObject arrow;
    public GameObject holdArrow;
    public GameObject stone;
    public Collider foot;
    public int shotSpeed = 6;
    public int throwSpeed = 10;
    public int rollSpeed = 10;
    public bool aims = false;
    public bool holdBlock = false;

    private StarterAssets.ThirdPersonController TPC;
    private DoDamage weaponScriptR;
    private DoDamage weaponScriptL;
    public Animator anim;
    private int attackCount = 0;
    private int chanceToRepeatIdle = 10; //chance die Idle zu wechseln 
    private float time;
    private bool isRolling = false;

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        TPC = this.gameObject.GetComponent<StarterAssets.ThirdPersonController>();
        GetWeapon(); 
    }

    #region WeaponHandling

    public void GetWeapon()
    {
        if (equippedWeaponR != null)
        {
            weaponScriptR = equippedWeaponR.GetComponent<DoDamage>();
        }
        if (equippedWeaponL != null && !equippedWeaponL.CompareTag("Torch"))
        {
            weaponScriptL = equippedWeaponL.GetComponent<DoDamage>();
        }

        if (equippedWeaponR != null && equippedWeaponR.CompareTag("GreatSword"))
        {
            OnEquipGreatsword();
        }
        if (equippedWeaponL != null && equippedWeaponL.CompareTag("Bow"))
        {
            OnEquipBow();
        }

        Debug.Log(equippedWeaponR);
        Debug.Log(equippedWeaponL);
    }

    private void OnEquipBow()
    {
        if (equippedWeaponR != null)
        {
            //FABIENNE: De-Equip left Weapon

            anim.SetTrigger("DeequipGreatSword");
            EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().ClearEquipmentSlot();
        }

        anim.SetTrigger("BowIdle");
    }

    private void OnEquipGreatsword()
    {
        if (equippedWeaponL != null)
        {
            //FABIENNE: De-Equip left Weapon

            anim.SetTrigger("DeequipBow");
            EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().ClearEquipmentSlot();
        }

        anim.SetTrigger("GreatSwordIdle");
    }
    #endregion

    #region AnimEvents
    public void ToggleInvincibility()
    {
        PlayerValueManager.instance.invincible = !PlayerValueManager.instance.invincible;
    }

    public void EnableDisableUnarmed()
    {
        for (int i = 0; i < colls.Count; i++)
        {
            colls[i].enabled = !colls[i].enabled;
        }
    }

    public void StopMOVING()
    {
        TPC.StopMovingWhileRolling();
    }

    public void ResetBool()
    {
        attackCount = 0;
    }

    public void EnableDisableFoot()
    {
        foot.enabled = !foot.enabled;
    }

    public void EnableDisableTorch()
    {
        equippedWeaponL.GetComponent<Collider>().enabled = !equippedWeaponL.GetComponent<Collider>().enabled;
    }

    public void AllowStateChange()
    {
        anim.SetBool("MayChange", !anim.GetBool("MayChange"));
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
    
    public void EnableDisableWeapon()
    {
        if (weaponScriptR != null)
        {
            weaponScriptR.isActive = !weaponScriptR.isActive;
        }

        if (weaponScriptL != null)
        {
            weaponScriptL.isActive = !weaponScriptL.isActive;

            if (weaponScriptL.gameObject.CompareTag("Shield"))
            {
                weaponScriptL.gameObject.GetComponent<BoxCollider>().isTrigger = !weaponScriptL.gameObject.GetComponent<BoxCollider>().isTrigger;
            }
        }
    }

    public void EnableDisableShieldCollider()
    {
        weaponScriptL.gameObject.GetComponent<BoxCollider>().enabled = !weaponScriptL.gameObject.GetComponent<BoxCollider>().enabled;
    }

    public void AttackedBool()
    {
        attackCount++;
    }
    #endregion

    #region Attacks
    private void OnLightAttackShoot()
    {
        if (equippedWeaponR == null && equippedWeaponL == null)
        {
            anim.SetTrigger("LightAttackFist");
            return;
        }

        if(equippedWeaponL != null)
        {
            if (equippedWeaponL.CompareTag("Bow") && aims == true)
            {
                anim.SetTrigger("Shoot");
                TPC.HandleBowAimingCameras(TPC._normalVCamera, TPC._bowAimingZoomVCamera, TPC._bowAimingVCamera);
                GameObject Arrow = Instantiate(arrow);
                Arrow.transform.position = equippedWeaponL.transform.position;
                Arrow.transform.rotation = Quaternion.LookRotation(transform.forward, new Vector3(0,0,0));
                Arrow.GetComponent<Rigidbody>().AddForce(transform.forward * shotSpeed, ForceMode.Impulse);
                Arrow = null;
                aims = !aims;
                TPC.transform.rotation = Quaternion.Euler(0, TPC.transform.rotation.eulerAngles.y, 0);

                //FABIENNE: Pfeile aus inventar entfernen
            }
            if(equippedWeaponL.CompareTag("Shield") && holdBlock == true)
            {
                weaponScriptL.heavyAttack = false;
                weaponScriptL.lightAttack = true;
                anim.SetTrigger("ShieldSmack");
            }
        }

        if(equippedWeaponR != null)
        {
            //FABIENNE: Stamina loss bei Angriffen
            if (equippedWeaponR.CompareTag("SwordOnehanded") && holdBlock == false)
            {
                weaponScriptR.heavyAttack = false;
                weaponScriptR.lightAttack = true;
                anim.SetInteger("AttackCount", attackCount);
                anim.SetTrigger("LightAttackSword");
            }
            if (equippedWeaponR.CompareTag("Axe"))
            {
                weaponScriptR.heavyAttack = false;
                weaponScriptR.lightAttack = true;
                anim.SetInteger("AttackCount", attackCount);
                anim.SetTrigger("LightAttackAxe");
            }
            if (equippedWeaponR.CompareTag("GreatSword"))
            {
                weaponScriptR.heavyAttack = false;
                weaponScriptR.lightAttack = true;
                anim.SetTrigger("GreatSwordKick");
            }
            if (equippedWeaponR.CompareTag("Club"))
            {
                weaponScriptR.heavyAttack = false;
                weaponScriptR.lightAttack = true;
                anim.SetTrigger("ClubAttack");
            }
            if(equippedWeaponR.CompareTag("Stone"))
            {
                anim.SetTrigger("ThrowStone");
            }
        }
    }

    private void OnHeavyAttackZoom()
    {
        if (equippedWeaponR == null && equippedWeaponL == null)
        {
            anim.SetTrigger("HeavyAttackFist");
            return;
        }

        if (equippedWeaponL != null && equippedWeaponL.CompareTag("Bow"))
        {
            TPC.HandleBowAimingCameras(TPC._bowAimingZoomVCamera, TPC._bowAimingVCamera, TPC._normalVCamera);
        }

        //FABIENNE: Stamina loss bei Angriffen
        if (equippedWeaponR.CompareTag("SwordOnehanded"))
        {
            weaponScriptR.lightAttack = false;
            weaponScriptR.heavyAttack = true;
            anim.SetInteger("AttackCount", attackCount);
            anim.SetTrigger("HeavyAttackSword");
        }
        if (equippedWeaponR.CompareTag("Axe"))
        {
            weaponScriptR.lightAttack = false;
            weaponScriptR.heavyAttack = true;
            anim.SetInteger("AttackCount", attackCount);
            anim.SetTrigger("HeavyAttackAxe");
        }
        if (equippedWeaponR.CompareTag("GreatSword"))
        {
            weaponScriptR.lightAttack = false;
            weaponScriptR.heavyAttack = true;
            anim.SetTrigger("GreatSwordSlash");
        }
        if (equippedWeaponR.CompareTag("Club"))
        {
            weaponScriptR.heavyAttack = true;
            weaponScriptR.lightAttack = false;
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
            TPC.canMove = !holdBlock;
            anim.SetBool("HoldBlock", holdBlock);
            //FABIENNE: Stamina ziehen
        }
        if (equippedWeaponL.CompareTag("Torch"))
        {
            anim.SetTrigger("AttackTorch");
        }
        if (equippedWeaponL.CompareTag("Bow"))
        {
            anim.SetTrigger("BowAim");
            
            if(aims == false)
            {
                TPC.HandleBowAimingCameras(TPC._bowAimingVCamera, TPC._bowAimingZoomVCamera, TPC._normalVCamera);
                //holdArrow.SetActive(true);
            }
            if (aims == true)
            {
                TPC.transform.rotation = Quaternion.Euler(0, TPC.transform.rotation.eulerAngles.y, 0);
                TPC.HandleBowAimingCameras(TPC._normalVCamera, TPC._bowAimingZoomVCamera, TPC._bowAimingVCamera);
                //holdArrow.SetActive(false);
            }
            
            aims = !aims;
        }
    }

    public void ThrowStone()
    {
        GameObject Stone = Instantiate(stone);
        stone.transform.position = equippedWeaponR.transform.position;
        equippedWeaponR.gameObject.SetActive(false);
        Stone.GetComponent<Rigidbody>().AddForce(Stone.transform.forward * throwSpeed, ForceMode.Impulse);

        equippedWeaponR = null;
    }
    #endregion

    #region Miscellaneous
     private void OnRoll()
    {
        StartCoroutine(GameManager.instance.playerGO.GetComponent<ThirdPersonController>().Roll());
        anim.SetTrigger("RollNew");
    }

    private void Update()
    {
        if(isRolling)
        {
            time += Time.deltaTime;
            float desiredDuration = time / rollSpeed;
            this.transform.position = Vector3.Lerp(this.transform.position, transform.forward * 3.16f, desiredDuration);
        }
    }

    #endregion

}
