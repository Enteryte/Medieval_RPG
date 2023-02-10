using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopCategoryButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public bool isPlayerCategoryBtn = false;

    public enum ShopCategoryItemType
    {
        none,
        food,
        potion,
        weapon,
        missionItems,
        others
    }

    public ShopCategoryItemType itemTypeToDisplay = ShopCategoryItemType.none;

    public string categoryName;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isPlayerCategoryBtn)
        {
            ShopManager.instance.currPlayerClickedSCBtn = this;

            ShopManager.instance.DisplayPlayerItems();

            ShopManager.instance.currPlayerCategoryNameTxt.text = categoryName;
        }
        else
        {
            ShopManager.instance.currMerchantClickedSCBtn = this;

            ShopManager.instance.DisplayMerchantItems();

            ShopManager.instance.currMerchantCategoryNameTxt.text = categoryName;

            Debug.Log("333333333333333333333333");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("333333");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("99999999");
    }
}
