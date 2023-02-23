using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;
using System;

public class ClickableInventorySlot : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerExitHandler
{
    public enum ClickableSlotType
    {
        inventorySlot,
        equipmentSlot,
        hotbarSlot,
        categoryButton,
        shopSlot
    }

    public ClickableSlotType clickableSlotType;

    public Image boarder;
    public Animator animator;

    public Image isNewSymbol;
    public TMP_Text storedAmountTxt;

    public ItemBaseProfile storedItemBase;
    public int storedAmount;

    [HideInInspector] public InventorySlot invSlot;
    //[HideInInspector] public List<Button> allInteractableButton;

    public HotbarSlotButton correspondingMainScreenHotbarSlotBtn;

    [HideInInspector] public bool isShopPlayerItem = false;

    public void Start()
    {
        if (clickableSlotType == ClickableSlotType.shopSlot && storedItemBase != null)
        {
            if (isShopPlayerItem)
            {
                if (storedItemBase.itemType == ItemBaseProfile.ItemType.none)
                {
                    this.gameObject.GetComponent<Image>().color = Color.red;
                    this.gameObject.GetComponent<Button>().interactable = false;
                }
                else
                {
                    this.gameObject.GetComponent<Image>().color = Color.white;
                    this.gameObject.GetComponent<Button>().interactable = true;
                }
            }
            else
            {
                if (storedItemBase.buyPrice > PlayerValueManager.instance.money)
                {
                    this.gameObject.GetComponent<Image>().color = Color.red;
                    this.gameObject.GetComponent<Button>().interactable = false;
                }
                else
                {
                    this.gameObject.GetComponent<Image>().color = Color.white;
                    this.gameObject.GetComponent<Button>().interactable = true;
                }
            }

            isNewSymbol.gameObject.SetActive(storedItemBase.isNew);
        }

        if (clickableSlotType != ClickableSlotType.categoryButton)
        {
            //if (this.gameObject.GetComponent<Button>() != null)
            //{
            //    this.gameObject.GetComponent<Button>().onClick.AddListener(SelectAction);

            //    //var eventSystem = EventSystem.current;
            //    //eventSystem.SetSelectedGameObject(null);
            //    //eventSystem.SetSelectedGameObject(this.gameObject, new BaseEventData(eventSystem));
            //}

            if (storedItemBase != null)
            {
                this.gameObject.GetComponent<Image>().sprite = storedItemBase.itemSprite;

                if (storedAmount == 0)
                {
                    storedAmountTxt.text = storedAmount.ToString();
                }
                //else
                //{
                //    // --------------------------------------------------------------> HIER wurde es geändert! <---------------------------------------------------------------------
                //    storedAmount = 1;
                //    storedAmountTxt.text = storedAmount.ToString();
                //}

                if (correspondingMainScreenHotbarSlotBtn != null)
                {
                    correspondingMainScreenHotbarSlotBtn.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = storedItemBase.itemSprite;
                    correspondingMainScreenHotbarSlotBtn.GetComponent<HotbarSlotButton>().itemAmountTxt.text = storedAmount.ToString();
                }
            }
            else
            {
                this.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = false;

                if (correspondingMainScreenHotbarSlotBtn != null)
                {
                    correspondingMainScreenHotbarSlotBtn.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = false;
                }
            }
        }

        if (clickableSlotType == ClickableSlotType.inventorySlot)
        {
            isNewSymbol.gameObject.SetActive(storedItemBase.isNew);
        }
    }

    public void Update()
    {
        if (clickableSlotType == ClickableSlotType.shopSlot && storedItemBase != null)
        {
            if (isShopPlayerItem)
            {
                if (storedItemBase.itemType == ItemBaseProfile.ItemType.none && storedItemBase.neededForMissions || storedItemBase.itemType == ItemBaseProfile.ItemType.bookOrNote)
                {
                    this.gameObject.GetComponent<Image>().color = Color.red;
                    this.gameObject.GetComponent<Button>().interactable = false;
                }
                else
                {
                    this.gameObject.GetComponent<Image>().color = Color.white;
                    this.gameObject.GetComponent<Button>().interactable = true;
                }
            }
            else
            {
                if (storedItemBase.itemsNeededForBuying.Length > 0)
                {
                    if (storedItemBase.CheckNeededItemsForBuying() && storedItemBase.buyPrice <= PlayerValueManager.instance.money)
                    {
                        this.gameObject.GetComponent<Image>().color = Color.white;
                        this.gameObject.GetComponent<Button>().interactable = true;
                    }
                    else
                    {
                        this.gameObject.GetComponent<Image>().color = Color.red;
                        this.gameObject.GetComponent<Button>().interactable = false;
                    }
                }
                else if (storedItemBase.buyPrice > PlayerValueManager.instance.money)
                {
                    this.gameObject.GetComponent<Image>().color = Color.red;
                    this.gameObject.GetComponent<Button>().interactable = false;
                }
                else
                {
                    this.gameObject.GetComponent<Image>().color = Color.white;
                    this.gameObject.GetComponent<Button>().interactable = true;
                }
            }
        }
    }

    public void UpdateSlotInformations()
    {
        if (storedItemBase != null)
        {
            this.gameObject.GetComponent<Image>().sprite = storedItemBase.itemSprite;
            storedAmountTxt.text = storedAmount.ToString();
        }
        else
        {
            this.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = false;
        }
    }

    public void SelectInventorySlot()
    {
        if (InventoryManager.currCIS != null)
        {
            InventoryManager.currCIS.animator.enabled = false;

            InventoryManager.currCIS.boarder.gameObject.SetActive(false);
        }

        InventoryManager.currCIS = this;

        boarder.gameObject.SetActive(true);

        animator.Rebind();
        animator.enabled = true;

        //if (clickableSlotType == ClickableSlotType.equipmentSlot)
        //{
        //    if (this.gameObject.GetComponent<EquipmentSlot>().currEquippedItem != null)
        //    {
        //        InventoryManager.instance.whatToDoTxt.text = "[Leertaste] Entfernen [R] Wegwerfen";
        //    }
        //    else
        //    {
                InventoryManager.instance.whatToDoTxt.text = "";
        //    }
        //}

        //if (InventoryManager.instance.currClickedBtn == null/* && clickableSlotType != ClickableSlotType.categoryButton*/)
        //{

        if (clickableSlotType != ClickableSlotType.shopSlot)
        {
            DisplayAllItemInformationsOnClick();
        }

        //}
        //else if (InventoryManager.instance.currClickedBtn == null && clickableSlotType != ClickableSlotType.categoryButton)
        //{

        //}
    }

