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

    public void Awake()
    {

    }

    public void Start()
    {
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
                storedAmountTxt.text = storedAmount.ToString();

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
            DisplayAllItemInformationsOnClick();
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
            if (storedItemBase.itemType != ItemBaseProfile.ItemType.weapon && storedItemBase.itemType != ItemBaseProfile.ItemType.bookOrNote)
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
            if (storedItemBase.itemType != ItemBaseProfile.ItemType.weapon && storedItemBase.itemType != ItemBaseProfile.ItemType.bookOrNote)
            {
                SelectHotbarSlotToEquipTo();
            }
            else if (storedItemBase.itemType == ItemBaseProfile.ItemType.weapon)
            {
                EquipItemToEquipment(null, 0);
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
            if (InventoryManager.instance.currClickedBtn.storedItemBase != null && EquippingManager.instance.rightWeaponParentObj != null)
            {
                for (int i = 0; i < EquippingManager.instance.rightWeaponParentObj.transform.childCount; i++)
                {
                    if (EquippingManager.instance.rightWeaponParentObj.transform.GetChild(i).GetComponent<Item>().iBP == storedItemBase)
                    {
                        storedItemBase = InventoryManager.instance.currClickedBtn.storedItemBase;

                        EquippingManager.instance.rightWeaponParentObj.transform.GetChild(i).gameObject.SetActive(true);
                        FightingActions.instance.equippedWeaponR = EquippingManager.instance.rightWeaponParentObj.transform.GetChild(i).gameObject;

                        FightingActions.instance.GetWeapon();

                        if (EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase != null)
                        {
                            InventoryManager.instance.RemoveHoldingWeight(EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase.weight, 1);
                            InventoryManager.instance.inventory.AddItem(EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase, 1);
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

                        break;
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < EquippingManager.instance.rightWeaponParentObj.transform.childCount; i++)
            {
                if (EquippingManager.instance.rightWeaponParentObj.transform.GetChild(i).GetComponent<Item>().iBP == ibToUse)
                {
                    storedItemBase = ibToUse;

                    EquippingManager.instance.rightWeaponParentObj.transform.GetChild(i).gameObject.SetActive(true);
                    FightingActions.instance.equippedWeaponR = EquippingManager.instance.rightWeaponParentObj.transform.GetChild(i).gameObject;

                    FightingActions.instance.GetWeapon();

                    if (EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase != null)
                    {
                        InventoryManager.instance.RemoveHoldingWeight(EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase.weight, 1);
                        InventoryManager.instance.inventory.AddItem(EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase, 1);
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

                    break;
                }
            }
        }

        InventoryManager.instance.DisplayItemsOfCategory();
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

            // --------------------- Linke Hand fehlt noch

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

    public void DisplayAllItemInformationsOnClick()
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

    public void OnSelect(BaseEventData eventData)
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        SelectInventorySlot();

        if (clickableSlotType == ClickableSlotType.inventorySlot && storedItemBase != null && storedItemBase.itemType == ItemBaseProfile.ItemType.weapon)
        {
            TutorialManager.instance.CheckIfTutorialIsAlreadyCompleted(InventoryManager.instance.equipmentTutorial);
        } // ---------------------------- WIP: Hier noch die Abfrage fürs Equipment einfügen.
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
        }
        else
        {
            ShopManager.instance.itemInfoPopUp.gameObject.GetComponent<ItemInfoPopUp>().SetItemInformationsToDisplay(storedItemBase, isShopPlayerItem);
            ShopManager.instance.itemInfoPopUp.gameObject.SetActive(true);
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
        InventoryManager.instance.currItemNameTxt.text = "";
        InventoryManager.instance.currItemDescriptionTxt.text = "";

        animator.enabled = false;
        boarder.gameObject.SetActive(false);

        if (clickableSlotType == ClickableSlotType.shopSlot)
        {
            ShopManager.instance.itemInfoPopUp.gameObject.SetActive(false);
        }
        else if (clickableSlotType == ClickableSlotType.equipmentSlot || clickableSlotType == ClickableSlotType.inventorySlot)
        {
            InventoryManager.instance.itemInfoPopUp.gameObject.SetActive(false);
        }

        //this.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = true;
    }
}
