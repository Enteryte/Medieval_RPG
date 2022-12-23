using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StarterAssets;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public GameObject shopScreen;
    public GameObject mainShopScreen;
    public HowManyScreen hMScreen;
    public BoughtOrSoldMessageScreen bOSMScreen;

    public Merchant currMerchant;
    public static MerchantBaseProfile currMBP;
    public ShopListBaseProfile currSLBP;

    public Transform foodCateParentTrans;
    public Transform swordCateParentTrans;
    public Transform bowCateParentTrans;

    public GameObject shopItemButtonPrefab;
    public Transform shopItemPreviewCamTrans;

    public TMP_Text weightTxt;
    public TMP_Text moneyTxt;

    public GameObject itemInfoPopUp;

    public static ItemBaseProfile currClickedItem;

    public bool isShopPlayerItem = false;

    [Header("Player Item-Display")]
    public ShopCategoryButton currPlayerClickedSCBtn;
    public Transform playerItemSlotParentTrans;
    public TMP_Text currPlayerCategoryNameTxt;

    [Header("Shop Merchant Item-Display")]
    public ShopCategoryButton currMerchantClickedSCBtn;
    public Transform merchantItemSlotParentTrans;
    public TMP_Text currMerchantCategoryNameTxt;

    public List<string> allCurrCategoryNames;

    [Header("Right Shop Informations")]
    public GameObject rightShopItemInformationGO;

    public TMP_Text itemNameTxt;
    public TMP_Text itemDescriptionTxt;
    public TMP_Text itemMinLvlToUseTxt;
    public TMP_Text itemBuySellPriceTxt;
    public TMP_Text itemCurrAmountInInvTxt;

    public bool isBuying = true;

    [Header("Main Screen")]
    public Transform mainScreenButtonParentTrans;

    [Header("Button Prefabs")]
    public GameObject buyButtonPrefab;
    public GameObject sellButtonPrefab;
    public GameObject missionButtonPrefab;
    public GameObject closeMainShopScreenButtonPrefab;

    public void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && shopScreen != null && shopScreen.activeSelf)
        {
            if (hMScreen.gameObject.activeSelf)
            {
                hMScreen.gameObject.SetActive(false);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return) && shopScreen != null && shopScreen.activeSelf)
        {
            if (hMScreen.gameObject.activeSelf)
            {
                hMScreen.gameObject.SetActive(false);

                BuyOrSellItem(HowManyScreen.currIBP, (int)hMScreen.howManySSlider.value);
            }
        }

        if (itemInfoPopUp.activeSelf)
        {
            itemInfoPopUp.transform.position = Input.mousePosition;
        }
    }

    public void DisplayMainScreenButtons()
    {
        for (int i = 0; i < mainScreenButtonParentTrans.childCount; i++)
        {
            Destroy(mainScreenButtonParentTrans.GetChild(i).gameObject);
        }

        //Debug.Log(currMerchant);
        //Debug.Log(currMerchant.currCorrTask);

        Instantiate(buyButtonPrefab, mainScreenButtonParentTrans);
        //Instantiate(sellButtonPrefab, mainScreenButtonParentTrans);

        if (currMerchant.neededForMission && currMerchant.neededForMission /*!currMerchant.currCorrTask.missionTaskCompleted*/)
        {
            for (int i = 0; i < currMerchant.allCurrCorrTasks.Count; i++)
            {
                if (!currMerchant.allCurrCorrTasks[i].missionTaskCompleted)
                {
                    var shopMissionButton = Instantiate(missionButtonPrefab, mainScreenButtonParentTrans);

                    shopMissionButton.GetComponent<ShopMissionButton>().storedMissionTask = currMerchant.allCurrCorrTasks[i];
                    shopMissionButton.GetComponent<ShopMissionButton>().missionDescriptionTxt.text = currMerchant.allCurrCorrTasks[i].missionButtonDescription;
                }
            }

            for (int i = 0; i < currMerchant.allCorrMissions.Count; i++)
            {
                if (!currMerchant.allCorrMissions[i].isActive)
                {
                    var shopMissionButton = Instantiate(missionButtonPrefab, mainScreenButtonParentTrans);

                    shopMissionButton.GetComponent<ShopMissionButton>().storedMission = currMerchant.allCorrMissions[i];
                    shopMissionButton.GetComponent<ShopMissionButton>().missionDescriptionTxt.text = currMerchant.allCorrMissions[i].missionDescription;
                }
            }
        }

        Instantiate(closeMainShopScreenButtonPrefab, mainScreenButtonParentTrans);

        shopScreen.SetActive(true);

        //Debug.Log("SCREEN");

        GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, false);

        ThirdPersonController.instance.canMove = false;
        ThirdPersonController.instance._animator.SetFloat("Speed", 0);
    }

    public void OpenBuyScreen()
    {
        isBuying = true;

        DisplayShopItems();

        mainShopScreen.SetActive(true);
    }

    //public void OpenSellScreen()
    //{
    //    isBuying = false;

    //    DisplayShopItems();

    //    mainShopScreen.SetActive(true);
    //}

    public void DisplayPlayerItems()
    {
        for (int i = 0; i < playerItemSlotParentTrans.childCount; i++)
        {
            Destroy(playerItemSlotParentTrans.GetChild(i).gameObject);
        }

        for (int i = 0; i < InventoryManager.instance.inventory.slots.Count; i++)
        {
            var currItem = InventoryManager.instance.inventory.slots[i].itemBase;

            if (currItem.itemType.ToString() == currPlayerClickedSCBtn.itemTypeToDisplay.ToString())
            {
                var newItemSlot = Instantiate(shopItemButtonPrefab, playerItemSlotParentTrans);

                newItemSlot.GetComponent<ClickableInventorySlot>().storedItemBase = currItem;
                //newItemSlot.GetComponent<ClickableInventorySlot>().();

                newItemSlot.GetComponent<ClickableInventorySlot>().clickableSlotType = ClickableInventorySlot.ClickableSlotType.shopSlot;
                newItemSlot.GetComponent<ClickableInventorySlot>().isShopPlayerItem = true;

                newItemSlot.GetComponent<ClickableInventorySlot>().storedAmount = InventoryManager.instance.inventory.slots[i].itemAmount;

                newItemSlot.GetComponent<ClickableInventorySlot>().UpdateSlotInformations();

                //if (allCurrCategoryNames.Contains(currPlayerClickedSCBtn.itemTypeToDisplay.ToString()))
                //{
                //    newItemSlot.GetComponent<ShopItemButton>().im
                //}
            }
        }
    }

    public void DisplayMerchantItems()
    {
        for (int i = 0; i < merchantItemSlotParentTrans.childCount; i++)
        {
            Destroy(merchantItemSlotParentTrans.GetChild(i).gameObject);
        }

        allCurrCategoryNames.Clear();

        for (int i = 0; i < currSLBP.itemBaseProfiles.Length; i++)
        {
            var currItem = currSLBP.itemBaseProfiles[i];

            if (currItem.itemType.ToString() == currMerchantClickedSCBtn.itemTypeToDisplay.ToString())
            {
                var newItemSlot = Instantiate(shopItemButtonPrefab, merchantItemSlotParentTrans);

                newItemSlot.GetComponent<ClickableInventorySlot>().storedItemBase = currItem;
                //newItemSlot.GetComponent<ShopItemButton>().DisplayStoredItemInformation();

                if (!allCurrCategoryNames.Contains(currMerchantClickedSCBtn.itemTypeToDisplay.ToString()))
                {
                    allCurrCategoryNames.Add(currMerchantClickedSCBtn.itemTypeToDisplay.ToString());
                }

                newItemSlot.GetComponent<ClickableInventorySlot>().clickableSlotType = ClickableInventorySlot.ClickableSlotType.shopSlot;

                newItemSlot.GetComponent<ClickableInventorySlot>().storedAmountTxt.gameObject.SetActive(false);
            }
        }
    }

    public void DisplayShopItems()
    {
        ClearShopCategoryChilds(foodCateParentTrans);
        ClearShopCategoryChilds(swordCateParentTrans);
        ClearShopCategoryChilds(bowCateParentTrans);

        if (isBuying)
        {
            for (int i = 0; i < currSLBP.itemBaseProfiles.Length; i++)
            {
                GameObject newShopItemButton = null;

                if (currSLBP.itemBaseProfiles[i].itemType == ItemBaseProfile.ItemType.food)
                {
                    foodCateParentTrans.gameObject.SetActive(true);

                    newShopItemButton = Instantiate(shopItemButtonPrefab, foodCateParentTrans);
                }
                else if (currSLBP.itemBaseProfiles[i].itemType == ItemBaseProfile.ItemType.weapon)
                {
                    if (currSLBP.itemBaseProfiles[i].weaponType == ItemBaseProfile.WeaponType.sword)
                    {
                        swordCateParentTrans.gameObject.SetActive(true);

                        newShopItemButton = Instantiate(shopItemButtonPrefab, swordCateParentTrans);
                    }
                    else if (currSLBP.itemBaseProfiles[i].weaponType == ItemBaseProfile.WeaponType.bow)
                    {
                        bowCateParentTrans.gameObject.SetActive(true);

                        newShopItemButton = Instantiate(shopItemButtonPrefab, bowCateParentTrans);
                    }
                }

                if (newShopItemButton != null)
                {
                    newShopItemButton.GetComponent<ShopItemButton>().storedItemBase = currSLBP.itemBaseProfiles[i];
                    newShopItemButton.GetComponent<ShopItemButton>().DisplayStoredItemInformation();
                }
            }
        }
        else
        {
            for (int i = 0; i < InventoryManager.instance.inventory.slots.Count; i++)
            {
                GameObject newShopItemButton = null;

                if (InventoryManager.instance.inventory.slots[i].itemBase.itemType == ItemBaseProfile.ItemType.food)
                {
                    foodCateParentTrans.gameObject.SetActive(true);

                    newShopItemButton = Instantiate(shopItemButtonPrefab, foodCateParentTrans);
                }
                else if (InventoryManager.instance.inventory.slots[i].itemBase.itemType == ItemBaseProfile.ItemType.weapon)
                {
                    if (InventoryManager.instance.inventory.slots[i].itemBase.weaponType == ItemBaseProfile.WeaponType.sword)
                    {
                        swordCateParentTrans.gameObject.SetActive(true);

                        newShopItemButton = Instantiate(shopItemButtonPrefab, swordCateParentTrans);
                    }
                    else if (InventoryManager.instance.inventory.slots[i].itemBase.weaponType == ItemBaseProfile.WeaponType.bow)
                    {
                        bowCateParentTrans.gameObject.SetActive(true);

                        newShopItemButton = Instantiate(shopItemButtonPrefab, bowCateParentTrans);
                    }
                }

                if (newShopItemButton != null)
                {
                    newShopItemButton.GetComponent<ShopItemButton>().storedItemBase = InventoryManager.instance.inventory.slots[i].itemBase;
                    newShopItemButton.GetComponent<ShopItemButton>().DisplayStoredItemInformation();
                }
            }
        }
    }

    public void DisplayAllInformationsAboutOneItem(ItemBaseProfile iBP/*, InventorySlot invSlot*/)
    {
        itemNameTxt.text = iBP.itemName;
        itemDescriptionTxt.text = iBP.itemDescription;
        itemMinLvlToUseTxt.text = iBP.minLvlToUse.ToString();

        if (isBuying)
        {
            itemBuySellPriceTxt.text = iBP.buyPrice.ToString();
        }
        else
        {
            itemBuySellPriceTxt.text = iBP.sellingPrice.ToString();
        }

        //itemCurrAmountInInvTxt.text = invSlot.itemAmount.ToString();

        for (int i = 0; i < shopItemPreviewCamTrans.childCount; i++)
        {
            Destroy(shopItemPreviewCamTrans.GetChild(i).gameObject);
        }

        GameObject newPreviewItem = Instantiate(iBP.itemPrefab, Vector3.zero, Quaternion.Euler(0, 0, 2f), shopItemPreviewCamTrans);
        newPreviewItem.AddComponent<PreviewItem>();

        newPreviewItem.transform.localPosition = new Vector3(0, 0, iBP.previewSpawnPositionZ);

        newPreviewItem.layer = LayerMask.NameToLayer("PreviewItem");

        if (!rightShopItemInformationGO.activeSelf)
        {
            rightShopItemInformationGO.SetActive(true);
        }
    }

    public void BuyOrSellItem(ItemBaseProfile itemBase, int amount)
    {
        if (!isShopPlayerItem)
        {
            InventoryManager.instance.inventory.AddItem(itemBase, amount);

            Debug.Log("Bought: " + itemBase.itemName);

            PlayerValueManager.instance.money -= (itemBase.buyPrice * amount);

            bOSMScreen.boughtOrSoldTxt.text = "Item erworben";
        }
        else
        {
            InventoryManager.instance.inventory.RemoveItem(itemBase, amount);

            Debug.Log("Sold: " + itemBase.itemName);

            PlayerValueManager.instance.money += (itemBase.sellingPrice * amount);

            //if (EquippingManager.instance.bowES.currEquippedItem == itemBase)
            //{
            //    EquippingManager.instance.bowES.newItemToEquip = null;
            //    EquippingManager.instance.bowES.ChangeEquippedItem();
            //}
            //else if (EquippingManager.instance.leftWeaponES.currEquippedItem == itemBase)
            //{
            //    EquippingManager.instance.bowES.newItemToEquip = null;
            //    EquippingManager.instance.bowES.ChangeEquippedItem();
            //}

            bOSMScreen.boughtOrSoldTxt.text = "Item verkauft";
        }

        //rightShopItemInformationGO.SetActive(false);

        DisplayPlayerItems();

        moneyTxt.text = PlayerValueManager.instance.money.ToString();

        //bOSMScreen.gameObject.SetActive(true);
        //bOSMScreen.enabled = true;
    }

    public void ClearShopCategoryChilds(Transform categoryTrans)
    {
        for (int i = 1; i < categoryTrans.childCount; i++)
        {
            Destroy(categoryTrans.GetChild(i).gameObject);
        }

        categoryTrans.gameObject.SetActive(false);
    }

    public void CheckIfNeededForCutscene()
    {
        if (CutsceneManager.instance.currCP.playNewCutsceneAfterDeactivatedObj)
        {
            if (CutsceneManager.instance.currCP.gameObjectToDeactivateName == shopScreen.name)
            {
                CutsceneManager.instance.currCP = CutsceneManager.instance.currCP.cutsceneToPlayAfter;
                CutsceneManager.instance.playableDirector.playableAsset = CutsceneManager.instance.currCP.cutscene;
                CutsceneManager.instance.playableDirector.Play();
            }
        }
    }
}