    public void OpenHowManyScreen(ItemBaseProfile iBP,bool isPlayerItem)
    {
        //if (ShopManager.instance.isBuying)
        //{
        //    ShopManager.instance.hMScreen.buyOrSellTxt.text = "Wie oft möchtst du das Item kaufen?";
        //}
        //else
        //{
        //    ShopManager.instance.hMScreen.buyOrSellTxt.text = "Wie oft möchtst du das Item verkaufen?";
        //}

        if (iBP.itemType == ItemBaseProfile.ItemType.weapon)
        {
            ShopManager.instance.BuyOrSellItem(iBP, 1);
        }
        else
        {
            ShopManager.instance.hMScreen.SetNewMaxAmount(iBP, isPlayerItem);

            ShopManager.instance.hMScreen.gameObject.SetActive(true);
        }

        //HowManyScreen.currItem = storedItemBase;
        //ShopManager.instance.hMScreen.currAmountInInv = amountInInv;
    }

    public void DisplayWhatToDoText()
    {
        if (clickableSlotType == ClickableSlotType.inventorySlot)
        {
            if (storedItemBase.itemType != ItemBaseProfile.ItemType.weapon && storedItemBase.itemType != ItemBaseProfile.ItemType.bookOrNote && storedItemBase.itemType != ItemBaseProfile.ItemType.none)
            {
                SelectHotbarSlotToEquipTo();
            }
            else if (storedItemBase.itemType == ItemBaseProfile.ItemType.weapon)
            {
                // Equip
            }
        }
        else if (clickableSlotType == ClickableSlotType.hotbarSlot)
        {
            if (InventoryManager.instance.selectHotbarSlotScreen.activeSelf)
            {
                EquipItemToHotbar(null, 0);
            }
            else if (storedItemBase != null)
            {
                ClearHotbarSlot();
                // Zahlen müssen geupdated werden
            }
        }
        else if (clickableSlotType == ClickableSlotType.equipmentSlot)
        {
            if (storedItemBase != null)
            {
                ClearEquipmentSlot();
            }
        }
        else if (clickableSlotType == ClickableSlotType.shopSlot)
        {
            ShopManager.instance.isShopPlayerItem = isShopPlayerItem;

            OpenHowManyScreen(storedItemBase, isShopPlayerItem);
        }
    }

    public void SelectAction()
    {
        if (clickableSlotType == ClickableSlotType.inventorySlot)
        {
            if (storedItemBase.itemType != ItemBaseProfile.ItemType.weapon && storedItemBase.itemType != ItemBaseProfile.ItemType.bookOrNote && storedItemBase.itemType != ItemBaseProfile.ItemType.none)
            {
                SelectHotbarSlotToEquipTo();
            }
            else if (storedItemBase.itemType == ItemBaseProfile.ItemType.weapon)
            {
                EquipItemToEquipment(null, 0);
            }
            else if (storedItemBase.itemType == ItemBaseProfile.ItemType.bookOrNote)
            {
                if (storedItemBase.readType == ItemBaseProfile.ReadType.book)
                {
                    OpenBookScrollOrNoteUI(InventoryManager.instance.bookUI);
                }
                else if (storedItemBase.readType == ItemBaseProfile.ReadType.scroll)
                {
                    OpenBookScrollOrNoteUI(InventoryManager.instance.scrollUI);
                }
                else if (storedItemBase.readType == ItemBaseProfile.ReadType.note)
                {
                    OpenBookScrollOrNoteUI(InventoryManager.instance.noteUI);
                }
            }
        }
        else if (clickableSlotType == ClickableSlotType.hotbarSlot)
        {
            if (InventoryManager.instance.selectHotbarSlotScreen.activeSelf)
            {
                EquipItemToHotbar(null, 0);
            }
            else if (storedItemBase != null)
            {
                ClearHotbarSlot();
                // Zahlen müssen geupdated werden
            }
        }
        else if (clickableSlotType == ClickableSlotType.equipmentSlot)
        {
            if (storedItemBase != null)
            {
                ClearEquipmentSlot();
            }
        }
        else if (clickableSlotType == ClickableSlotType.shopSlot)
        {
            ShopManager.instance.isShopPlayerItem = isShopPlayerItem;

            OpenHowManyScreen(storedItemBase, isShopPlayerItem);
        }

        InventoryManager.instance.weightTxt.text = InventoryManager.instance.currHoldingWeight + " / " + InventoryManager.instance.maxHoldingWeight;
        ShopManager.instance.weightTxt.text = InventoryManager.instance.currHoldingWeight + " / " + InventoryManager.instance.maxHoldingWeight;
    }

