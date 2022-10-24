using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HowManyScreen : MonoBehaviour
{
    public static ItemBaseProfile currItem;
    public int currAmountInInv;

    public int currAmount = 0;

    public TMP_Text buyOrSellTxt;
    public TMP_Text currAmountTxt;

    public Button acceptBtn; 

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (currAmount == 0)
        {
            acceptBtn.interactable = false;
        }
        else
        {
            acceptBtn.interactable = true;
        }
    }

    public void AddAmount()
    {
        if (!ShopManager.instance.isBuying)
        {
            if (currAmount + 1 <= currAmountInInv)
            {
                currAmount += 1;
            }
            else
            {
                currAmount = 0;
            }
        }
        else
        {
            if (currItem.buyPrice * (currAmount + 1) <= PlayerValueManager.instance.money)
            {
                currAmount += 1;
            }
            else
            {
                currAmount = 0;
            }
        }

        currAmountTxt.text = currAmount.ToString();
    }

    public void RemoveAmount()
    {
        if (currAmount > 0)
        {
            currAmount -= 1;
        }
        else if (currAmount == 0 && !ShopManager.instance.isBuying)
        {
            //float weight = (InventoryManager.instance.currInvWeight + ((currAmountInInv * currItem.itemWeight) 
            //    * (currItem.sellingPrice * GameManager.instance.coinWeightValue)));

            currAmount = currAmountInInv;
        }
        else if (currAmount == 0 && ShopManager.instance.isBuying)
        {
            currAmount = ((int)PlayerValueManager.instance.money / currItem.buyPrice);          
        }

        currAmountTxt.text = currAmount.ToString();
    }

    public void AcceptBuyOrSellAmount()
    {
        ShopManager.instance.BuyOrSellItem(currItem, currAmount);

        ShopManager.instance.rightShopItemInformationGO.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
