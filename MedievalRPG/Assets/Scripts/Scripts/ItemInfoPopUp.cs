using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemInfoPopUp : MonoBehaviour
{
    public TMP_Text itemNameTxt;
    public TMP_Text itemTypeTxt;

    public TMP_Text buyOrSellPriceTxt;
    public TMP_Text weightTxt;

    [Header("Whats Needed For Buying - UI")]
    public GameObject wNFBUI;
    public Image[] neededItemsImages;

    public void SetItemInformationsToDisplay(ItemBaseProfile iBP, bool isPlayerItem)
    {
        if (iBP != null)
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

            if (ShopManager.instance.itemInfoPopUpLeft.activeSelf || ShopManager.instance.itemInfoPopUpRight.activeSelf || ShopManager.instance.mainShopScreen.activeSelf)
            {
                if (isPlayerItem)
                {
                    buyOrSellPriceTxt.text = iBP.sellingPrice.ToString();
                }
                else
                {
                    buyOrSellPriceTxt.text = iBP.buyPrice.ToString();
                }
            }
            else
            {
                buyOrSellPriceTxt.text = iBP.sellingPrice.ToString();
            }

            weightTxt.text = iBP.weight.ToString();

            if (ShopManager.instance.mainShopScreen.activeSelf)
            {
                if (iBP.itemsNeededForBuying.Length > 0)
                {
                    wNFBUI.SetActive(true);

                    for (int i = 0; i < neededItemsImages.Length; i++)
                    {
                        neededItemsImages[i].gameObject.SetActive(false);
                    }

                    for (int i = 0; i < iBP.itemsNeededForBuying.Length; i++)
                    {
                        neededItemsImages[i].sprite = iBP.itemsNeededForBuying[i].itemSprite;

                        neededItemsImages[i].gameObject.SetActive(true);

                        neededItemsImages[i].color = Color.red;

                        for (int y = 0; y < InventoryManager.instance.inventory.slots.Count; y++)
                        {
                            if (InventoryManager.instance.inventory.slots[y].itemBase == iBP.itemsNeededForBuying[i])
                            {
                                neededItemsImages[i].color = Color.white;

                                return;
                            }
                        }
                    }
                }
                else
                {
                    wNFBUI.SetActive(false);
                }
            }            
        }        
    }
}