    public void EquipItemToEquipment(ItemBaseProfile ibToUse, int amountToStore)
    {
        InventoryManager.instance.currClickedBtn = this;

        //var oldIB = storedItemBase;
        //var newIB =

        //var oldIB = storedItemBase;

        //Debug.Log(oldIB);

        //var newIB = InventoryManager.instance.currClickedBtn.storedItemBase;

        //Debug.Log(newIB);

        // --------------------- Linke Hand fehlt noch
        
        if (ibToUse == null)
        {
            if (InventoryManager.instance.currClickedBtn.storedItemBase != null)
            {
                for (int i = 0; i < EquippingManager.instance.rightWeaponParentObj.transform.childCount; i++)
                {
                    //HandleEquippingWeapon(EquippingManager.instance.rightWeaponES, EquippingManager.instance.rightWeaponParentObj, FightingActions.equippedWeaponR,
                    //    InventoryManager.instance.currClickedBtn.storedItemBase, amountToStore, i);

                    if (EquippingManager.instance.rightWeaponParentObj.transform.GetChild(i).GetComponent<Item>().iBP == storedItemBase)
                    {
                        storedItemBase = InventoryManager.instance.currClickedBtn.storedItemBase;

                        EquippingManager.instance.rightWeaponParentObj.transform.GetChild(i).gameObject.SetActive(true);
                        FightingActions.instance.equippedWeaponR = EquippingManager.instance.rightWeaponParentObj.transform.GetChild(i).gameObject;

                        FightingActions.instance.GetWeapon();

                        if (EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase != null)
                        {
                            if (EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase.weaponType == ItemBaseProfile.WeaponType.bow)
                            {
                                //FightingActions.instance.anim.SetTrigger("DeequipBow");

                                FightingActions.instance.anim.ResetTrigger("BowIdle");
                                EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().ClearEquipmentSlot();

                                Debug.Log("1" + storedItemBase);
                            }                         
                        }

                        if (EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase != null)
                        {
                            InventoryManager.instance.RemoveHoldingWeight(EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase.weight, 1);
                            InventoryManager.instance.inventory.AddItem(EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase, 1);

                            for (int x = 0; x < EquippingManager.instance.rightWeaponParentObj.transform.childCount; x++)
                            {
                                if (EquippingManager.instance.rightWeaponParentObj.transform.GetChild(x).GetComponent<Item>().iBP ==
                                    EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase)
                                {
                                    EquippingManager.instance.rightWeaponParentObj.transform.GetChild(x).gameObject.SetActive(false);
                                }
                            }

                            //if (EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase.weaponType == ItemBaseProfile.WeaponType.greatsword)
                            //{
                            //    FightingActions.instance.anim.SetTrigger("DeequipGreatSword");
                            //}

                            if (storedItemBase.weaponType == ItemBaseProfile.WeaponType.greatsword)
                            {
                                EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().ClearEquipmentSlot();
                            }
                        }

                        EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase = storedItemBase;
                        EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedAmount = 1;

                        //EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedAmountTxt.gameObject.SetActive(false);

                        EquippingManager.instance.rightWeaponES.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                        EquippingManager.instance.rightWeaponES.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = storedItemBase.itemSprite;

                        InventoryManager.instance.AddHoldingWeight(storedItemBase.weight, 1);

                        InventoryManager.instance.inventory.RemoveItem(EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase, 1);

                        //storedItemBase

                        //var eventSystem = EventSystem.current;
                        //eventSystem.SetSelectedGameObject(null);
                        //eventSystem.SetSelectedGameObject(EquippingManager.instance.rightWeaponES.gameObject, new BaseEventData(eventSystem));

                        Debug.Log("IS HERE!");
                        break;
                    }
                }

                for (int i = 0; i < EquippingManager.instance.weaponParentObj.transform.childCount; i++)
                {
                    //HandleEquippingWeapon(EquippingManager.instance.rightWeaponES, EquippingManager.instance.rightWeaponParentObj, FightingActions.equippedWeaponR,
                    //    InventoryManager.instance.currClickedBtn.storedItemBase, amountToStore, i);

                    if (EquippingManager.instance.weaponParentObj.transform.GetChild(i).GetComponent<Item>().iBP == storedItemBase)
                    {
                        storedItemBase = InventoryManager.instance.currClickedBtn.storedItemBase;

                        EquippingManager.instance.weaponParentObj.transform.GetChild(i).gameObject.SetActive(true);
                        FightingActions.instance.equippedWeaponL = EquippingManager.instance.weaponParentObj.transform.GetChild(i).gameObject;

                        FightingActions.instance.GetWeapon();

                        if (EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase != null)
                        {
                            if (EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase.weaponType == ItemBaseProfile.WeaponType.greatsword)
                            {
                                //FightingActions.instance.anim.SetTrigger("DeequipGreatSword");

                                FightingActions.instance.anim.ResetTrigger("GreatSwordIdle");
                                EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().ClearEquipmentSlot();

                                Debug.Log("1" + storedItemBase);
                            }
                        }

                        if (EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase != null)
                        {
                            InventoryManager.instance.RemoveHoldingWeight(EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase.weight, 1);
                            InventoryManager.instance.inventory.AddItem(EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase, 1);

                            for (int x = 0; x < EquippingManager.instance.weaponParentObj.transform.childCount; x++)
                            {
                                if (EquippingManager.instance.weaponParentObj.transform.GetChild(x).GetComponent<Item>().iBP ==
                                    EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase)
                                {
                                    EquippingManager.instance.weaponParentObj.transform.GetChild(x).gameObject.SetActive(false);
                                }
                            }

                            if (EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase.weaponType == ItemBaseProfile.WeaponType.bow)
                            {
                                FightingActions.instance.anim.SetTrigger("DeequipBow");
                            }
                        }

                        if (storedItemBase.weaponType == ItemBaseProfile.WeaponType.bow)
                        {
                            EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().ClearEquipmentSlot();

                            FightManager.instance.UpdateArrowHUDDisplay();
                            GameManager.instance.arrowHUDDisplayGO.SetActive(true);
                        }

                        //if (EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase != null)
                        //{
                        //    //FightingActions.instance.anim.SetTrigger("DeequipBow");
                        //    //FightingActions.instance.anim.SetTrigger("DeequipGreatSword");

                        //    if (storedItemBase.weaponType == ItemBaseProfile.WeaponType.bow)
                        //    {
                        //        FightingActions.instance.anim.SetTrigger("DeequipGreatSword");

                        //        if (EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase != null)
                        //        {
                        //            EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().ClearEquipmentSlot();
                        //        }

                        //        Debug.Log("1" + storedItemBase);
                        //    }
                        //    else if (storedItemBase.weaponType == ItemBaseProfile.WeaponType.greatsword)
                        //    {
                        //        FightingActions.instance.anim.SetTrigger("DeequipBow");

                        //        if (EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase != null)
                        //        {
                        //            EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().ClearEquipmentSlot();
                        //        }

                        //        Debug.Log("2" + storedItemBase);
                        //    }
                        //    else
                        //    {
                        //        FightingActions.instance.anim.SetTrigger("DeequipGreatSword");
                        //        FightingActions.instance.anim.SetTrigger("DeequipBow");

                        //        Debug.Log("3" + storedItemBase);
                        //    }

                        //    InventoryManager.instance.RemoveHoldingWeight(EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase.weight, 1);
                        //    InventoryManager.instance.inventory.AddItem(EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase, 1);

                        //    for (int x = 0; x < EquippingManager.instance.weaponParentObj.transform.childCount; x++)
                        //    {
                        //        if (EquippingManager.instance.weaponParentObj.transform.GetChild(x).GetComponent<Item>().iBP ==
                        //            EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase)
                        //        {
                        //            EquippingManager.instance.leftWeaponES.transform.GetChild(x).gameObject.SetActive(false);
                        //        }
                        //    }
                        //}

                        EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase = storedItemBase;
                        EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedAmount = 1;

                        //EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedAmountTxt.gameObject.SetActive(false);

                        EquippingManager.instance.leftWeaponES.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                        EquippingManager.instance.leftWeaponES.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = storedItemBase.itemSprite;

                        InventoryManager.instance.AddHoldingWeight(storedItemBase.weight, 1);

                        InventoryManager.instance.inventory.RemoveItem(EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase, 1);

                        //storedItemBase

                        //var eventSystem = EventSystem.current;
                        //eventSystem.SetSelectedGameObject(null);
                        //eventSystem.SetSelectedGameObject(EquippingManager.instance.rightWeaponES.gameObject, new BaseEventData(eventSystem));

                        //FightingActions.instance.anim.SetTrigger("DeequipGreatSword");

                        Debug.Log("IS HERE!2");
                        break;
                    }
                }

                //for (int i = 0; i < EquippingManager.instance.weaponParentObj.transform.childCount; i++)
                //{
                //    HandleEquippingWeapon(EquippingManager.instance.leftWeaponES, EquippingManager.instance.weaponParentObj, FightingActions.equippedWeaponL, 
                //        InventoryManager.instance.currClickedBtn.storedItemBase, amountToStore, i);
                //}

                if (storedItemBase == EquippingManager.instance.glovesIB && !EquippingManager.instance.glovesGO.activeSelf)
                {
                    EquippingManager.instance.glovesGO.SetActive(true);
                    EquippingManager.instance.glovesGO2.SetActive(true);

                    InventoryManager.instance.AddHoldingWeight(EquippingManager.instance.glovesIB.weight, 1);

                    InventoryManager.instance.inventory.RemoveItem(EquippingManager.instance.glovesIB, 1);

                    EquippingManager.instance.glovesES.gameObject.GetComponent<ClickableInventorySlot>().storedItemBase = EquippingManager.instance.glovesIB;
                    EquippingManager.instance.glovesES.GetComponent<ClickableInventorySlot>().storedAmount = 1;

                    EquippingManager.instance.glovesES.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                    EquippingManager.instance.glovesES.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = EquippingManager.instance.glovesIB.itemSprite;
                }
                else if (storedItemBase == EquippingManager.instance.pauldronsIB && !EquippingManager.instance.pauldronsGO.activeSelf)
                {
                    EquippingManager.instance.pauldronsGO.SetActive(true);
                    EquippingManager.instance.pauldronsGO2.SetActive(true);

                    InventoryManager.instance.AddHoldingWeight(EquippingManager.instance.pauldronsIB.weight, 1);

                    InventoryManager.instance.inventory.RemoveItem(EquippingManager.instance.pauldronsIB, 1);

                    EquippingManager.instance.pauldronsES.gameObject.GetComponent<ClickableInventorySlot>().storedItemBase = EquippingManager.instance.pauldronsIB;
                    EquippingManager.instance.pauldronsES.GetComponent<ClickableInventorySlot>().storedAmount = 1;

                    EquippingManager.instance.pauldronsES.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                    EquippingManager.instance.pauldronsES.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = EquippingManager.instance.pauldronsIB.itemSprite;
                }
                else if (storedItemBase == EquippingManager.instance.poleynsIB && !EquippingManager.instance.poleynsGO.activeSelf)
                {
                    EquippingManager.instance.poleynsGO.SetActive(true);
                    EquippingManager.instance.poleynsGO2.SetActive(true);

                    InventoryManager.instance.AddHoldingWeight(EquippingManager.instance.poleynsIB.weight, 1);

                    InventoryManager.instance.inventory.RemoveItem(EquippingManager.instance.poleynsIB, 1);

                    EquippingManager.instance.poleynsES.gameObject.GetComponent<ClickableInventorySlot>().storedItemBase = EquippingManager.instance.poleynsIB;
                    EquippingManager.instance.poleynsES.GetComponent<ClickableInventorySlot>().storedAmount = 1;

                    EquippingManager.instance.poleynsES.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                    EquippingManager.instance.poleynsES.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = EquippingManager.instance.poleynsIB.itemSprite;

                    Debug.Log("ßßßßßßßßßßßßßßßßßßßßßßßß");
                }
            }
        }
        else
        {
            //for (int i = 0; i < EquippingManager.instance.rightWeaponParentObj.transform.childCount; i++)
            //{
            //    HandleEquippingWeapon(EquippingManager.instance.rightWeaponES, EquippingManager.instance.rightWeaponParentObj, FightingActions.equippedWeaponR, ibToUse, amountToStore, i);
            //}

            //for (int i = 0; i < EquippingManager.instance.weaponParentObj.transform.childCount; i++)
            //{
            //    HandleEquippingWeapon(EquippingManager.instance.leftWeaponES, EquippingManager.instance.weaponParentObj, FightingActions.equippedWeaponL, ibToUse, amountToStore, i);
            //}

            for (int i = 0; i < EquippingManager.instance.rightWeaponParentObj.transform.childCount; i++)
            {
                if (EquippingManager.instance.rightWeaponParentObj.transform.GetChild(i).GetComponent<Item>().iBP == ibToUse)
                {
                    storedItemBase = ibToUse;

                    EquippingManager.instance.rightWeaponParentObj.transform.GetChild(i).gameObject.SetActive(true);
                    FightingActions.instance.equippedWeaponR = EquippingManager.instance.rightWeaponParentObj.transform.GetChild(i).gameObject;

                    FightingActions.instance.GetWeapon();

                    if (EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase != null)
                    {
                        if (EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase.weaponType == ItemBaseProfile.WeaponType.bow)
                        {
                            //FightingActions.instance.anim.SetTrigger("DeequipBow");

                            FightingActions.instance.anim.ResetTrigger("BowIdle");
                            EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().ClearEquipmentSlot();

                            Debug.Log("1" + storedItemBase);
                        }
                    }

                    if (EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase != null)
                    {
                        InventoryManager.instance.RemoveHoldingWeight(EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase.weight, 1);
                        InventoryManager.instance.inventory.AddItem(EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase, 1);

                        for (int x = 0; x < EquippingManager.instance.rightWeaponParentObj.transform.childCount; x++)
                        {
                            if (EquippingManager.instance.rightWeaponParentObj.transform.GetChild(x).GetComponent<Item>().iBP ==
                                EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase)
                            {
                                EquippingManager.instance.rightWeaponParentObj.transform.GetChild(x).gameObject.SetActive(false);
                            }
                        }

                        //if (EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase.weaponType == ItemBaseProfile.WeaponType.greatsword)
                        //{
                        //    FightingActions.instance.anim.SetTrigger("DeequipGreatSword");
                        //}

                        //if (storedItemBase.weaponType == ItemBaseProfile.WeaponType.greatsword)
                        //{
                        //    EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().ClearEquipmentSlot();
                        //}
                    }

                    EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase = storedItemBase;
                    EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedAmount = 1;

                    //EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedAmountTxt.gameObject.SetActive(false);

                    EquippingManager.instance.rightWeaponES.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                    EquippingManager.instance.rightWeaponES.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = storedItemBase.itemSprite;

                    InventoryManager.instance.AddHoldingWeight(storedItemBase.weight, 1);

                    InventoryManager.instance.inventory.RemoveItem(EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase, 1);

                    //storedItemBase

                    //var eventSystem = EventSystem.current;
                    //eventSystem.SetSelectedGameObject(null);
                    //eventSystem.SetSelectedGameObject(EquippingManager.instance.rightWeaponES.gameObject, new BaseEventData(eventSystem));
                    Debug.Log("IS HERE!2");

                    //FightingActions.instance.anim.SetTrigger("DeequipBow");

                    break;
                }
            }

            for (int i = 0; i < EquippingManager.instance.weaponParentObj.transform.childCount; i++)
            {
                if (EquippingManager.instance.weaponParentObj.transform.GetChild(i).GetComponent<Item>().iBP == ibToUse)
                {
                    storedItemBase = ibToUse;

                    EquippingManager.instance.weaponParentObj.transform.GetChild(i).gameObject.SetActive(true);
                    FightingActions.instance.equippedWeaponL = EquippingManager.instance.weaponParentObj.transform.GetChild(i).gameObject;

                    FightingActions.instance.GetWeapon();

                    if (EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase != null)
                    {
                        if (EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase.weaponType == ItemBaseProfile.WeaponType.greatsword)
                        {
                            //FightingActions.instance.anim.SetTrigger("DeequipGreatSword");

                            FightingActions.instance.anim.ResetTrigger("GreatSwordIdle");
                            EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().ClearEquipmentSlot();

                            Debug.Log("1" + storedItemBase);
                        }
                    }

                    if (EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase != null)
                    {
                        InventoryManager.instance.RemoveHoldingWeight(EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase.weight, 1);
                        InventoryManager.instance.inventory.AddItem(EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase, 1);

                        for (int x = 0; x < EquippingManager.instance.weaponParentObj.transform.childCount; x++)
                        {
                            if (EquippingManager.instance.weaponParentObj.transform.GetChild(x).GetComponent<Item>().iBP ==
                                EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase)
                            {
                                EquippingManager.instance.weaponParentObj.transform.GetChild(x).gameObject.SetActive(false);
                            }
                        }

                        if (EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase.weaponType == ItemBaseProfile.WeaponType.bow)
                        {
                            FightingActions.instance.anim.SetTrigger("DeequipBow");
                        }
                    }

                    if (storedItemBase.weaponType == ItemBaseProfile.WeaponType.bow)
                    {
                        EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().ClearEquipmentSlot();

                        FightManager.instance.UpdateArrowHUDDisplay();
                        GameManager.instance.arrowHUDDisplayGO.SetActive(true);
                    }

                    EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase = storedItemBase;
                    EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedAmount = 1;

                    //EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedAmountTxt.gameObject.SetActive(false);

                    EquippingManager.instance.leftWeaponES.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                    EquippingManager.instance.leftWeaponES.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = storedItemBase.itemSprite;

                    InventoryManager.instance.AddHoldingWeight(storedItemBase.weight, 1);

                    InventoryManager.instance.inventory.RemoveItem(EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase, 1);

                    //storedItemBase

                    //var eventSystem = EventSystem.current;
                    //eventSystem.SetSelectedGameObject(null);
                    //eventSystem.SetSelectedGameObject(EquippingManager.instance.rightWeaponES.gameObject, new BaseEventData(eventSystem));
                    Debug.Log("IS HERE!2");

                    //FightingActions.instance.anim.SetTrigger("DeequipGreatSword");

                    break;
                }
            }

            if (ibToUse == EquippingManager.instance.glovesIB && !EquippingManager.instance.glovesGO.activeSelf)
            {
                EquippingManager.instance.glovesGO.SetActive(true);
                EquippingManager.instance.glovesGO2.SetActive(true);

                InventoryManager.instance.AddHoldingWeight(ibToUse.weight, 1);

                InventoryManager.instance.inventory.RemoveItem(ibToUse, 1);

                EquippingManager.instance.glovesES.gameObject.GetComponent<ClickableInventorySlot>().storedItemBase = EquippingManager.instance.glovesIB;
                EquippingManager.instance.glovesES.GetComponent<ClickableInventorySlot>().storedAmount = 1;

                EquippingManager.instance.glovesES.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                EquippingManager.instance.glovesES.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = EquippingManager.instance.glovesIB.itemSprite;
            }
            else if (ibToUse == EquippingManager.instance.pauldronsIB && !EquippingManager.instance.pauldronsGO.activeSelf)
            {
                EquippingManager.instance.pauldronsGO.SetActive(true);
                EquippingManager.instance.pauldronsGO2.SetActive(true);

                InventoryManager.instance.AddHoldingWeight(ibToUse.weight, 1);

                InventoryManager.instance.inventory.RemoveItem(ibToUse, 1);

                EquippingManager.instance.pauldronsES.gameObject.GetComponent<ClickableInventorySlot>().storedItemBase = EquippingManager.instance.pauldronsIB;
                EquippingManager.instance.pauldronsES.GetComponent<ClickableInventorySlot>().storedAmount = 1;

                EquippingManager.instance.pauldronsES.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                EquippingManager.instance.pauldronsES.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = EquippingManager.instance.pauldronsIB.itemSprite;
            }
            else if (ibToUse == EquippingManager.instance.poleynsIB && !EquippingManager.instance.poleynsGO.activeSelf)
            {
                EquippingManager.instance.poleynsGO.SetActive(true);
                EquippingManager.instance.poleynsGO2.SetActive(true);

                InventoryManager.instance.AddHoldingWeight(ibToUse.weight, 1);

                InventoryManager.instance.inventory.RemoveItem(ibToUse, 1);

                EquippingManager.instance.poleynsES.gameObject.GetComponent<ClickableInventorySlot>().storedItemBase = EquippingManager.instance.poleynsIB;
                EquippingManager.instance.poleynsES.GetComponent<ClickableInventorySlot>().storedAmount = 1;

                EquippingManager.instance.poleynsES.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                EquippingManager.instance.poleynsES.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = EquippingManager.instance.poleynsIB.itemSprite;

                Debug.Log("ßßßßßßßßßßßßßßßßßßßßßßßß");
            }
        }

        if (FightingActions.instance.equippedWeaponR != null)
        {
            FightingActions.lastWeapon = FightingActions.instance.equippedWeaponR.GetComponent<Item>().iBP;
        }
        else
        {
            FightingActions.lastWeapon = null;
        }

        InventoryManager.instance.DisplayItemsOfCategory();
    }

