using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HowManyScreen : MonoBehaviour
{
    public static ItemBaseProfile currItem;
    public int currAmountInInv = 0;

    public int currAmount = 0;

    public TMP_Text currAmountTxt;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddAmount()
    {
        if (!ShopManager.instance.isBuying)
        {
            if (currAmount + 1 <= currAmountInInv)
            {
                currAmount += 1;
            }
        }
        else
        {
            if (currItem.buyPrice * (currAmount + 1) <= GameManager.instance.playerMoney)
            {
                currAmount += 1;
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

        currAmountTxt.text = currAmount.ToString();
    }

    public void AcceptBuyOrSellAmount()
    {
        ShopManager.instance.BuyOrSellItem(currItem, currAmount);

        this.gameObject.SetActive(false);
    }
}
