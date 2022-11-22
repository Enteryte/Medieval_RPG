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

    [HideInInspector] public InventorySlot invSlot;

    public void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(DisplayAllItemInformationsOnClick);
    }

    public void Update()
    {
        //if (invSlot.itemAmount <= 0)
        //{
        //    Destroy(this.gameObject);

        //    InventoryManager.instance.DisplayItemsOfCategory();
        //}
    }

    public void DisplayItemInformations()
    {
        itemNameTxt.text = storedItemBase.itemName;

        itemIsNewSymbolGO.SetActive(storedItemBase.isNew);
    }

    public void DisplayAllItemInformationsOnClick()
    {
        if (storedItemBase.isNew)
        {
            storedItemBase.isNew = false;
            itemIsNewSymbolGO.SetActive(false);
        }

        InventoryManager.instance.currItemNameTxt.text = storedItemBase.itemName;
        InventoryManager.instance.currItemDescriptionTxt.text = storedItemBase.itemDescription;
        InventoryManager.instance.currItemAmountInInvTxt.text = invSlot.itemAmount.ToString();
        InventoryManager.instance.currItemSellPriceTxt.text = storedItemBase.sellingPrice.ToString();

        InventoryManager.currIBP = storedItemBase;
        InventoryManager.currIS = invSlot;

        if (!storedItemBase.neededForMissions)
        {
            if (storedItemBase.itemType == ItemBaseProfile.ItemType.food || storedItemBase.itemType == ItemBaseProfile.ItemType.weapon 
                || storedItemBase.itemType == ItemBaseProfile.ItemType.bookOrNote)
            {
                InventoryManager.instance.useItemButton.SetActive(true);
            }
            else
            {
                InventoryManager.instance.useItemButton.SetActive(false);
            }
        }
        else
        {
            InventoryManager.instance.useItemButton.SetActive(false);
        }

        for (int i = 0; i < InventoryManager.instance.invItemPreviewCamTrans.childCount; i++)
        {
            Destroy(InventoryManager.instance.invItemPreviewCamTrans.GetChild(i).gameObject);
        }

        GameObject newPreviewItem = Instantiate(storedItemBase.itemPrefab, Vector3.zero, Quaternion.Euler(0, 0, 2f), InventoryManager.instance.invItemPreviewCamTrans);
        newPreviewItem.AddComponent<PreviewItem>();

        newPreviewItem.transform.localPosition = new Vector3(0, 0, storedItemBase.previewSpawnPositionZ);

        newPreviewItem.layer = LayerMask.NameToLayer("PreviewItem");
    }

    public void CheckIfSlotHasToBeDestroyed()
    {
        if (invSlot.itemAmount <= 0)
        {
            InventoryManager.instance.DisplayItemsOfCategory();
        }
    }

    public void InstantiateDraggableCopy()
    {
        if (storedItemBase.itemType != ItemBaseProfile.ItemType.weapon)
        {
            HotbarManager.currDraggedIBP = storedItemBase;

            HotbarManager.instance.startedOnHSB = false;

            GameObject newDraggableSlot = Instantiate(InventoryManager.instance.draggableInvSlotPrefab, Input.mousePosition, Quaternion.identity, InventoryManager.instance.draggableInvSlotParent);

            newDraggableSlot.GetComponent<DraggableInventorySlot>().SetInformations();

            newDraggableSlot.GetComponent<DraggableInventorySlot>().iBPImg.color = newDraggableSlot.GetComponent<DraggableInventorySlot>().cantBeSetColor;

            HotbarManager.instance.currDraggableInventorySlotObj = newDraggableSlot;
        }
    }

    public void OnClickAndLeaveSlot()
    {
        if (storedItemBase.itemType != ItemBaseProfile.ItemType.weapon)
        {
            if (Input.GetKey(KeyCode.Mouse0) && HotbarManager.instance.currDraggableInventorySlotObj == null && !HotbarManager.instance.startedOnHSB)
            {
                InstantiateDraggableCopy();
            }
        }
    }
}
