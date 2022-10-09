using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ShopItemButton : MonoBehaviour
{
    public ItemBaseProfile storedItemBase;
    public int amountInInv = 0;

    public TMP_Text itemNameTxt;
    public TMP_Text itemBuyPriceTxt;
    public TMP_Text itemMindLvlTxt;

    public ShopCursor shopCursor;

    [HideInInspector] public bool isPressing = false;
    [HideInInspector] public float timeToPress = 1f;
    [HideInInspector] public float pressedTime = 0;

    public void Awake()
    {
        shopCursor = ShopCursor.instance;
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
        if (isPressing)
        {
            pressedTime += Time.deltaTime;
            shopCursor.cursorImg.fillAmount += 1.0f/timeToPress * Time.deltaTime;

            if (pressedTime >= timeToPress)
            {
                isPressing = false;

                Debug.Log("Jjjjjjjjjjjjjjjjjjjjjjjj");

                if (storedItemBase.stackable)
                {
                    OpenHowManyScreen();
                }
                else
                {
                    ShopManager.instance.BuyOrSellItem(storedItemBase, 1);
                }

                shopCursor.cursorImg.fillAmount = 0;
                pressedTime = 0;
            }
        }
    }

    public void DisplayStoredItemInformation()
    {
        itemNameTxt.text = storedItemBase.itemName;
        itemBuyPriceTxt.text = storedItemBase.buyPrice.ToString();

        if (storedItemBase.minLvlToUse != 0)
        {
            itemMindLvlTxt.text = "Lvl. " + storedItemBase.minLvlToUse.ToString();
        }
        else
        {
            itemMindLvlTxt.gameObject.SetActive(false);
        }
    }

    public void OpenHowManyScreen()
    {
        ShopManager.instance.hMScreen.currAmount = 0;
        ShopManager.instance.hMScreen.currAmountTxt.text = ShopManager.instance.hMScreen.currAmount.ToString();

        HowManyScreen.currItem = storedItemBase;
        ShopManager.instance.hMScreen.currAmountInInv = amountInInv;

        ShopManager.instance.hMScreen.gameObject.SetActive(true);
    }

    public void StartedPressingButton()
    {
        isPressing = true;
    }

    public void EndedPressingButton()
    {
        isPressing = false;

        shopCursor.cursorImg.fillAmount = 0;
        pressedTime = 0;
    }

    public void CloseRightShopItemInformations()
    {
        ShopManager.instance.rightShopItemInformationGO.SetActive(false);
    }
}
