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

    public static MerchantBaseProfile currMBP;
    public ShopListBaseProfile currSLBP;

    public Transform foodCateParentTrans;
    public Transform swordCateParentTrans;
    public Transform bowCateParentTrans;

    public GameObject shopItemButtonPrefab;
    public Transform shopItemPreviewCamTrans;

    [Header("Right Shop Informations")]
    public GameObject rightShopItemInformationGO;

    public TMP_Text itemNameTxt;
    public TMP_Text itemDescriptionTxt;
    public TMP_Text itemMinLvlToUseTxt;
    public TMP_Text itemBuySellPriceTxt;
    public TMP_Text itemCurrAmountInInvTxt;

    public bool isBuying = true;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (mainShopScreen.activeSelf)
            {
                mainShopScreen.SetActive(false);
            }
            else if (shopScreen.activeSelf)
            {
                shopScreen.SetActive(false);

                GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, true);
            }
        }
    }

    public void OpenBuyScreen()
    {
        isBuying = true;

        DisplayShopItems();

        mainShopScreen.SetActive(true);
    }

    public void OpenSellScreen()
    {
        isBuying = false;

        DisplayShopItems();

        mainShopScreen.SetActive(true);
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

                Debug.Log(currSLBP);
                Debug.Log(currSLBP.itemBaseProfiles[i]);
                Debug.Log(currSLBP.itemBaseProfiles[i].itemType);

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

                newShopItemButton.GetComponent<ShopItemButton>().storedItemBase = currSLBP.itemBaseProfiles[i];
                newShopItemButton.GetComponent<ShopItemButton>().DisplayStoredItemInformation();
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

                newShopItemButton.GetComponent<ShopItemButton>().storedItemBase = InventoryManager.instance.inventory.slots[i].itemBase;
                newShopItemButton.GetComponent<ShopItemButton>().DisplayStoredItemInformation();
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
        if (ShopManager.instance.isBuying)
        {
            InventoryManager.instance.inventory.AddItem(itemBase, amount);

            Debug.Log("Bought: " + itemBase.itemName);

            GameManager.instance.playerMoney -= (itemBase.buyPrice * amount);

            bOSMScreen.boughtOrSoldTxt.text = "Item erworben";
        }
        else
        {
            InventoryManager.instance.inventory.RemoveItem(itemBase, amount);

            Debug.Log("Sold: " + itemBase.itemName);

            GameManager.instance.playerMoney += (itemBase.sellingPrice * amount);

            bOSMScreen.boughtOrSoldTxt.text = "Item verkauft";
        }

        //rightShopItemInformationGO.SetActive(false);

        ShopManager.instance.DisplayShopItems();

        bOSMScreen.gameObject.SetActive(true);
        bOSMScreen.enabled = true;
    }

    public void ClearShopCategoryChilds(Transform categoryTrans)
    {
        for (int i = 1; i < categoryTrans.childCount; i++)
        {
            Destroy(categoryTrans.GetChild(i).gameObject);
        }

        categoryTrans.gameObject.SetActive(false);
    }
}
