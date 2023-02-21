using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemInfoPopUp : MonoBehaviour
{
    public TMP_Text itemNameTxt;
    public TMP_Text itemTypeTxt;

    public TMP_Text itemDescriptionTxt;

    public TMP_Text buyOrSellPriceTxt;
    public TMP_Text weightTxt;

    public TMP_Text mainStatTxt;
    //public TMP_Text[] otherStatTxts;

    public GameObject arrowRed;
    public GameObject arrowGreen;
    public TMP_Text howMuchMoreOrLessTxt;

    [Header("Whats Needed For Buying - UI")]
    public GameObject[] wNFBUI;
    public Image[] neededItemsImages;

    public void SetItemInformationsToDisplay(ItemBaseProfile iBP, bool isPlayerItem)
    {
        if (iBP != null)
        {
            itemNameTxt.text = iBP.itemName;

            itemDescriptionTxt.text = iBP.itemDescription.ToString();

            Debug.Log(iBP.itemDescription);

            if (iBP.itemType == ItemBaseProfile.ItemType.weapon)
            {
                itemTypeTxt.text = iBP.weaponType.ToString();

                //if (iBP.weaponType != ItemBaseProfile.WeaponType.bow)
                //{
                mainStatTxt.text = iBP.normalDamage + " Schaden";

                if (EquippingManager.instance.leftWeaponES.currEquippedItem == null && EquippingManager.instance.rightWeaponES.currEquippedItem == null)
                {
                    arrowGreen.SetActive(true);
                    arrowRed.SetActive(false);

                    howMuchMoreOrLessTxt.text = "+ " + iBP.normalDamage;                   
                }
                else if (EquippingManager.instance.leftWeaponES.currEquippedItem != null || EquippingManager.instance.rightWeaponES.currEquippedItem != null)
                {
                    if (EquippingManager.instance.leftWeaponES.currEquippedItem != null)
                    {
                        if (PlayerValueManager.instance.normalStrength - EquippingManager.instance.leftWeaponES.currEquippedItem.normalDamage
                            > PlayerValueManager.instance.normalStrength - iBP.normalDamage)
                        {
                            arrowGreen.SetActive(false);
                            arrowRed.SetActive(true);

                            howMuchMoreOrLessTxt.text = "- " + ((PlayerValueManager.instance.normalStrength - EquippingManager.instance.leftWeaponES.currEquippedItem.normalDamage) -
                             (PlayerValueManager.instance.normalStrength - iBP.normalDamage)).ToString();
                        }
                        else if (PlayerValueManager.instance.normalStrength - EquippingManager.instance.leftWeaponES.currEquippedItem.normalDamage
                            == PlayerValueManager.instance.normalStrength - iBP.normalDamage)
                        {
                            arrowGreen.SetActive(false);
                            arrowRed.SetActive(false);

                            howMuchMoreOrLessTxt.text = "";
                        }
                        else
                        {
                            arrowGreen.SetActive(true);
                            arrowRed.SetActive(false);

                            howMuchMoreOrLessTxt.text = "+ " + ((PlayerValueManager.instance.normalStrength - iBP.normalDamage) -
                                (PlayerValueManager.instance.normalStrength - EquippingManager.instance.leftWeaponES.currEquippedItem.normalDamage)).ToString();
                        }
                    }
                    else
                    {
                        if (PlayerValueManager.instance.normalStrength - EquippingManager.instance.rightWeaponES.currEquippedItem.normalDamage
                            > PlayerValueManager.instance.normalStrength - iBP.normalDamage)
                        {
                            arrowGreen.SetActive(false);
                            arrowRed.SetActive(true);

                            howMuchMoreOrLessTxt.text = "- " + ((PlayerValueManager.instance.normalStrength - EquippingManager.instance.rightWeaponES.currEquippedItem.normalDamage) -
                             (PlayerValueManager.instance.normalStrength - iBP.normalDamage)).ToString();
                        }
                        else if (PlayerValueManager.instance.normalStrength - EquippingManager.instance.rightWeaponES.currEquippedItem.normalDamage
                            == PlayerValueManager.instance.normalStrength - iBP.normalDamage)
                        {
                            arrowGreen.SetActive(false);
                            arrowRed.SetActive(false);

                            howMuchMoreOrLessTxt.text = "";
                        }
                        else
                        {
                            arrowGreen.SetActive(true);
                            arrowRed.SetActive(false);

                            howMuchMoreOrLessTxt.text = "+ " + ((PlayerValueManager.instance.normalStrength - iBP.normalDamage) -
                                (PlayerValueManager.instance.normalStrength - EquippingManager.instance.rightWeaponES.currEquippedItem.normalDamage)).ToString();
                        }
                    }
                }
                //}
                //else
                //{
                //    mainStatTxt.text = iBP.normalDamage + " Fernkampfschaden";

                //    arrowRed.SetActive(false);
                //    arrowGreen.SetActive(false);
                //}
            }
            else if (iBP.itemType == ItemBaseProfile.ItemType.potion)
            {
                itemTypeTxt.text = iBP.potionType.ToString();

                if (iBP.potionType == ItemBaseProfile.PotionType.healing)
                {
                    mainStatTxt.text = iBP.potionBuffValue + " Heilung";

                    arrowRed.SetActive(false);
                    arrowGreen.SetActive(false);

                    howMuchMoreOrLessTxt.text = "";
                }
                else if (iBP.potionType == ItemBaseProfile.PotionType.speed)
                {
                    mainStatTxt.text = iBP.potionBuffValue + " Geschwindigkeit" + " ( " + iBP.potionBuffLastingTime + "s )";

                    arrowRed.SetActive(false);
                    arrowGreen.SetActive(false);

                    howMuchMoreOrLessTxt.text = "";
                }
                else if (iBP.potionType == ItemBaseProfile.PotionType.strength)
                {
                    mainStatTxt.text = iBP.potionBuffValue + " Geschwindigkeit" + " ( " + iBP.potionBuffLastingTime + "s )";

                    arrowRed.SetActive(false);
                    arrowGreen.SetActive(false);

                    howMuchMoreOrLessTxt.text = "";
                }
                else if (iBP.potionType == ItemBaseProfile.PotionType.stamina)
                {
                    mainStatTxt.text = iBP.potionBuffValue + " Stamina";

                    arrowRed.SetActive(false);
                    arrowGreen.SetActive(false);

                    howMuchMoreOrLessTxt.text = "";
                }

                //mainStatTxt.text = iBP.potionBuffValue
            }
            else if (iBP.itemType == ItemBaseProfile.ItemType.bookOrNote)
            {
                // Buch oder Notiz

                mainStatTxt.text = "";

                arrowRed.SetActive(false);
                arrowGreen.SetActive(false);

                howMuchMoreOrLessTxt.text = "";
            }
            else
            {
                if (iBP.itemType == ItemBaseProfile.ItemType.food)
                {
                    itemTypeTxt.text = "Nahrung";

                    mainStatTxt.text = iBP.foodHealValue + " Heilung" + "( " + iBP.timeTillValueIsHealed + "s )";

                    arrowRed.SetActive(false);
                    arrowGreen.SetActive(false);

                    howMuchMoreOrLessTxt.text = "";
                }
                else if (iBP.itemType == ItemBaseProfile.ItemType.none)
                {
                    if (iBP.readType == ItemBaseProfile.ReadType.book)
                    {
                        itemTypeTxt.text = "Buch";
                    }
                    else if (iBP.readType == ItemBaseProfile.ReadType.scroll)
                    {
                        itemTypeTxt.text = "Brief";
                    }
                    else if (iBP.readType == ItemBaseProfile.ReadType.note)
                    {
                        itemTypeTxt.text = "Notiz";
                    }
                    else
                    {
                        itemTypeTxt.text = "Questgegenstand";
                    }

                    arrowRed.SetActive(false);
                    arrowGreen.SetActive(false);

                    howMuchMoreOrLessTxt.text = "";
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
                    wNFBUI[0].SetActive(true);
                    wNFBUI[1].SetActive(true);

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
                    wNFBUI[0].SetActive(false);
                    wNFBUI[1].SetActive(false);
                }
            }

            //for (int i = 0; i < otherStatTxts.Length; i++)
            //{
            //    if (iBP.otherItemStats != null && iBP.otherItemStats.Length >= i)
            //    {
            //        otherStatTxts[i].gameObject.SetActive(false);

            //        if (iBP.otherItemStats[i].itemStatType.ToString().Contains("plus"))
            //        {
            //            if (iBP.otherItemStats[i].itemStatType == ItemStat.ItemStatType.plusProcentDmg)
            //            {
            //                otherStatTxts[i].text = "+ " + iBP.otherItemStats[i].statValue + " Schadenserhöhung (allgemein)";
            //            }
            //            else if (iBP.otherItemStats[i].itemStatType == ItemStat.ItemStatType.plusProcentDmgSkeleton)
            //            {
            //                otherStatTxts[i].text = "+ " + iBP.otherItemStats[i].statValue + " Schadenserhöhung (Skelette)";
            //            }
            //            else if (iBP.otherItemStats[i].itemStatType == ItemStat.ItemStatType.plusProcentDmgUndead)
            //            {
            //                otherStatTxts[i].text = "+ " + iBP.otherItemStats[i].statValue + " Schadenserhöhung (Untote)"; ;
            //            }
            //        }
            //        else if (iBP.otherItemStats[i].itemStatType.ToString().Contains("minus"))
            //        {
            //            if (iBP.otherItemStats[i].itemStatType == ItemStat.ItemStatType.plusProcentDmg)
            //            {
            //                otherStatTxts[i].text = "- " + iBP.otherItemStats[i].statValue + " Schadensreduzierung (allgemein)";
            //            }
            //            else if (iBP.otherItemStats[i].itemStatType == ItemStat.ItemStatType.plusProcentDmgSkeleton)
            //            {
            //                otherStatTxts[i].text = "- " + iBP.otherItemStats[i].statValue + " Schadensreduzierung (Skelette)";
            //            }
            //            else if (iBP.otherItemStats[i].itemStatType == ItemStat.ItemStatType.plusProcentDmgUndead)
            //            {
            //                otherStatTxts[i].text = "- " + iBP.otherItemStats[i].statValue + " Schadensreduzierung (Untote)"; ;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        otherStatTxts[i].gameObject.SetActive(false);
            //    }
            //}
        }        
    }
}
