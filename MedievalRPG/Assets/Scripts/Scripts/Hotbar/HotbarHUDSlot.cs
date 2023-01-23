using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarHUDSlot : MonoBehaviour
{
    public string keyToPress;

    public HotbarSlotButton corrHSB;

    public void Start()
    {
        corrHSB = this.gameObject.GetComponent<HotbarSlotButton>();
    }

    // Update is called once per frame
    void Update()
    {
        if (corrHSB.iBP != null && Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), keyToPress)) && HotbarManager.instance.isUsingItem && ThirdPersonController.instance.canMove)
        {
            if (corrHSB.iBP.itemType == ItemBaseProfile.ItemType.food)
            {
                UseItemManager.instance.UseFoodItem(corrHSB.iBP);
            }
            else if (corrHSB.iBP.itemType == ItemBaseProfile.ItemType.potion)
            {
                if (corrHSB.iBP.potionType == ItemBaseProfile.PotionType.healing)
                {
                    PlayerValueManager.instance.CurrHP += corrHSB.iBP.potionBuffValue;

                    UseHotbarItem();
                }
                else if (corrHSB.iBP.potionType == ItemBaseProfile.PotionType.stamina)
                {
                    PlayerValueManager.instance.currStamina += corrHSB.iBP.potionBuffValue;

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

        corrHSB.itemAmount -= 1;

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
    }
}
