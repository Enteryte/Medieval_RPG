using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseItemButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(UseItem);
    }

    public void UseItem()
    {
        if (InventoryManager.currIBP.itemType == ItemBaseProfile.ItemType.food)
        {
            // WIP: --> An dieser Stelle soll der Spieler geheilt werden.
        }
        else if (InventoryManager.currIBP.itemType == ItemBaseProfile.ItemType.weapon)
        {
            if (InventoryManager.currIBP.weaponType == ItemBaseProfile.WeaponType.sword)
            {
                EquippingManager.instance.leftWeaponES.newItemToEquip = InventoryManager.currIBP;
                EquippingManager.instance.leftWeaponES.ChangeEquippedItem();
            }
            else if (InventoryManager.currIBP.weaponType == ItemBaseProfile.WeaponType.bow)
            {
                EquippingManager.instance.bowES.newItemToEquip = InventoryManager.currIBP;
                EquippingManager.instance.bowES.ChangeEquippedItem();
            }
        }

        if (InventoryManager.currIS.itemAmount - 1 > 0)
        {
            InventoryManager.currIS.RemoveAmount(1);

            InventoryManager.instance.inventory.UpdateHotbarItems(InventoryManager.currIBP);
        }
        else
        {
            InventoryManager.instance.inventory.RemoveItem(InventoryManager.currIBP, 1);
        }

        InventoryManager.currIBP = null;
        InventoryManager.currIS = null;

        InventoryManager.instance.DisplayItemsOfCategory();
    }
}
