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

    public GameObject itemInfoPopUpLeft;
    public GameObject itemInfoPopUpRight;

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
        if (instance == null)
        {
            instance = this;
        }
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

        if (itemInfoPopUpLeft.activeSelf)
        {
            itemInfoPopUpLeft.transform.position = Input.mousePosition;
        }
        else if (itemInfoPopUpRight.activeSelf)
        {
            itemInfoPopUpRight.transform.position = Input.mousePosition;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !hMScreen.gameObject.activeSelf)
        {
            if (mainShopScreen.activeSelf)
            {
                CutsceneManager.instance.playableDirector.playableAsset = currMerchant.idleTimeline;
                CutsceneManager.instance.playableDirector.Play();

                mainShopScreen.SetActive(false);
            }
            else if (shopScreen.activeSelf)
            {              
                CloseMSScreenButton.CloseScreen();
            }
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

                    if (shopMissionButton.GetComponent<ShopMissionButton>().storedMission.missionType == MissionBaseProfile.MissionType.main)
                    {
                        shopMissionButton.GetComponent<ShopMissionButton>().missionTypeImg.sprite 
                            = shopMissionButton.GetComponent<ShopMissionButton>().mainQuestSprite;
                    }
                    else
                    {
                        shopMissionButton.GetComponent<ShopMissionButton>().missionTypeImg.sprite 
                            = shopMissionButton.GetComponent<ShopMissionButton>().sideQuestSprite;
                    }
                }
            }

            for (int i = 0; i < currMerchant.allCorrMissions.Count; i++)
            {
                if (!currMerchant.allCorrMissions[i].isActive)
                {
                    var shopMissionButton = Instantiate(missionButtonPrefab, mainScreenButtonParentTrans);

                    shopMissionButton.GetComponent<ShopMissionButton>().storedMission = currMerchant.allCorrMissions[i];
                    shopMissionButton.GetComponent<ShopMissionButton>().missionDescriptionTxt.text = currMerchant.allCorrMissions[i].missionDescription;

                    if (shopMissionButton.GetComponent<ShopMissionButton>().storedMission.missionType == MissionBaseProfile.MissionType.main)
                    {
                        shopMissionButton.GetComponent<ShopMissionButton>().missionTypeImg.sprite
                            = shopMissionButton.GetComponent<ShopMissionButton>().mainQuestSprite;
                    }
                    else
                    {
                        shopMissionButton.GetComponent<ShopMissionButton>().missionTypeImg.sprite
                            = shopMissionButton.GetComponent<ShopMissionButton>().sideQuestSprite;
                    }
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

        DisplayPlayerItems();
        DisplayMerchantItems();

        //DisplayShopItems();

        mainShopScreen.SetActive(true);

        moneyTxt.text = PlayerValueManager.instance.money.ToString();
        weightTxt.text = InventoryManager.instance.currHoldingWeight.ToString() + " / " + InventoryManager.instance.maxHoldingWeight.ToString();
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

            if (currPlayerClickedSCBtn != null)
            {
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
            else
            {
                if (currItem.itemType == ItemBaseProfile.ItemType.food)
                {
                    var newItemSlot = Instantiate(shopItemButtonPrefab, playerItemSlotParentTrans);

                    newItemSlot.GetComponent<ClickableInventorySlot>().storedItemBase = currItem;
                    //newItemSlot.GetComponent<ClickableInventorySlot>().();

                    newItemSlot.GetComponent<ClickableInventorySlot>().clickableSlotType = ClickableInventorySlot.ClickableSlotType.shopSlot;
                    newItemSlot.GetComponent<ClickableInventorySlot>().isShopPlayerItem = true;

                    newItemSlot.GetComponent<ClickableInventorySlot>().storedAmount = InventoryManager.instance.inventory.slots[i].itemAmount;

                    newItemSlot.GetComponent<ClickableInventorySlot>().UpdateSlotInformations();

                    currPlayerCategoryNameTxt.text = "Nahrung";

                    //if (allCurrCategoryNames.Contains(currPlayerClickedSCBtn.itemTypeToDisplay.ToString()))
                    //{
                    //    newItemSlot.GetComponent<ShopItemButton>().im
                    //}
                }
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

            if (currMerchantClickedSCBtn != null)
            {
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
            else
            {
                if (currItem.itemType == ItemBaseProfile.ItemType.food)
                {
                    var newItemSlot = Instantiate(shopItemButtonPrefab, merchantItemSlotParentTrans);

                    newItemSlot.GetComponent<ClickableInventorySlot>().storedItemBase = currItem;
                    //newItemSlot.GetComponent<ShopItemButton>().DisplayStoredItemInformation();

                    if (!allCurrCategoryNames.Contains(ItemBaseProfile.ItemType.food.ToString()))
                    {
                        allCurrCategoryNames.Add(ItemBaseProfile.ItemType.food.ToString());
                    }

                    newItemSlot.GetComponent<ClickableInventorySlot>().clickableSlotType = ClickableInventorySlot.ClickableSlotType.shopSlot;

                    newItemSlot.GetComponent<ClickableInventorySlot>().storedAmountTxt.gameObject.SetActive(false);

                    currMerchantCategoryNameTxt.text = "Nahrung";
                }
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

                //if (newShopItemButton != null)
                //{
                //    newShopItemButton.GetComponent<ClickableInventorySlot>().storedItemBase = currSLBP.itemBaseProfiles[i];
                //    newShopItemButton.GetComponent<ClickableInventorySlot>().DisplayAllItemInformationsOnClick();
                //}
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
        //itemMinLvlToUseTxt.text = iBP.minLvlToUse.ToString();

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

        //GameObject newPreviewItem = Instantiate(iBP.itemPrefab, Vector3.zero, Quaternion.Euler(0, 0, 2f), shopItemPreviewCamTrans);
        //newPreviewItem.AddComponent<PreviewItem>();

        //newPreviewItem.transform.localPosition = new Vector3(0, 0, iBP.previewSpawnPositionZ);

        //newPreviewItem.layer = LayerMask.NameToLayer("PreviewItem");

        if (!rightShopItemInformationGO.activeSelf)
        {
            rightShopItemInformationGO.SetActive(true);
        }
    }

    public void BuyOrSellItem(ItemBaseProfile itemBase, int amount)
    {
        if (isShopPlayerItem)
        {
            for (int x = 0; x < MissionManager.instance.allCurrAcceptedMissions.Count; x++)
            {
                for (int i = 0; i < MissionManager.instance.allCurrAcceptedMissions[x].allMissionTasks.Length; i++)
                {
                    if (MissionManager.instance.allCurrAcceptedMissions[x].allMissionTasks[i].mTB.missionTaskType == MissionTaskBase.MissionTaskType.collect)
                    {
                        for (int y = 0; y < InventoryManager.instance.inventory.slots.Count; y++)
                        {
                            if (InventoryManager.instance.inventory.slots[y].itemBase == MissionManager.instance.allCurrAcceptedMissions[x].allMissionTasks[i].mTB.itemToCollectBase 
                                && InventoryManager.instance.inventory.slots[y].itemBase == itemBase)
                            {
                                MissionManager.instance.allCurrAcceptedMissions[x].allMissionTasks[i].mTB.howManyAlreadyCollected = InventoryManager.instance.inventory.slots[y].itemAmount - amount;

                                break;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            for (int x = 0; x < MissionManager.instance.allCurrAcceptedMissions.Count; x++)
            {
                for (int i = 0; i < MissionManager.instance.allCurrAcceptedMissions[x].allMissionTasks.Length; i++)
                {
                    if (MissionManager.instance.allCurrAcceptedMissions[x].allMissionTasks[i].mTB.missionTaskType == MissionTaskBase.MissionTaskType.collect)
                    {
                        //for (int y = 0; y < InventoryManager.instance.inventory.slots.Count; y++)
                        //{
                            if (MissionManager.instance.allCurrAcceptedMissions[x].allMissionTasks[i].mTB.itemToCollectBase == itemBase)
                            {
                                MissionManager.instance.allCurrAcceptedMissions[x].allMissionTasks[i].mTB.howManyAlreadyCollected += amount;

                                break;
                            }
                        //}
                    }
                }
            }
        }

        if (!isShopPlayerItem)
        {
            InventoryManager.instance.inventory.AddItem(itemBase, amount);

            for (int i = 0; i < itemBase.itemsNeededForBuying.Length; i++)
            {
                InventoryManager.instance.inventory.RemoveItem(itemBase.itemsNeededForBuying[i], 1);
            }

            Debug.Log("Bought: " + itemBase.itemName);

            PlayerValueManager.instance.money -= (itemBase.buyPrice * amount);

            bOSMScreen.boughtOrSoldTxt.text = "Item erworben";

            //var chanceOfTriggerDialogue = Random.Range(0, 100);

            //if (chanceOfTriggerDialogue > 70)
            //{

            // ---------------------------------------------> WIP: Irgendwie buggy; Funktioniert manchmal nur bei den Waffen.
            var randomMerchantDialogue = Random.Range(0, currMerchant.mAfterBoughtShopPA.Length);

            CutsceneManager.instance.playableDirector.playableAsset = currMerchant.mAfterBoughtShopPA[randomMerchantDialogue];
            CutsceneManager.instance.playableDirector.Play();

            Debug.Log(randomMerchantDialogue);
            //}
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

        //if (currMerchant.mAfterBoughtShopPA.Length > 0)
        //{

        //}
        //else
        //{
        //    var randomMerchantDialogue = Random.Range(0, currMerchant.mAfterBoughtShopPA.Length);

        //    CutsceneManager.instance.playableDirector.playableAsset = currMerchant.mAfterBoughtShopPA[randomMerchantDialogue];
        //    CutsceneManager.instance.playableDirector.Play();
        //}
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