    public void HandleEquippingWeapon(EquipmentSlot eSlot, GameObject weaponParentObj, GameObject equippedWeaponLOrR, ItemBaseProfile ibToUse, int amountToStore, int i)
    {
        if (weaponParentObj.transform.GetChild(i).GetComponent<Item>().iBP == ibToUse)
        {
            storedItemBase = ibToUse;

            weaponParentObj.transform.GetChild(i).gameObject.SetActive(true);
            equippedWeaponLOrR = weaponParentObj.transform.GetChild(i).gameObject;

            FightingActions.instance.GetWeapon();

            if (eSlot.GetComponent<ClickableInventorySlot>().storedItemBase != null)
            {
                InventoryManager.instance.RemoveHoldingWeight(eSlot.GetComponent<ClickableInventorySlot>().storedItemBase.weight, 1);
                InventoryManager.instance.inventory.AddItem(eSlot.GetComponent<ClickableInventorySlot>().storedItemBase, 1);
            }

            eSlot.GetComponent<ClickableInventorySlot>().storedItemBase = storedItemBase;
            eSlot.GetComponent<ClickableInventorySlot>().storedAmount = 1;

            //EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedAmountTxt.gameObject.SetActive(false);

            eSlot.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
            eSlot.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = storedItemBase.itemSprite;

            InventoryManager.instance.AddHoldingWeight(storedItemBase.weight, 1);

            InventoryManager.instance.inventory.RemoveItem(/*eSlot.GetComponent<ClickableInventorySlot>().*/storedItemBase, 1);

            //storedItemBase

            //var eventSystem = EventSystem.current;
            //eventSystem.SetSelectedGameObject(null);
            //eventSystem.SetSelectedGameObject(EquippingManager.instance.rightWeaponES.gameObject, new BaseEventData(eventSystem));
        }
    }

