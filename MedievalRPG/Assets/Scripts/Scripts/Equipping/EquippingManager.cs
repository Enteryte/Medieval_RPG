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

    public EquipmentSlot glovesES;
    public EquipmentSlot pauldronsES;
    public EquipmentSlot poleynsES;

    public ItemBaseProfile glovesIB;
    public ItemBaseProfile pauldronsIB;
    public ItemBaseProfile poleynsIB;

    public GameObject glovesGO;
    public GameObject glovesGO2;
    public GameObject pauldronsGO;
    public GameObject pauldronsGO2;
    public GameObject poleynsGO;
    public GameObject poleynsGO2;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
