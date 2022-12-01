using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StarterAssets;

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

    public GameObject draggableInvSlotPrefab;
    public Transform draggableInvSlotParent;

    [Header("Weight")]
    public float maxHoldingWeight;
    public float currHoldingWeight; // Equipment + Hotbar Weight

    [Header("Current Clicked Button")]
    public GameObject currClickedBtn;
    public Animator currClickedBtnAnimator;

    public TMP_Text whatToDoTxt;

    public static ClickableInventorySlot currCIS;

    [Header("Equipp Item")]
    public GameObject selectHotbarSlotScreen;
    public Transform oldHotbarParentTrans;
    public Transform newHotbarParentTrans;
    public GameObject hotbarObj;

    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < inventory.database.items.Length; i++)
        {
            inventory.database.items[i].hasBeenRead = false;
        }
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

    public void AddHoldingWeight(float weightToAdd, int howOften)
    {
        currHoldingWeight += weightToAdd * howOften;

        CheckHoldingWeight();
    }

    public void RemoveHoldingWeight(float weightToRemove, int howOften)
    {
        currHoldingWeight -= weightToRemove * howOften;

        CheckHoldingWeight();
    }

    public void CheckHoldingWeight()
    {
        if (currHoldingWeight > maxHoldingWeight && ThirdPersonController.instance.MoveSpeed == ThirdPersonController.instance.normalMoveSpeed)
        {
            //ThirdPersonController.instance._animator.speed = 0.65f;

            ThirdPersonController.instance.MoveSpeed = 0.7f;
        }
        else if(currHoldingWeight <= maxHoldingWeight && ThirdPersonController.instance.MoveSpeed != ThirdPersonController.instance.normalMoveSpeed)
        {
            if (!DebuffManager.instance.slowPlayerDebuff)
            {
                ThirdPersonController.instance._animator.speed = 1;
            }

            ThirdPersonController.instance.MoveSpeed = ThirdPersonController.instance.normalMoveSpeed;
        }
    }
}
