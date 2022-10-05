using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ShopItemButton : MonoBehaviour
{
    public ItemBaseProfile storedItemBase;

    public TMP_Text itemNameTxt;
    public TMP_Text itemBuyPriceTxt;
    public TMP_Text itemMindLvlTxt;

    public void Start()
    {
        EventTrigger trigger = GetComponentInParent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { ShopManager.instance.DisplayAllInformationsAboutOneItem(storedItemBase); });
        trigger.triggers.Add(entry);
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

    public void CloseRightShopItemInformations()
    {
        ShopManager.instance.rightShopItemInformationGO.SetActive(false);
    }
}
