using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GotDamage : MonoBehaviour
{
    private Animator anim;
    private readonly int Hit = Animator.StringToHash("GotHit");

    private void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
    }

    public void GotHit(bool enemyDamage)
    {
        Debug.Log($"Hit = {enemyDamage}");

        if(enemyDamage == true)
        {
            anim.SetTrigger(Hit);

            if(!TutorialManager.instance)
                return;
            if (EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase != null 
                && EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase.weaponType == ItemBaseProfile.WeaponType.shield)
            {
                TutorialManager.instance.CheckIfTutorialIsAlreadyCompleted(FightManager.instance.shildBlockTutorial);
            }
            else
            {
                TutorialManager.instance.CheckIfTutorialIsAlreadyCompleted(FightManager.instance.doARollTutorial);
            }
        }
    }
}
