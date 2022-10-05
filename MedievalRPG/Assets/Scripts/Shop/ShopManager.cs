using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public static MerchantBaseProfile currMBP;
    public ShopListBaseProfile currSLBP;

    public Transform foodCateParentTrans;
    public Transform swordCateParentTrans;
    public Transform bowCateParentTrans;

    public GameObject shopItemButtonPrefab;

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayShopItems();
        }
    }

    public void DisplayShopItems()
    {
        ClearShopCategoryChilds(foodCateParentTrans);
        ClearShopCategoryChilds(swordCateParentTrans);
        ClearShopCategoryChilds(bowCateParentTrans);

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

            newShopItemButton.GetComponent<ShopItemButton>().storedItemBase = currSLBP.itemBaseProfiles[i];
            newShopItemButton.GetComponent<ShopItemButton>().DisplayStoredItemInformation();
        }
    }

    public void DisplayAllInformationsAboutOneItem(ItemBaseProfile iBP)
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

        itemCurrAmountInInvTxt.text = iBP.amountInInventory.ToString();

        rightShopItemInformationGO.SetActive(true);
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