    public void EquipItemToHotbar(ItemBaseProfile ibToUse, int amountToStore)
    {
        var oldIB = storedItemBase;
        ItemBaseProfile newIB;

        if (ibToUse != null)
        {
            newIB = ibToUse;
        }
        else
        {
            newIB = InventoryManager.instance.currClickedBtn.storedItemBase;
        }

        if (oldIB != null)
        {
            InventoryManager.instance.inventory.AddItem(oldIB, storedAmount);

            //if (storedItemBase.itemType == ItemBaseProfile.ItemType.weapon)
            //{
            //    InventoryManager.instance.RemoveHoldingWeight(oldIB.weight, 1);
            //}
            //else
            //{
            InventoryManager.instance.RemoveHoldingWeight(oldIB.weight, storedAmount);
            //}
        }

        //if (newIB != null)
        //{
        storedItemBase = newIB;
        storedAmount = Convert.ToInt32(HotbarManager.instance.hbHMScreen.howManyHBSlider.value);

        this.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = newIB.itemSprite;

        //Debug.Log(this.gameObject);

        storedAmountTxt.text = storedAmount.ToString();

        if (ibToUse != null)
        {
            storedAmount = amountToStore/* - Convert.ToInt32(HotbarManager.instance.hbHMScreen.howManyHBSlider.value)*/;
            storedAmountTxt.text = amountToStore.ToString();
        }
        else
        {
            InventoryManager.instance.currClickedBtn.storedAmount = InventoryManager.instance.currClickedBtn.storedAmount - Convert.ToInt32(HotbarManager.instance.hbHMScreen.howManyHBSlider.value);
            InventoryManager.instance.currClickedBtn.storedAmountTxt.text = InventoryManager.instance.currClickedBtn.storedAmount.ToString();
        }

        correspondingMainScreenHotbarSlotBtn.GetComponent<HotbarSlotButton>().itemSpriteImg.enabled = true;
        correspondingMainScreenHotbarSlotBtn.GetComponent<HotbarSlotButton>().itemSpriteImg.sprite = storedItemBase.itemSprite;

        if (ibToUse != null)
        {
            correspondingMainScreenHotbarSlotBtn.GetComponent<HotbarSlotButton>().itemAmountTxt.text = storedAmount.ToString();
        }
        else
        {
            correspondingMainScreenHotbarSlotBtn.GetComponent<HotbarSlotButton>().itemAmountTxt.text = Convert.ToInt32(HotbarManager.instance.hbHMScreen.howManyHBSlider.value).ToString();
        }

        //if (storedItemBase.itemType == ItemBaseProfile.ItemType.weapon)
        //{
        //    InventoryManager.instance.AddHoldingWeight(newIB.weight, 1);
        //}
        //else
        //{
        InventoryManager.instance.AddHoldingWeight(newIB.weight, storedAmount);
        //}
        //}
        //else
        //{
        //    this.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = null;

        //    correspondingMainScreenHotbarSlotBtn.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = false;
        //    correspondingMainScreenHotbarSlotBtn.GetComponent<HotbarSlotButton>().itemAmountTxt.text = "";
        //}

        //if (newIB != null)
        //{
        //    InventoryManager.instance.inventory.RemoveItem(storedItemBase, storedAmount);

        //    InventoryManager.currCIS.storedItemBase = newIB;
        //}

        //InventoryManager.instance.currClickedBtn.storedAmount - s

        animator.enabled = false;
        boarder.gameObject.SetActive(false);

        InventoryManager.instance.hotbarObj.transform.parent = InventoryManager.instance.oldHotbarParentTrans;

        this.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;

        InventoryManager.instance.selectHotbarSlotScreen.SetActive(false);

        //var eventSystem = EventSystem.current;
        //eventSystem.SetSelectedGameObject(null);
        //eventSystem.SetSelectedGameObject(InventoryManager.instance.currClickedBtn.gameObject, new BaseEventData(eventSystem));

        InventoryManager.instance.inventory.RemoveItem(newIB, Convert.ToInt32(HotbarManager.instance.hbHMScreen.howManyHBSlider.value));

        InventoryManager.instance.currClickedBtn = null;

        InventoryManager.instance.DisplayItemsOfCategory();
    }

