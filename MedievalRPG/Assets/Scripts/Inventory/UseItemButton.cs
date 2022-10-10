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

        Debug.Log(InventoryManager.currIS.itemBase.name);

        if (InventoryManager.currIS.itemAmount - 1 > 0)
        {
            InventoryManager.currIS.RemoveAmount(1);
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
