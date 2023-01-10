using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippingManager : MonoBehaviour
{
    public static EquippingManager instance;

    public GameObject weaponParentObj;
    public GameObject rightWeaponParentObj;

    public EquipmentSlot leftWeaponES;
    public EquipmentSlot rightWeaponES;

    public void Awake()
    {
        instance = this;
    }
}
