using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ClickableInventorySlot : MonoBehaviour, ISelectHandler
{
    public enum ClickableSlotType
    {
        inventorySlot,
        equipmentSlot,
        hotbarSlot
    }

    public ClickableSlotType clickableSlotType;

    public Image boarder;
    public Animator animator;

    public Image isNewSymbol;

    public ItemBaseProfile storedItemBase;
    public int storedAmount;

    [HideInInspector] public InventorySlot invSlot;
    //[HideInInspector] public List<Button> allInteractableButton;

    public void Awake()
    {
    }

    public void Start()
    {
        if (this.gameObject.GetComponent<Button>() != null)
        {
            this.gameObject.GetComponent<Button>().onClick.AddListener(SelectHotbarSlotToEquipTo);

            //var eventSystem = EventSystem.current;
            //eventSystem.SetSelectedGameObject(null);
            //eventSystem.SetSelectedGameObject(this.gameObject, new BaseEventData(eventSystem));
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

        DisplayAllItemInformationsOnClick();
    }

    // Press Enter
    public void SelectHotbarSlotToEquipTo()
    {
        for (int i = 1; i < InventoryManager.instance.newHotbarParentTrans.childCount; i++)
        {
            Destroy(InventoryManager.instance.newHotbarParentTrans.transform.GetChild(i).gameObject);
        }

        var cISCopy = Instantiate(this.gameObject, this.gameObject.transform.parent);
        cISCopy.GetComponent<LayoutElement>().ignoreLayout = true;
        cISCopy.transform.position = this.gameObject.transform.position;

        cISCopy.transform.parent = InventoryManager.instance.newHotbarParentTrans;

        InventoryManager.instance.hotbarObj.transform.parent = InventoryManager.instance.newHotbarParentTrans;

        InventoryManager.instance.selectHotbarSlotScreen.SetActive(true);

        cISCopy.GetComponent<Animator>().enabled = false;
        cISCopy.GetComponent<ClickableInventorySlot>().boarder.gameObject.SetActive(true);

        var eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(InventoryManager.instance.hotbarObj.transform.GetChild(0).gameObject, new BaseEventData(eventSystem));
        //InventoryManager.instance.hotbarObj.transform.GetChild(0).GetComponent<ClickableInventorySlot>().SelectInventorySlot();

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
            InventoryManager.instance.currItemSellPriceTxt.text = storedItemBase.sellingPrice.ToString();

            InventoryManager.currIBP = storedItemBase;
            //InventoryManager.currIS = invSlot;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        SelectInventorySlot();
    }
}
