using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HowManyScreen : MonoBehaviour
{
    public int currDisplayedAmount = 1;
    public int currMaxAmount = 1;

    public TMP_Text currAmountTxt;
    public TMP_Text currMaxAmountTxt;
    public TMP_Text currPriceTxt;

    public Color normalWeightColor;
    public Color tooMuchWeightColor;

    public static ItemBaseProfile currIBP;

    public Slider howManySSlider;
    public GameObject sliderHandle;

    public bool isPlayerItem = false;

    public void Update()
    {
        UpdateSliderValues();
    }

    public void SetStartValues(int maxAmount)
    {
        howManySSlider.maxValue = maxAmount;
        howManySSlider.value = maxAmount;

        currAmountTxt.text = maxAmount.ToString();
        currMaxAmountTxt.text = maxAmount.ToString();

        if (isPlayerItem)
        {
            currPriceTxt.text = (currIBP.sellingPrice * maxAmount).ToString();
        }
        else
        {
            currPriceTxt.text = (currIBP.highBuyPrice * maxAmount).ToString();
        }

        currDisplayedAmount = maxAmount;

        Debug.Log("1");
    }

    public void UpdateSliderValues()
    {
        currAmountTxt.text = howManySSlider.value.ToString();

        if (isPlayerItem)
        {
            currPriceTxt.text = (currIBP.sellingPrice * howManySSlider.value).ToString();
        }
        else
        {
            currPriceTxt.text = (currIBP.highBuyPrice * howManySSlider.value).ToString();
        }

        Debug.Log(howManySSlider.value.ToString());
        Debug.Log(currDisplayedAmount);
    }

    public void SetNewMaxAmount(ItemBaseProfile iBP, bool isPlayerItem)
    {
        currIBP = iBP;
        this.isPlayerItem = isPlayerItem;

        if (isPlayerItem)
        {
            for (int i = 0; i < InventoryManager.instance.inventory.slots.Count; i++)
            {
                if (InventoryManager.instance.inventory.slots[i].itemBase == iBP)
                {
                    currMaxAmount = InventoryManager.instance.inventory.slots[i].itemAmount;

                    break;
                }
            }
        }
        else
        {
            currMaxAmount = ((int)PlayerValueManager.instance.money / iBP.highBuyPrice);
        }

        SetStartValues(currMaxAmount);
    }

    public void AddAmountToAdd()
    {
        if (currDisplayedAmount < currMaxAmount)
        {
            currDisplayedAmount += 1;
        }
        else
        {
            currDisplayedAmount = 1;
        }

        UpdateAmountText();
        //UpdateWeightTextColor();
    }

    public void RemoveAmountToAdd()
    {
        if (currDisplayedAmount == 1)
        {
            currDisplayedAmount = currMaxAmount;
        }
        else
        {
            currDisplayedAmount -= 1;
        }

        UpdateAmountText();
        //UpdateWeightTextColor();
    }

    public void UpdateAmountText()
    {
        currAmountTxt.text = currDisplayedAmount.ToString();

        if (isPlayerItem)
        {
            currPriceTxt.text = (currIBP.sellingPrice * howManySSlider.value).ToString();
        }
        else
        {
            currPriceTxt.text = (currIBP.highBuyPrice * howManySSlider.value).ToString();
        }
    }

    //public void UpdateWeightTextColor()
    //{
    //    float currStoredWeightOfItem = 0;

    //    for (int i = 0; i < HotbarManager.instance.allHotbarSlotBtn.Length; i++)
    //    {
    //        if (HotbarManager.instance.allHotbarSlotBtn[i].storedItemBase != null && HotbarManager.instance.allHotbarSlotBtn[i].storedItemBase == currIBP)
    //        {
    //            currStoredWeightOfItem = HotbarManager.instance.allHotbarSlotBtn[i].storedAmount * HotbarManager.instance.allHotbarSlotBtn[i].storedItemBase.weight;

    //            break;
    //        }
    //    }

    //    if (InventoryManager.instance.maxHoldingWeight - (InventoryManager.instance.currHoldingWeight - currStoredWeightOfItem) > currDisplayedAmount * currIBP.weight)
    //    {
    //        currAmountTxt.color = normalWeightColor;
    //    }
    //    else
    //    {
    //        currAmountTxt.color = tooMuchWeightColor;
    //    }
    //}

    public void AcceptAmount()
    {
        //HotbarManager.instance.hbHMScreen.UpdateAmountText();
        //HotbarManager.instance.hbHMScreen.SetNewMaxAmount();

        Debug.Log(currDisplayedAmount);
        //HotbarManager.currHSB.ChangeHotbarSlotItem(currIBP, currDisplayedAmount);

        //HotbarManager.currHSB = null;

        this.gameObject.SetActive(false);
    }

    public void CloseScreen()
    {
        //HotbarManager.instance.hbHMScreen.UpdateAmountText();
        //HotbarManager.instance.hbHMScreen.SetNewMaxAmount();

        //HotbarManager.currHSB = null;

        this.gameObject.SetActive(false);
    }
}
