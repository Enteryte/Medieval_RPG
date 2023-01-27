using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarHUDSlot : MonoBehaviour
{
    public string keyToPress;

    public HotbarSlotButton corrHSB;
    public ClickableInventorySlot corrInvCIS;

    public void Start()
    {
        corrHSB = this.gameObject.GetComponent<HotbarSlotButton>();

        //for (int i = 0; i < HotbarManager.instance.allHotbarSlotBtn.Length; i++)
        //{
        //    if (HotbarManager.instance.allHotbarSlotBtn[i].correspondingMainScreenHotbarSlotBtn == this.gameObject.GetComponent<ClickableInventorySlot>())
        //    {
        //        Debug.Log("GUTEN TAG");
        //        corrInvCIS = HotbarManager.instance.allHotbarSlotBtn[i];
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (corrInvCIS.storedItemBase != null && Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), keyToPress)) && !HotbarManager.instance.isUsingItem && ThirdPersonController.instance.canMove)
        {
            if (corrInvCIS.storedItemBase.itemType == ItemBaseProfile.ItemType.food)
            {
                Debug.Log("USEEEEEEEEEEEEEEEEEEEEE");
                UseItemManager.instance.UseFoodItem(corrInvCIS.storedItemBase);
                UseHotbarItem();
            }
            else if (corrInvCIS.storedItemBase.itemType == ItemBaseProfile.ItemType.potion)
            {
                if (corrInvCIS.storedItemBase.potionType == ItemBaseProfile.PotionType.healing)
                {
                    //PlayerValueManager.instance.CurrHP += corrInvCIS.storedItemBase.potionBuffValue;

                    UseItemManager.instance.UseHealPotion(corrInvCIS.storedItemBase);
                    UseHotbarItem();
                }
                else if (corrInvCIS.storedItemBase.potionType == ItemBaseProfile.PotionType.stamina)
                {
                    //PlayerValueManager.instance.currStamina += corrInvCIS.storedItemBase.potionBuffValue;

                    UseItemManager.instance.UseStaminaPotion(corrInvCIS.storedItemBase);
                    UseHotbarItem();
                }
                else if (corrInvCIS.storedItemBase.potionType == ItemBaseProfile.PotionType.speed)
                {
                    //PlayerValueManager.instance.currStamina += corrInvCIS.storedItemBase.potionBuffValue;

                    UseItemManager.instance.UseSpeedPotion(corrInvCIS.storedItemBase);
                    UseHotbarItem();
                }
                else if (corrInvCIS.storedItemBase.potionType == ItemBaseProfile.PotionType.strength)
                {
                    //PlayerValueManager.instance.currStamina += corrInvCIS.storedItemBase.potionBuffValue;

                    UseItemManager.instance.UseStrengthPotion(corrInvCIS.storedItemBase);
                    UseHotbarItem();
                }
                //else if (corrHSB.iBP.potionType == ItemBaseProfile.PotionType.)
                //{
                //    PlayerValueManager.instance.currStamina += corrHSB.iBP.potionBuffValue;
                //}
            }

            Debug.Log("GJHKLM111111111111");
        }
    }

    public void UseHotbarItem()
    {
        HotbarManager.instance.isUsingItem = true;
        ThirdPersonController.instance._animator.SetBool("UsingHBItem", true);

        ThirdPersonController.instance._animator.Play(HotbarManager.instance.drinkingAnim.ToString());

        corrInvCIS.storedAmount -= 1;
        corrInvCIS.storedAmountTxt.text = corrInvCIS.storedAmount.ToString();
        corrHSB.itemAmount = corrInvCIS.storedAmount;
        corrHSB.itemAmountTxt.text = corrInvCIS.storedAmount.ToString();

        for (int i = 0; i < InventoryManager.instance.inventory.slots.Count; i++)
        {
            if (InventoryManager.instance.inventory.slots[i].itemBase == corrHSB.iBP)
            {
                //if (InventoryManager.instance.inventory.slots[i].itemAmount - 1 <= 0)
                //{
                //    for (int i = 0; i < length; i++)
                //    {

                //    }
                //}

                InventoryManager.instance.inventory.slots[i].RemoveAmount(1);
            }
        }

        if (corrHSB.itemAmount == 0)
        {
            corrHSB.iBP = null;
            corrInvCIS.storedAmount = 0;

            corrInvCIS.ClearHotbarSlot();

            Debug.Log("TFZGUIJOKLMÖ;Ä:_'");
        }
    }
}
