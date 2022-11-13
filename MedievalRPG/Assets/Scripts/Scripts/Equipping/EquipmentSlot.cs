using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    public ItemBaseProfile currEquippedItem;
    public ItemBaseProfile newItemToEquip;

    public Image currEquippedImg;

    public Sprite noCurrEquippedItemSprite;

    [HideInInspector] public bool isPressing = false;
    public float timeToPress = 1f;
    [HideInInspector] public float pressedTime = 0;

    public Button btnComp;

    public ClickCursor clickCursor;

    public void Awake()
    {
        clickCursor = ClickCursor.instance;
    }

    public void Start()
    {
        ChangeEquippedItem();
    }

    public void Update()
    {
        if (isPressing)
        {
            if (currEquippedItem != null)
            {
                pressedTime += Time.deltaTime;
                clickCursor.cursorImg.fillAmount += 1.0f / timeToPress * Time.deltaTime;

                if (pressedTime >= timeToPress)
                {
                    isPressing = false;

                    ChangeEquippedItem();
                    InventoryManager.instance.DisplayItemsOfCategory();

                    clickCursor.cursorImg.fillAmount = 0;
                    pressedTime = 0;
                }
            }
        }

        if (currEquippedItem == null)
        {
            btnComp.interactable = false;
        }
        else
        {
            btnComp.interactable = true;
        }
    }

    public void ChangeEquippedItem()
    {
        if (currEquippedItem != null)
        {
            InventoryManager.instance.inventory.AddItem(currEquippedItem, 1);

            InventoryManager.instance.RemoveHoldingWeight(currEquippedItem.weight, 1);
        }

        if (newItemToEquip != null)
        {
            InventoryManager.instance.AddHoldingWeight(newItemToEquip.weight, 1);
        }

        if (currEquippedItem != null && EquippingManager.instance.weaponParentObj != null)
        {
            for (int i = 0; i < EquippingManager.instance.weaponParentObj.transform.childCount; i++)
            {
                if (EquippingManager.instance.weaponParentObj.transform.GetChild(i).GetComponent<Item>().iBP.itemPrefab == currEquippedItem.itemPrefab)
                {
                    EquippingManager.instance.weaponParentObj.transform.GetChild(i).gameObject.SetActive(false);

                    return;
                }
            }
        }

        if (newItemToEquip != null && EquippingManager.instance.weaponParentObj != null)
        {
            for (int i = 0; i < EquippingManager.instance.weaponParentObj.transform.childCount; i++)
            {
                if (EquippingManager.instance.weaponParentObj.transform.GetChild(i).GetComponent<Item>().iBP.itemPrefab == newItemToEquip.itemPrefab)
                {
                    EquippingManager.instance.weaponParentObj.transform.GetChild(i).gameObject.SetActive(false);

                    return;
                }
            }
        }

        currEquippedItem = newItemToEquip;

        DisplayCurrentEquippedItemSprite();

        //InventoryManager.currIBP = null;

        //InventoryManager.instance.inventorySlotsParentObjTrans.GetChild(0).GetComponent<InventorySlotButton>().DisplayAllItemInformationsOnClick();

        if (InventoryManager.instance.inventorySlotsParentObjTrans.childCount == 0)
        {
            InventoryManager.instance.rightInventoryScreen.SetActive(false);
        }

        //InventoryManager.instance.DisplayItemsOfCategory();

        newItemToEquip = null;
    }

    public void DisplayCurrentEquippedItemSprite()
    {
        if (currEquippedItem == null)
        {
            currEquippedImg.sprite = noCurrEquippedItemSprite;
        }
        else
        {
            currEquippedImg.sprite = currEquippedItem.itemSprite;
        }
    }

    public void StartedPressingButton()
    {
        isPressing = true;
    }

    public void EndedPressingButton()
    {
        isPressing = false;

        clickCursor.cursorImg.fillAmount = 0;
        pressedTime = 0;
    }
}