    public void ClearHotbarSlot()
    {
        if (storedItemBase != null)
        {
            InventoryManager.instance.inventory.AddItem(storedItemBase, storedAmount);

            InventoryManager.instance.RemoveHoldingWeight(storedItemBase.weight, storedAmount);
        }

        this.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = false;

        storedItemBase = null;
        storedAmount = 0;

        storedAmountTxt.text = "";

        correspondingMainScreenHotbarSlotBtn.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = false;
        correspondingMainScreenHotbarSlotBtn.GetComponent<HotbarSlotButton>().itemAmountTxt.text = "";

        InventoryManager.instance.DisplayItemsOfCategory();

        InventoryManager.instance.itemInfoPopUp.gameObject.SetActive(false);
    }

    public void ClearEquipmentSlot()
    {
        if (storedItemBase != null)
        {
            InventoryManager.instance.inventory.AddItem(storedItemBase, 1);

            this.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = false;

            if (storedItemBase.weaponType == ItemBaseProfile.WeaponType.bow)
            {
                FightingActions.instance.anim.SetTrigger("DeequipBow");

                GameManager.instance.arrowHUDDisplayGO.SetActive(false);
            }

            if (storedItemBase.weaponType == ItemBaseProfile.WeaponType.greatsword)
            {
                FightingActions.instance.anim.SetTrigger("DeequipGreatSword");
            }

            if (storedItemBase == EquippingManager.instance.glovesIB && EquippingManager.instance.glovesGO.activeSelf)
            {
                EquippingManager.instance.glovesGO.SetActive(false);
                EquippingManager.instance.glovesGO2.SetActive(false);

                InventoryManager.instance.RemoveHoldingWeight(EquippingManager.instance.glovesIB.weight, 1);

                //InventoryManager.instance.inventory.AddItem(EquippingManager.instance.glovesIB, 1);

                EquippingManager.instance.glovesES.gameObject.GetComponent<ClickableInventorySlot>().storedItemBase = null;

                EquippingManager.instance.glovesES.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
            }
            else if (storedItemBase == EquippingManager.instance.pauldronsIB && EquippingManager.instance.pauldronsGO.activeSelf)
            {
                EquippingManager.instance.pauldronsGO.SetActive(false);
                EquippingManager.instance.pauldronsGO2.SetActive(false);

                InventoryManager.instance.RemoveHoldingWeight(EquippingManager.instance.pauldronsIB.weight, 1);

                //InventoryManager.instance.inventory.AddItem(EquippingManager.instance.pauldronsIB, 1);

                EquippingManager.instance.pauldronsES.gameObject.GetComponent<ClickableInventorySlot>().storedItemBase = null;

                EquippingManager.instance.pauldronsES.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
            }
            else if (storedItemBase == EquippingManager.instance.poleynsIB && EquippingManager.instance.poleynsGO.activeSelf)
            {
                EquippingManager.instance.poleynsGO.SetActive(false);
                EquippingManager.instance.poleynsGO2.SetActive(false);

                InventoryManager.instance.RemoveHoldingWeight(EquippingManager.instance.poleynsIB.weight, 1);

                //InventoryManager.instance.inventory.AddItem(EquippingManager.instance.poleynsIB, 1);

                EquippingManager.instance.poleynsES.gameObject.GetComponent<ClickableInventorySlot>().storedItemBase = null;

                EquippingManager.instance.poleynsES.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
            }

            InventoryManager.instance.DisplayItemsOfCategory();

            if (storedItemBase != null && EquippingManager.instance.rightWeaponParentObj != null)
            {
                for (int i = 0; i < EquippingManager.instance.rightWeaponParentObj.transform.childCount; i++)
                {
                    if (EquippingManager.instance.rightWeaponParentObj.transform.GetChild(i).GetComponent<Item>().iBP == storedItemBase)
                    {
                        EquippingManager.instance.rightWeaponParentObj.transform.GetChild(i).gameObject.SetActive(false);
                        FightingActions.instance.equippedWeaponR = null;

                        //FightingActions.instance.GetWeapon();

                        InventoryManager.instance.RemoveHoldingWeight(storedItemBase.weight, 1);

                        EquippingManager.instance.rightWeaponES.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
                        EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase = null;

                        break;
                    }
                }

                InventoryManager.instance.DisplayItemsOfCategory();
            }

            if (storedItemBase != null && EquippingManager.instance.weaponParentObj != null)
            {
                for (int i = 0; i < EquippingManager.instance.weaponParentObj.transform.childCount; i++)
                {
                    if (EquippingManager.instance.weaponParentObj.transform.GetChild(i).GetComponent<Item>().iBP == storedItemBase)
                    {
                        EquippingManager.instance.weaponParentObj.transform.GetChild(i).gameObject.SetActive(false);
                        FightingActions.instance.equippedWeaponL = null;

                        //FightingActions.instance.GetWeapon();

                        InventoryManager.instance.RemoveHoldingWeight(storedItemBase.weight, 1);

                        EquippingManager.instance.leftWeaponES.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
                        EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase = null;

                        //if (storedItemBase != null)
                        //{
                        //    if (storedItemBase.weaponType == ItemBaseProfile.WeaponType.bow)
                        //    {
                        //        FightingActions.instance.anim.SetTrigger("DeequipGreatSword");
                        //    }
                        //    else if (storedItemBase.weaponType == ItemBaseProfile.WeaponType.greatsword)
                        //    {
                        //        FightingActions.instance.anim.SetTrigger("DeequipBow");
                        //    }
                        //    else
                        //    {
                        //        FightingActions.instance.anim.SetTrigger("DeequipGreatSword");
                        //        FightingActions.instance.anim.SetTrigger("DeequipBow");
                        //    }
                        //}

                        break;
                    }
                }

                InventoryManager.instance.DisplayItemsOfCategory();
            }

            InventoryManager.instance.itemInfoPopUp.gameObject.SetActive(false);

            //if (storedItemBase != null && EquippingManager.instance.leftWeaponES != null)
            //{
            //    for (int i = 0; i < EquippingManager.instance.leftWeaponES.transform.childCount; i++)
            //    {
            //        if (EquippingManager.instance.leftWeaponES.transform.GetChild(i).GetComponent<Item>().iBP == storedItemBase)
            //        {
            //            EquippingManager.instance.leftWeaponES.transform.GetChild(i).gameObject.SetActive(false);
            //            FightingActions.instance.equippedWeaponL = null;

            //            //FightingActions.instance.GetWeapon();

            //            EquippingManager.instance.leftWeaponES.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
            //            EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase = null;

            //            break;
            //        }
            //    }
            //}

            //storedAmountTxt.text = "";
        }

        this.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = false;
    }

