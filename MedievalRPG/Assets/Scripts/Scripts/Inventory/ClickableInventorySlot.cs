using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class ClickableInventorySlot : MonoBehaviour, ISelectHandler
{
    public enum ClickableSlotType
    {
        inventorySlot,
        equipmentSlot,
        hotbarSlot,
        categoryButton
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

    public void Awake()
    {

    }

    public void Start()
    {
        if (clickableSlotType != ClickableSlotType.categoryButton)
        {
            if (this.gameObject.GetComponent<Button>() != null)
            {
                this.gameObject.GetComponent<Button>().onClick.AddListener(SelectAction);

                //var eventSystem = EventSystem.current;
                //eventSystem.SetSelectedGameObject(null);
                //eventSystem.SetSelectedGameObject(this.gameObject, new BaseEventData(eventSystem));
            }

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

        if (InventoryManager.instance.currClickedBtn == null/* && clickableSlotType != ClickableSlotType.categoryButton*/)
        {
            DisplayAllItemInformationsOnClick();
        }
        //else if (InventoryManager.instance.currClickedBtn == null && clickableSlotType != ClickableSlotType.categoryButton)
        //{

        //}
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
                EquipItemToHotbar();
            }
            else if (storedItemBase != null)
            {
                ClearHotbarSlot();
                // Zahlen m�ssen geupdated werden
            }
        }
        else if (clickableSlotType == ClickableSlotType.equipmentSlot)
        {
            if (storedItemBase != null)
            {
                ClearEquipmentSlot();
            }
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
                EquipItemToEquipment();
            }
        }
        else if (clickableSlotType == ClickableSlotType.hotbarSlot)
        {
            if (InventoryManager.instance.selectHotbarSlotScreen.activeSelf)
            {
                EquipItemToHotbar();
            }
            else if (storedItemBase != null)
            {
                ClearHotbarSlot();
                // Zahlen m�ssen geupdated werden
            }
        }
        else if (clickableSlotType == ClickableSlotType.equipmentSlot)
        {
            if (storedItemBase != null)
            {
                ClearEquipmentSlot();
            }
        }

        InventoryManager.instance.weightTxt.text = InventoryManager.instance.currHoldingWeight + " / " + InventoryManager.instance.maxHoldingWeight;
    }

    public void EquipItemToEquipment()
    {
        InventoryManager.instance.currClickedBtn = this;
        //if (currEquippedItem != null)
        //{
        //    InventoryManager.instance.inventory.AddItem(currEquippedItem, 1);

        //    InventoryManager.instance.RemoveHoldingWeight(currEquippedItem.weight, 1);
        //}

        //if (newItemToEquip != null)
        //{
        //    InventoryManager.instance.AddHoldingWeight(newItemToEquip.weight, 1);
        //}

        //if (currEquippedItem != null && EquippingManager.instance.weaponParentObj != null)
        //{
        //    for (int i = 0; i < EquippingManager.instance.weaponParentObj.transform.childCount; i++)
        //    {
        //        if (EquippingManager.instance.weaponParentObj.transform.GetChild(i).GetComponent<Item>().iBP.itemPrefab == currEquippedItem.itemPrefab)
        //        {
        //            EquippingManager.instance.weaponParentObj.transform.GetChild(i).gameObject.SetActive(false);

        //            break;
        //        }
        //    }
        //}

        //if (InventoryManager.instance.currClickedBtn.storedItemBase != null && EquippingManager.instance.leftWeaponES != null)
        //{
        //    for (int i = 0; i < EquippingManager.instance.leftWeaponES.transform.childCount; i++)
        //    {
        //        if (EquippingManager.instance.leftWeaponES.transform.GetChild(i).GetComponent<Item>().iBP == storedItemBase)
        //        {
        //            EquippingManager.instance.leftWeaponES.transform.GetChild(i).gameObject.SetActive(true);
        //            FightingActions.instance.equippedWeaponL = EquippingManager.instance.leftWeaponES.transform.GetChild(i).gameObject;

        //            FightingActions.instance.GetWeapon();

        //            EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase = storedItemBase;
        //            EquippingManager.instance.leftWeaponES.GetComponent<ClickableInventorySlot>().storedAmount = storedAmount;

        //            EquippingManager.instance.leftWeaponES.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
        //            EquippingManager.instance.leftWeaponES.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = storedItemBase.itemSprite;

        //            var eventSystem = EventSystem.current;
        //            eventSystem.SetSelectedGameObject(null);
        //            eventSystem.SetSelectedGameObject(EquippingManager.instance.leftWeaponES.gameObject, new BaseEventData(eventSystem));

        //            break;
        //        }
        //    }
        //}

        if (InventoryManager.instance.currClickedBtn.storedItemBase != null && EquippingManager.instance.rightWeaponParentObj != null)
        {
            for (int i = 0; i < EquippingManager.instance.rightWeaponParentObj.transform.childCount; i++)
            {
                if (EquippingManager.instance.rightWeaponParentObj.transform.GetChild(i).GetComponent<Item>().iBP == storedItemBase)
                {
                    EquippingManager.instance.rightWeaponParentObj.transform.GetChild(i).gameObject.SetActive(true);
                    FightingActions.instance.equippedWeaponR = EquippingManager.instance.rightWeaponParentObj.transform.GetChild(i).gameObject;

                    FightingActions.instance.GetWeapon();

                    EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase = storedItemBase;
                    EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedAmount = storedAmount;

                    EquippingManager.instance.rightWeaponES.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                    EquippingManager.instance.rightWeaponES.transform.GetChild(0).gameObject.GetComponent<Image>().sprite = storedItemBase.itemSprite;

                    var eventSystem = EventSystem.current;
                    eventSystem.SetSelectedGameObject(null);
                    eventSystem.SetSelectedGameObject(EquippingManager.instance.rightWeaponES.gameObject, new BaseEventData(eventSystem));

                    break;
                }
            }
        }
    }

    public void EquipItemToHotbar()
    {
        var oldIB = storedItemBase;
        var newIB = InventoryManager.instance.currClickedBtn.storedItemBase;

        if (oldIB != null)
        {
            InventoryManager.instance.inventory.AddItem(oldIB, storedAmount);

            InventoryManager.instance.RemoveHoldingWeight(oldIB.weight, storedAmount);
        }

        if (newIB != null)
        {
            storedItemBase = newIB;
            storedAmount = Convert.ToInt32(HotbarManager.instance.hbHMScreen.howManyHBSlider.value);

            this.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = newIB.itemSprite;
            storedAmountTxt.text = storedAmount.ToString();

            InventoryManager.instance.currClickedBtn.storedAmount = InventoryManager.instance.currClickedBtn.storedAmount - Convert.ToInt32(HotbarManager.instance.hbHMScreen.howManyHBSlider.value);
            InventoryManager.instance.currClickedBtn.storedAmountTxt.text = InventoryManager.instance.currClickedBtn.storedAmount.ToString();

            InventoryManager.instance.AddHoldingWeight(newIB.weight, storedAmount);
        }
        else
        {
            this.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = null;
        }

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

        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(InventoryManager.instance.currClickedBtn.gameObject, new BaseEventData(eventSystem));

        InventoryManager.instance.currClickedBtn = null;
    }

    public void ClearHotbarSlot()
    {
        InventoryManager.instance.inventory.AddItem(storedItemBase, storedAmount);

        InventoryManager.instance.RemoveHoldingWeight(storedItemBase.weight, storedAmount);

        this.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = false;

        storedAmountTxt.text = "";
    }

    public void ClearEquipmentSlot()
    {
        InventoryManager.instance.inventory.AddItem(storedItemBase, 1);

        this.gameObject.transform.GetChild(0).GetComponent<Image>().enabled = false;

        if (storedItemBase != null && EquippingManager.instance.rightWeaponParentObj != null)
        {
            for (int i = 0; i < EquippingManager.instance.rightWeaponParentObj.transform.childCount; i++)
            {
                if (EquippingManager.instance.rightWeaponParentObj.transform.GetChild(i).GetComponent<Item>().iBP == storedItemBase)
                {
                    EquippingManager.instance.rightWeaponParentObj.transform.GetChild(i).gameObject.SetActive(false);
                    FightingActions.instance.equippedWeaponR = null;

                    //FightingActions.instance.GetWeapon();

                    EquippingManager.instance.rightWeaponES.transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
                    EquippingManager.instance.rightWeaponES.GetComponent<ClickableInventorySlot>().storedItemBase = null;

                    break;
                }
            }
        }

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

        InventoryManager.instance.hotbarObj.transform.parent = InventoryManager.instance.newHotbarParentTrans;

        InventoryManager.instance.selectHotbarSlotScreen.SetActive(true);

        cISCopy.GetComponent<Animator>().enabled = false;
        cISCopy.GetComponent<ClickableInventorySlot>().boarder.color = Color.white;
        cISCopy.GetComponent<ClickableInventorySlot>().boarder.gameObject.SetActive(true);

        HotbarManager.instance.howManyToHotbarScreen.GetComponent<HotbarHowManyScreen>().SetStartValues(storedAmount);

        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(HotbarManager.instance.howManyToHotbarScreen.GetComponent<HotbarHowManyScreen>().howManyHBSlider.gameObject, new BaseEventData(eventSystem));

        Debug.Log("EQUIPPED");
    }

    public void DisplayAllItemInformationsOnClick()
    {
        if (isNewSymbol != null && storedItemBase != null)
        {
            storedItemBase.isNew = false;
            isNewSymbol.gameObject.SetActive(false);

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
        SelectInventorySlot();
    }
}
