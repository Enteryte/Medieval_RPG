using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemButton : MonoBehaviour
{
    public ItemBaseProfile storedItemBase;
    public int amountInInv = 0;

    public TMP_Text itemNameTxt;
    public TMP_Text itemBuyPriceTxt;
    public TMP_Text itemMindLvlTxt;

    public ClickCursor clickCursor;

    //[HideInInspector] public bool isPressing = false;
    //public float timeToPress = 1f;
    //[HideInInspector] public float pressedTime = 0;

    public Button btnComp;

    public void Awake()
    {
        clickCursor = ClickCursor.instance;
    }

    public void Start()
    {
        EventTrigger trigger = GetComponentInParent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { ShopManager.instance.DisplayAllInformationsAboutOneItem(storedItemBase); });
        trigger.triggers.Add(entry);

        if (storedItemBase.stackable)
        {
            for (int i = 0; i < InventoryManager.instance.inventory.slots.Count; i++)
            {
                if (InventoryManager.instance.inventory.slots[i].itemBase == storedItemBase)
                {
                    amountInInv = InventoryManager.instance.inventory.slots[i].itemAmount;

                    return;
                }
            }
        }
        else
        {
            for (int i = 0; i < InventoryManager.instance.inventory.slots.Count; i++)
            {
                if (InventoryManager.instance.inventory.slots[i].itemBase == storedItemBase)
                {
                    amountInInv += 1;
                }
            }
        }
    }

    public void Update()
    {
        //if (isPressing)
        //{
        //    pressedTime += Time.deltaTime;
        //    clickCursor.cursorImg.fillAmount += 1.0f/timeToPress * Time.deltaTime;

        //    if (pressedTime >= timeToPress)
        //    {
        //        isPressing = false;

        //        if (storedItemBase.stackable)
        //        {
        //            OpenHowManyScreen();
        //        }
        //        else
        //        {
        //            ShopManager.instance.BuyOrSellItem(storedItemBase, 1);
        //        }

        //        clickCursor.cursorImg.fillAmount = 0;
        //        pressedTime = 0;
        //    }
        //}

        if (this.gameObject.transform.parent == ShopManager.instance.merchantItemSlotParentTrans)
        {
            if (PlayerValueManager.instance.money >= storedItemBase.buyPrice)
            {
                btnComp.interactable = true;
            }
            else
            {
                btnComp.interactable = false;
            }
        }
    }

    public void DisplayStoredItemInformation()
    {
        itemNameTxt.text = storedItemBase.itemName;

        if (ShopManager.instance.isBuying)
        {
            itemBuyPriceTxt.text = storedItemBase.buyPrice.ToString();
        }
        else
        {
            itemBuyPriceTxt.text = storedItemBase.sellingPrice.ToString();
        }

        //if (storedItemBase.minLvlToUse != 0)
        //{
        //    itemMindLvlTxt.text = "Lvl. " + storedItemBase.minLvlToUse.ToString();
        //}
        //else
        //{
        //    itemMindLvlTxt.gameObject.SetActive(false);
        //}
    }

    //public void OpenHowManyScreen(ItemBaseProfile iBP)
    //{
    //    //if (ShopManager.instance.isBuying)
    //    //{
    //    //    ShopManager.instance.hMScreen.buyOrSellTxt.text = "Wie oft möchtst du das Item kaufen?";
    //    //}
    //    //else
    //    //{
    //    //    ShopManager.instance.hMScreen.buyOrSellTxt.text = "Wie oft möchtst du das Item verkaufen?";
    //    //}

    //    ShopManager.instance.hMScreen.SetNewMaxAmount(iBP);

    //    //HowManyScreen.currItem = storedItemBase;
    //    //ShopManager.instance.hMScreen.currAmountInInv = amountInInv;

    //    ShopManager.instance.hMScreen.gameObject.SetActive(true);
    //}

    //public void StartedPressingButton()
    //{
    //    if (ShopManager.instance.isBuying)
    //    {
    //        if (PlayerValueManager.instance.money >= storedItemBase.buyPrice)
    //        {
    //            isPressing = true;
    //        }
    //    }
    //    else
    //    {
    //        isPressing = true;
    //    }
    //}

    //public void EndedPressingButton()
    //{
    //    isPressing = false;

    //    clickCursor.cursorImg.fillAmount = 0;
    //    pressedTime = 0;
    //}

    //public void CloseRightShopItemInformations()
    //{
    //    if (!ShopManager.instance.hMScreen.gameObject.activeSelf)
    //    {
    //        ShopManager.instance.rightShopItemInformationGO.SetActive(false);

    //        isPressing = false;

    //        clickCursor.cursorImg.fillAmount = 0;
    //        pressedTime = 0;
    //    }
    //}
}
