using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HotbarSlotButton : MonoBehaviour
{
    public ItemBaseProfile iBP;
    public int itemAmountInInv;

    public Image itemSpriteImg;
    public TMP_Text itemAmountTxt;

    public HotbarSlotButton correspondingMainScreenHotbarSlotBtn; // Hotbar-Slot in the Main-UI -> not in the inventory!

    public bool isInteractable = false;

    [HideInInspector] public bool isOverButton = false;

    public void ChangeHotbarSlotItem(ItemBaseProfile newItemBP)
    {
        var oldIBP = iBP;

        iBP = newItemBP;

        if (oldIBP != null)
        {
            InventoryManager.instance.RemoveHoldingWeight(oldIBP.weight);
        }

        if (newItemBP != null)
        {
            InventoryManager.instance.AddHoldingWeight(newItemBP.weight);
        }

        for (int i = 0; i < HotbarManager.instance.allHotbarSlotBtn.Length; i++)
        {
            if (HotbarManager.instance.allHotbarSlotBtn[i].iBP != null && HotbarManager.instance.allHotbarSlotBtn[i].iBP == newItemBP && HotbarManager.instance.allHotbarSlotBtn[i] != this)
            {
                HotbarManager.instance.allHotbarSlotBtn[i].ChangeHotbarSlotItem(oldIBP);

                break;
            }
        }

        if (iBP != null)
        {
            SetItemAmountInInv();

            itemSpriteImg.sprite = iBP.itemSprite;
            itemAmountTxt.text = itemAmountInInv.ToString();

            correspondingMainScreenHotbarSlotBtn.itemSpriteImg.sprite = iBP.itemSprite;
            correspondingMainScreenHotbarSlotBtn.itemAmountTxt.text = itemAmountInInv.ToString();
        }
        else
        {
            itemAmountInInv = 0;

            itemSpriteImg.sprite = null;
            itemAmountTxt.text = itemAmountInInv.ToString();

            correspondingMainScreenHotbarSlotBtn.itemSpriteImg.sprite = null;
            correspondingMainScreenHotbarSlotBtn.itemAmountTxt.text = itemAmountInInv.ToString();

            Debug.Log("IS NULL");
        }

        HotbarManager.currHSB = null;
        HotbarManager.currDraggedIBP = null;

        HotbarManager.instance.currDraggableInventorySlotObj = null;
    }

    public void SetItemAmountInInv()
    {
        for (int i = 0; i < InventoryManager.instance.inventory.slots.Count; i++)
        {
            if (InventoryManager.instance.inventory.slots[i].itemBase == iBP)
            {
                itemAmountInInv = InventoryManager.instance.inventory.slots[i].itemAmount;

                return;
            }
        }
    }

    public void RemoveStoredItem()
    {
        ChangeHotbarSlotItem(null);
    }

    public void OnHoverOverSlotStart()
    {
        isOverButton = true;

        HotbarManager.currHSB = this;

        if (HotbarManager.instance.currDraggableInventorySlotObj != null)
        {
            HotbarManager.instance.currDraggableInventorySlotObj.GetComponent<DraggableInventorySlot>().iBPImg.color =
            HotbarManager.instance.currDraggableInventorySlotObj.GetComponent<DraggableInventorySlot>().canBeSetColor;
        }

        HotbarManager.instance.draggedHotbarItem = false;

        HotbarManager.lastDraggedStoredItemHS = null;
    }

    public void OnHoverOverSlotExit()
    {
        isOverButton = false;

        HotbarManager.currHSB = null;

        if (HotbarManager.instance.currDraggableInventorySlotObj != null)
        {
            HotbarManager.instance.currDraggableInventorySlotObj.GetComponent<DraggableInventorySlot>().iBPImg.color =
                HotbarManager.instance.currDraggableInventorySlotObj.GetComponent<DraggableInventorySlot>().cantBeSetColor;
        }
    }

    public void OnMouseDownWithItemBase()
    {
        if (iBP != null)
        {
            HotbarManager.currDraggedIBP = iBP;

            GameObject newDraggableSlot = Instantiate(InventoryManager.instance.draggableInvSlotPrefab, Input.mousePosition, Quaternion.identity, InventoryManager.instance.draggableInvSlotParent);

            newDraggableSlot.GetComponent<DraggableInventorySlot>().SetInformations();

            newDraggableSlot.GetComponent<DraggableInventorySlot>().iBPImg.color = newDraggableSlot.GetComponent<DraggableInventorySlot>().cantBeSetColor;

            HotbarManager.instance.currDraggableInventorySlotObj = newDraggableSlot;

            HotbarManager.instance.draggedHotbarItem = true;

            HotbarManager.lastDraggedStoredItemHS = this.gameObject;
        }
    }
}
