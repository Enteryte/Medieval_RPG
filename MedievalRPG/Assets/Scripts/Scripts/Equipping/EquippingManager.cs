using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippingManager : MonoBehaviour
{
    public static EquippingManager instance;

    public EquipmentSlot leftWeaponES;
    public EquipmentSlot bowES;

    public void Awake()
    {
        instance = this;
    }
}
