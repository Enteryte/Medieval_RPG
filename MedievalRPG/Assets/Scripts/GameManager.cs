using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject playerGO;

    public int playerMoney = 0;

    public void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !ShopManager.instance.shopScreen.activeSelf)
        {
            OpenInventory();
        }
    }

    public void OpenInventory()
    {
        InventoryManager.instance.inventoryScreen.SetActive(!InventoryManager.instance.inventoryScreen.activeSelf);
        //InventoryManager.instance.DisplayItemsOfCategory();

        for (int i = 0; i < InventoryManager.instance.allInvCategoryButton.Count; i++)
        {
            if (InventoryManager.currIBP != null)
            {
                if (!InventoryManager.currIBP.neededForMissions)
                {
                    if (InventoryManager.instance.allInvCategoryButton[i].itemTypeToDisplay == InventoryManager.currIBP.itemType)
                    {
                        InventoryManager.instance.allInvCategoryButton[i].ChangeCurrentInvItemCategory();
                    }
                }
                else
                {
                    if (InventoryManager.instance.allInvCategoryButton[i].itemTypeToDisplay == ItemBaseProfile.ItemType.none)
                    {
                        InventoryManager.instance.allInvCategoryButton[i].ChangeCurrentInvItemCategory();
                    }
                }
            }
            else
            {
                if (InventoryManager.instance.allInvCategoryButton[i].itemTypeToDisplay == ItemBaseProfile.ItemType.food)
                {
                    InventoryManager.instance.allInvCategoryButton[i].ChangeCurrentInvItemCategory();
                }
            }
        }

        FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, !InventoryManager.instance.inventoryScreen.activeSelf);

        ThirdPersonController.instance._animator.SetFloat("Speed", 0);
    }

    public void FreezeCameraAndSetMouseVisibility(ThirdPersonController tPC, StarterAssetsInputs _input, bool isVisible)
    {
        _input.cursorInputForLook = isVisible;
        _input.cursorLocked = isVisible;

        tPC.LockCameraPosition = !isVisible;

        Cursor.visible = !isVisible;

        _input.SetCursorState(_input.cursorLocked);
    }
}