    // Press Enter
    public void SelectHotbarSlotToEquipTo()
    {
        InventoryManager.instance.currClickedBtn = this;

        for (int i = 1; i < InventoryManager.instance.newHotbarParentTrans.childCount; i++)
        {
            Destroy(InventoryManager.instance.newHotbarParentTrans.transform.GetChild(i).gameObject);
        }

        var cISCopy = Instantiate(this.gameObject, this.gameObject.transform.parent);

        cISCopy.transform.position = this.gameObject.transform.position;

        cISCopy.GetComponent<LayoutElement>().ignoreLayout = false;

        cISCopy.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);

        cISCopy.transform.parent = InventoryManager.instance.newHotbarParentTrans;

        cISCopy.gameObject.name = "CISCOPY";

        InventoryManager.instance.hotbarObj.transform.parent = InventoryManager.instance.newHotbarParentTrans;

        InventoryManager.instance.selectHotbarSlotScreen.SetActive(true);

        cISCopy.GetComponent<Animator>().enabled = false;
        cISCopy.GetComponent<ClickableInventorySlot>().boarder.color = Color.white;
        cISCopy.GetComponent<ClickableInventorySlot>().boarder.gameObject.SetActive(true);

        HotbarManager.instance.howManyToHotbarScreen.GetComponent<HotbarHowManyScreen>().howManyHBSlider.maxValue = storedAmount;
        HotbarManager.instance.howManyToHotbarScreen.GetComponent<HotbarHowManyScreen>().SetStartValues(storedAmount);

