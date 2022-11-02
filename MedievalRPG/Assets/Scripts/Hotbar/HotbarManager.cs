using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotbarManager : MonoBehaviour
{
    public static HotbarManager instance;

    [Header("Hotbar")]
    public HotbarSlotButton[] allHotbarSlotBtn;

    public static HotbarSlotButton currHSB;
    public static ItemBaseProfile currDraggedIBP;

    public GameObject currDraggableInventorySlotObj;

    public bool draggedHotbarItem = false;
    public static GameObject lastDraggedStoredItemHS;

    public void Awake()
    {
        instance = this;
    }
}
