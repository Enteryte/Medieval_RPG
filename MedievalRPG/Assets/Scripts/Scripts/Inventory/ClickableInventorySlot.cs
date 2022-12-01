using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickableInventorySlot : MonoBehaviour
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

    public void Awake()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(SelectInventorySlot);
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

    public void DisplayAllItemInformationsOnClick()
    {
        //if (storedItemBase.isNew)
        //{
            storedItemBase.isNew = false;
            isNewSymbol.gameObject.SetActive(false);
        //}

        InventoryManager.instance.currItemNameTxt.text = storedItemBase.itemName;
        InventoryManager.instance.currItemDescriptionTxt.text = storedItemBase.itemDescription;
        //InventoryManager.instance.currItemAmountInInvTxt.text = invSlot.itemAmount.ToString();
        InventoryManager.instance.currItemSellPriceTxt.text = storedItemBase.sellingPrice.ToString();

        InventoryManager.currIBP = storedItemBase;
        //InventoryManager.currIS = invSlot;

        //if (!storedItemBase.neededForMissions)
        //{
        //    if (storedItemBase.itemType == ItemBaseProfile.ItemType.food || storedItemBase.itemType == ItemBaseProfile.ItemType.weapon
        //        || storedItemBase.itemType == ItemBaseProfile.ItemType.bookOrNote)
        //    {
        //        InventoryManager.instance.useItemButton.SetActive(true);
        //    }
        //    else
        //    {
        //        InventoryManager.instance.useItemButton.SetActive(false);
        //    }
        //}
        //else
        //{
        //    InventoryManager.instance.useItemButton.SetActive(false);
        //}

        //for (int i = 0; i < InventoryManager.instance.invItemPreviewCamTrans.childCount; i++)
        //{
        //    Destroy(InventoryManager.instance.invItemPreviewCamTrans.GetChild(i).gameObject);
        //}

        //GameObject newPreviewItem = Instantiate(storedItemBase.itemPrefab, Vector3.zero, Quaternion.Euler(0, 0, 2f), InventoryManager.instance.invItemPreviewCamTrans);
        //newPreviewItem.AddComponent<PreviewItem>();

        //newPreviewItem.transform.localPosition = new Vector3(0, 0, storedItemBase.previewSpawnPositionZ);

        //newPreviewItem.layer = LayerMask.NameToLayer("PreviewItem");
    }
}
