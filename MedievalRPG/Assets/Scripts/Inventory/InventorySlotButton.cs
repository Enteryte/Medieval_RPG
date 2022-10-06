using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InventorySlotButton : MonoBehaviour
{
    public ItemBaseProfile storedItemBase;

    public TMP_Text itemNameTxt;
    public GameObject itemIsNewSymbolGO;

    public void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(DisplayAllItemInformationsOnClick);
    }

    public void DisplayItemInformations()
    {
        itemNameTxt.text = storedItemBase.itemName;

        itemIsNewSymbolGO.SetActive(storedItemBase.isNew);
    }

    public void DisplayAllItemInformationsOnClick()
    {
        Debug.Log(this.gameObject.name);

        if (storedItemBase.isNew)
        {
            storedItemBase.isNew = false;
            itemIsNewSymbolGO.SetActive(false);
        }

        InventoryManager.instance.currItemNameTxt.text = storedItemBase.itemName;
        InventoryManager.instance.currItemDescriptionTxt.text = storedItemBase.itemDescription;
        InventoryManager.instance.currItemAmountInInvTxt.text = storedItemBase.amountInInventory.ToString();
        InventoryManager.instance.currItemSellPriceTxt.text = storedItemBase.sellingPrice.ToString();

        InventoryManager.currIBP = storedItemBase;
    }

    public void CheckIfSlotHasToBeDestroyed()
    {
        if (storedItemBase.amountInInventory <= 0)
        {
            InventoryManager.instance.DisplayItemsOfCategory();
        }
    }
}
