using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Items/Item Base Profile", fileName = "New Item Base Profile", order = 0)]
public class ItemBaseProfile : ScriptableObject
{
    [Header("General Informations")]
    [Tooltip("The name of the item.")] public string itemName;
    [Tooltip("The description of the item.")] [TextArea] public string itemDescription;

    public enum ItemType
    {
        none,
        food,
        weapon
    }

    public ItemType itemType;

    [Header("Visuals")]
    [Tooltip("The 2D-sprite of the item.")] public Sprite itemSprite;
    [Tooltip("The 3D-prefab of the item.")] public GameObject itemPrefab;

    [Header("Shop Values")]
    [Tooltip("The purchase price of the item.")] [Min(0)] public int buyPrice;
    [Tooltip("The selling price of the item.")] [Min(0)] public int sellingPrice;

    #region FoodItem Values
    [HideInInspector] public float foodHealValue;
    #endregion

    #region WeaponItem Values
    public enum WeaponType
    {
        none,
        sword,
        bow
    }

    [HideInInspector] [Tooltip("The type of the weapon.")] public WeaponType weaponType;
    [HideInInspector] [Tooltip("The normal damage value of the weapon.")] [Min(0)] public float normalDamage;
    #endregion

    [CustomEditor(typeof(ItemBaseProfile))]
    public class ItemBaseProfileEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ItemBaseProfile iBP = (ItemBaseProfile)target;

            EditorGUILayout.Space();

            if (iBP.itemType == ItemType.food)
            {
                EditorGUILayout.LabelField("Food Values", EditorStyles.boldLabel);

                iBP.foodHealValue = EditorGUILayout.FloatField("Heal Value", iBP.foodHealValue);
            }
            else if (iBP.itemType == ItemType.weapon)
            {
                EditorGUILayout.LabelField("Weapon Values", EditorStyles.boldLabel);

                iBP.weaponType = (ItemBaseProfile.WeaponType)EditorGUILayout.EnumPopup("Weapon Type", iBP.weaponType);
                iBP.normalDamage = EditorGUILayout.FloatField("Normal Damage", iBP.normalDamage);

                if (iBP.normalDamage < 0)
                {
                    iBP.normalDamage = 0;
                }
            }

            EditorUtility.SetDirty(target);
        }
    }
}
