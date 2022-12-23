using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemInfoPopUp : MonoBehaviour
{
    public TMP_Text itemNameTxt;
    public TMP_Text itemTypeTxt;

    public TMP_Text buyOrSellPriceTxt;
    public TMP_Text weightTxt;

    public void SetItemInformationsToDisplay(ItemBaseProfile iBP, bool isPlayerItem)
    {
        itemNameTxt.text = iBP.itemName;

        if (iBP.itemType == ItemBaseProfile.ItemType.weapon)
        {
            itemTypeTxt.text = iBP.weaponType.ToString();
        }
        else if (iBP.itemType == ItemBaseProfile.ItemType.potion)
        {
            itemTypeTxt.text = iBP.potionType.ToString();
        }
        else if (iBP.itemType == ItemBaseProfile.ItemType.bookOrNote)
        {
            // Buch oder Notiz
        }
        else
        {
            if (iBP.itemType == ItemBaseProfile.ItemType.food)
            {
                itemTypeTxt.text = "Nahrung";
            }
            else if (iBP.itemType == ItemBaseProfile.ItemType.none)
            {
                itemTypeTxt.text = "Questgegenstand";
            }
        }

        if (isPlayerItem)
        {
            buyOrSellPriceTxt.text = iBP.sellingPrice.ToString();
        }
        else
        {
            buyOrSellPriceTxt.text = iBP.buyPrice.ToString();
        }

        weightTxt.text = iBP.weight.ToString();
    }
}
