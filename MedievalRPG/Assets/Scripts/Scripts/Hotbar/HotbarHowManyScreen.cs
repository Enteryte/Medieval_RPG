using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HotbarHowManyScreen : MonoBehaviour
{
    public int currDisplayedAmount = 1;
    public int currMaxAmount = 1;

    public TMP_Text currAmountTxt;
    public TMP_Text currMaxAmountTxt;

    public Color normalWeightColor;
    public Color tooMuchWeightColor;

    public static ItemBaseProfile currIBP;

    public Slider howManyHBSlider;
    public GameObject sliderHandle;

    public void Update()
    {
        UpdateSliderValues();
    }

    public void SetStartValues(int maxAmount)
    {
        howManyHBSlider.maxValue = maxAmount;
        howManyHBSlider.value = maxAmount;

        currAmountTxt.text = maxAmount.ToString();
        currMaxAmountTxt.text = maxAmount.ToString();

        currDisplayedAmount = maxAmount;

        Debug.Log("1");
    }

    public void UpdateSliderValues()
    {
        currAmountTxt.text = howManyHBSlider.value.ToString();

        Debug.Log(howManyHBSlider.value.ToString());
        Debug.Log(currDisplayedAmount);
    }

    public void SetNewMaxAmount()
    {
        for (int i = 0; i < InventoryManager.instance.inventory.slots.Count; i++)
        {
            if (InventoryManager.instance.inventory.slots[i].itemBase == HotbarManager.currDraggedIBP)
            {
                currMaxAmount = InventoryManager.instance.inventory.slots[i].itemAmount;

                return;
            }
        }
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
        UpdateWeightTextColor();
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
        UpdateWeightTextColor();
    }

    public void UpdateAmountText()
    {
        currAmountTxt.text = currDisplayedAmount.ToString();
    }

    public void UpdateWeightTextColor()
    {
        float currStoredWeightOfItem = 0;

        for (int i = 0; i < HotbarManager.instance.allHotbarSlotBtn.Length; i++)
        {
            if (HotbarManager.instance.allHotbarSlotBtn[i].storedItemBase != null && HotbarManager.instance.allHotbarSlotBtn[i].storedItemBase == currIBP)
            {
                currStoredWeightOfItem = HotbarManager.instance.allHotbarSlotBtn[i].storedAmount * HotbarManager.instance.allHotbarSlotBtn[i].storedItemBase.weight;

                break;
            }
        }

        if (InventoryManager.instance.maxHoldingWeight - (InventoryManager.instance.currHoldingWeight - currStoredWeightOfItem) > currDisplayedAmount * currIBP.weight)
        {
            currAmountTxt.color = normalWeightColor;
        }
        else
        {
            currAmountTxt.color = tooMuchWeightColor;
        }
    }

    public void AcceptAmount()
    {
        //HotbarManager.instance.hbHMScreen.UpdateAmountText();
        //HotbarManager.instance.hbHMScreen.SetNewMaxAmount();

        Debug.Log(currDisplayedAmount);
        HotbarManager.currHSB.ChangeHotbarSlotItem(currIBP, currDisplayedAmount);

        HotbarManager.currHSB = null;

        this.gameObject.SetActive(false);
    }

    public void CloseScreen()
    {
        //HotbarManager.instance.hbHMScreen.UpdateAmountText();
        //HotbarManager.instance.hbHMScreen.SetNewMaxAmount();

        HotbarManager.currHSB = null;

        this.gameObject.SetActive(false);
    }
}
