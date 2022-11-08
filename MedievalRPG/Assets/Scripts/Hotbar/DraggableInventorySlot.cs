using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DraggableInventorySlot : MonoBehaviour
{
    public Image iBPImg;
    public TMP_Text iBPAmountTxt;

    public Color canBeSetColor;
    public Color cantBeSetColor;

    public void SetInformations()
    {
        iBPImg.sprite = HotbarManager.currDraggedIBP.itemSprite;

        //if (HotbarManager.instance.startedOnHSB)
        //{
        //    iBPAmountTxt.text = "99";
        //}
        if (!HotbarManager.instance.startedOnHSB)
        {
            SetItemAmountText();
        }

        HotbarHowManyScreen.currIBP = HotbarManager.currDraggedIBP;
    }

    public void Update()
    {
        transform.position = Input.mousePosition;

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (HotbarManager.currHSB == null)
            {
                if (HotbarManager.instance.draggedHotbarItem)
                {
                    HotbarManager.lastDraggedStoredItemHS.GetComponent<HotbarSlotButton>().RemoveStoredItem();

                    HotbarManager.lastDraggedStoredItemHS = null;
                    HotbarManager.instance.draggedHotbarItem = false;
                    HotbarManager.instance.startedOnHSB = false;

                    HotbarManager.instance.currDraggableInventorySlotObj = null;

                    Destroy(this.gameObject);
                }
                else
                {
                    HotbarManager.currDraggedIBP = null;
                    HotbarManager.instance.currDraggableInventorySlotObj = null;

                    Destroy(this.gameObject);
                }
            }
            else
            {
                if (HotbarManager.instance.startedOnHSB)
                {
                    HotbarManager.instance.hbHMScreen.currDisplayedAmount = HotbarManager.currHSB.itemAmount;

                    Debug.Log(HotbarManager.currHSB.itemAmount);
                    HotbarManager.currHSB.ChangeHotbarSlotItem(HotbarManager.currDraggedIBP, int.Parse(iBPAmountTxt.text));
                }
                else
                {
                    HotbarManager.instance.hbHMScreen.currDisplayedAmount = 1;

                    if (int.Parse(iBPAmountTxt.text) > 1)
                    {
                        HotbarManager.instance.OpenHowManyHotbarScreen();
                    }
                    else
                    {
                        HotbarManager.currHSB.ChangeHotbarSlotItem(HotbarManager.currDraggedIBP, 1);
                    }
                }

                Destroy(this.gameObject);
            }
        }
    }

    public void SetItemAmountText()
    {
        for (int i = 0; i < InventoryManager.instance.inventory.slots.Count; i++)
        {
            if (InventoryManager.instance.inventory.slots[i].itemBase == HotbarManager.currDraggedIBP)
            {
                iBPAmountTxt.text = InventoryManager.instance.inventory.slots[i].itemAmount.ToString();

                return;
            }
        }
    }
}
