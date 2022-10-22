using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public GameObject inventoryScreen;

    public InventoryBaseProfile inventory;

    public GameObject inventorySlotPrefab;

    public Transform inventorySlotsParentObjTrans;

    public static ItemBaseProfile.ItemType currItemType;

    public TMP_Text currCategoryNameTxt;

    public TMP_Text currItemNameTxt;
    public TMP_Text currItemDescriptionTxt;
    public TMP_Text currItemAmountInInvTxt;
    public TMP_Text currItemSellPriceTxt;

    public GameObject rightInventoryScreen;
    public GameObject useItemButton;

    public Transform invItemPreviewCamTrans;

    public static ItemBaseProfile currIBP;
    public static InventorySlot currIS;

    public List<InventoryCategoryButton> allInvCategoryButton;

    public float maxInvWeight;
    public float currInvWeight;

    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayItemsOfCategory()
    {
        currIBP = null;

        for (int i = 0; i < inventorySlotsParentObjTrans.childCount; i++)
        {
            Destroy(inventorySlotsParentObjTrans.GetChild(i).gameObject);
        }

        for (int i = 0; i < inventory.slots.Count; i++)
        {
            if (currItemType != ItemBaseProfile.ItemType.none)
            {
                if (inventory.slots[i].itemBase.itemType == currItemType && !inventory.slots[i].itemBase.neededForMissions)
                {
                    GameObject newInventorySlot = Instantiate(inventorySlotPrefab, inventorySlotsParentObjTrans);

                    newInventorySlot.GetComponent<InventorySlotButton>().storedItemBase = inventory.slots[i].itemBase;
                    newInventorySlot.GetComponent<InventorySlotButton>().DisplayItemInformations();

                    newInventorySlot.GetComponent<InventorySlotButton>().invSlot = inventory.slots[i];

                    if (currIBP == null)
                    {
                        newInventorySlot.GetComponent<InventorySlotButton>().DisplayAllItemInformationsOnClick();
                    }
                }
            }
            else
            {
                if (inventory.slots[i].itemBase.neededForMissions)
                {
                    GameObject newInventorySlot = Instantiate(inventorySlotPrefab, inventorySlotsParentObjTrans);

                    newInventorySlot.GetComponent<InventorySlotButton>().storedItemBase = inventory.slots[i].itemBase;
                    newInventorySlot.GetComponent<InventorySlotButton>().DisplayItemInformations();

                    if (currIBP == null)
                    {
                        newInventorySlot.GetComponent<InventorySlotButton>().DisplayAllItemInformationsOnClick();
                    }
                }
            }
        }

        if (currIBP == null)
        {
            rightInventoryScreen.gameObject.SetActive(false);
        }
        else
        {
            rightInventoryScreen.gameObject.SetActive(true);
        }
    }

    public void AddInvWeight(float weightToAdd)
    {
        currInvWeight += weightToAdd;
    }

    public void RemoveInvWeight(float weightToRemove)
    {
        currInvWeight -= weightToRemove;
    }
}
