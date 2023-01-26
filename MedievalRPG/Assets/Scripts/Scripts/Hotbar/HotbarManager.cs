using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarManager : MonoBehaviour
{
    public static HotbarManager instance;

    public HotbarHowManyScreen hbHMScreen;
    public GameObject howManyToHotbarScreen;

    [Header("Hotbar")]
    public ClickableInventorySlot[] allHotbarSlotBtn;

    public static HotbarSlotButton currHSB;
    public static ItemBaseProfile currDraggedIBP;
    public int currAmount;

    public GameObject currDraggableInventorySlotObj;

    public bool draggedHotbarItem = false;
    public bool startedOnHSB = false;
    public static GameObject lastDraggedStoredItemHS;

    public bool isUsingItem = false;

    [Header("If Using Item")]
    public AnimationClip drinkingAnim;

    public void Awake()
    {
        instance = this;
    }

    public void OpenHowManyHotbarScreen()
    {
        hbHMScreen.currDisplayedAmount = 1;

        hbHMScreen.UpdateAmountText();
        hbHMScreen.UpdateWeightTextColor();
        hbHMScreen.SetNewMaxAmount();

        howManyToHotbarScreen.SetActive(true);
    }
}
