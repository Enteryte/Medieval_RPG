using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class InventoryCategoryButton : MonoBehaviour
{
    public ItemBaseProfile.ItemType itemTypeToDisplay;
    public string nameOfCategory;

    public void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(ChangeCurrentInvItemCategory);
    }

    public void ChangeCurrentInvItemCategory()
    {
        InventoryManager.currItemType = itemTypeToDisplay;
        InventoryManager.instance.currCategoryNameTxt.text = nameOfCategory;

        InventoryManager.instance.DisplayItemsOfCategory();
    }
}