        //var eventSystem = EventSystem.current;
        //eventSystem.SetSelectedGameObject(null);
        //eventSystem.SetSelectedGameObject(HotbarManager.instance.howManyToHotbarScreen.GetComponent<HotbarHowManyScreen>().howManyHBSlider.gameObject, new BaseEventData(eventSystem));

        Debug.Log("EQUIPPED");

        if (clickableSlotType == ClickableSlotType.inventorySlot && storedItemBase != null)
        {
            TutorialManager.instance.CheckIfTutorialIsAlreadyCompleted(InventoryManager.instance.hotbarTutorial);
        }
    }

    public void OpenBookScrollOrNoteUI(GameObject goToActivate)
    {
        GameManager.currBookOrNote = storedItemBase;

        InventoryManager.instance.bookUI.SetActive(false);
        InventoryManager.instance.scrollUI.SetActive(false);
        InventoryManager.instance.noteUI.SetActive(false);

        if (goToActivate == InventoryManager.instance.bookUI)
        {
            InventoryManager.instance.bookHandler.StartDisplayingPages();
        }
        else if (goToActivate == InventoryManager.instance.noteUI)
        {
            InventoryManager.instance.noteTxt.text = storedItemBase.noteTxtString;
        }

        goToActivate.SetActive(true);

        InventoryManager.instance.bNOSScreenParent.SetActive(true);
    }

    public void DisplayAllItemInformationsOnClick()
    {
        if (InventoryManager.instance.currItemNameTxt != null)
        {
            if (storedItemBase != null)
            {
                if (isNewSymbol != null)
                {
                    storedItemBase.isNew = false;
                    isNewSymbol.gameObject.SetActive(false);
                }

                InventoryManager.instance.currItemNameTxt.text = storedItemBase.itemName;
                InventoryManager.instance.currItemDescriptionTxt.text = storedItemBase.itemDescription;
                //InventoryManager.instance.currItemAmountInInvTxt.text = invSlot.itemAmount.ToString();
                //InventoryManager.instance.currItemSellPriceTxt.text = storedItemBase.sellingPrice.ToString();

                InventoryManager.currIBP = storedItemBase;
                //InventoryManager.currIS = invSlot;
            }
            else if (storedItemBase == null)
            {
                InventoryManager.instance.currItemNameTxt.text = "";
                InventoryManager.instance.currItemDescriptionTxt.text = "";
            }
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (clickableSlotType == ClickableSlotType.hotbarSlot && clickableSlotType == ClickableSlotType.shopSlot)
            {
                SelectAction();
            }
            else
            {
                if (InventoryManager.instance.currClickedBtn == null || InventoryManager.instance.currClickedBtn != this && this.gameObject.name != "CISCOPY")
                {
                    SelectAction();
                }
            }
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (clickableSlotType == ClickableSlotType.inventorySlot && storedItemBase != null && storedItemBase.itemType == ItemBaseProfile.ItemType.weapon)
        {
            TutorialManager.instance.CheckIfTutorialIsAlreadyCompleted(InventoryManager.instance.equipmentTutorial);
        }

        Debug.Log("GHBJNK");

        SelectInventorySlot();
        
        // ---------------------------- WIP: Hier noch die Abfrage fürs Equipment einfügen.
        //else if (clickableSlotType == ClickableSlotType.inventorySlot && storedItemBase != null && storedItemBase.itemType == ItemBaseProfile.ItemType.)
        //{
        //    TutorialManager.instance.CheckIfTutorialIsAlreadyCompleted(InventoryManager.instance.equipmentTutorial);
        //}

        //if (clickableSlotType == ClickableSlotType.shopSlot)
        //{
        //    if (storedItemBase.itemType != ItemBaseProfile.ItemType.weapon)
        //    {

        if (InventoryManager.instance.inventoryScreen.activeSelf)
        {
            if (storedItemBase != null)
            {
                InventoryManager.instance.itemInfoPopUp.gameObject.GetComponent<ItemInfoPopUp>().SetItemInformationsToDisplay(storedItemBase, isShopPlayerItem);
                InventoryManager.instance.itemInfoPopUp.gameObject.SetActive(true);
            }
            else
            {
                InventoryManager.instance.itemInfoPopUp.gameObject.SetActive(false);
            }

            if (isNewSymbol != null)
            {
                storedItemBase.isNew = false;
                isNewSymbol.gameObject.SetActive(false);
            }
        }
        else
        {
            if (storedItemBase != null)
            {
                if (isShopPlayerItem)
                {
                    ShopManager.instance.itemInfoPopUpLeft.gameObject.GetComponent<ItemInfoPopUp>().SetItemInformationsToDisplay(storedItemBase, isShopPlayerItem);
                    ShopManager.instance.itemInfoPopUpLeft.gameObject.SetActive(true);
                }
                else
                {
                    ShopManager.instance.itemInfoPopUpRight.gameObject.GetComponent<ItemInfoPopUp>().SetItemInformationsToDisplay(storedItemBase, isShopPlayerItem);
                    ShopManager.instance.itemInfoPopUpRight.gameObject.SetActive(true);
                }

                if (isNewSymbol != null)
                {
                    storedItemBase.isNew = false;
                    isNewSymbol.gameObject.SetActive(false);
                }
            }
        }

        //    }
        //    else
        //    {
        //        ShopManager.instance.BuyOrSellItem(storedItemBase, 1);
        //    }
        //}
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (clickableSlotType != ClickableSlotType.shopSlot && InventoryManager.instance.currItemNameTxt != null)
        {
            InventoryManager.instance.currItemNameTxt.text = "";
            InventoryManager.instance.currItemDescriptionTxt.text = "";
        }

        animator.enabled = false;
        boarder.gameObject.SetActive(false);

        if (clickableSlotType == ClickableSlotType.shopSlot)
        {
            ShopManager.instance.itemInfoPopUpLeft.gameObject.SetActive(false);
            ShopManager.instance.itemInfoPopUpRight.gameObject.SetActive(false);
        }
        else if (clickableSlotType == ClickableSlotType.equipmentSlot || clickableSlotType == ClickableSlotType.inventorySlot)
        {
            InventoryManager.instance.itemInfoPopUp.gameObject.SetActive(false);
        }

        //this.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;
    }   
}
