using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Base Profile", menuName = "Scriptable Objects/Items/Item Base Profile", order = 0)]
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

    [Tooltip("The type of the item.")] public ItemType itemType;
    [Tooltip("Determines whether an item is used for or in a mission.")] public int minLvlToUse;
    [Tooltip("Determines whether an item is used for or in a mission.")] public bool neededForMissions = false;

    [Header("Visuals")]
    [Tooltip("The 2D-sprite of the item.")] public Sprite itemSprite;
    [Tooltip("The 3D-prefab of the item.")] public GameObject itemPrefab;

    [Header("Shop Values")]
    [Tooltip("The purchase price of the item.")] [Min(0)] public int buyPrice;
    [Tooltip("The selling price of the item.")] [Min(0)] public int sellingPrice;

    [Header("Inventory Values")]
    [Tooltip("Indicates how often the item is currently in the inventory.")] [Min(0)] public int amountInInventory;
    [Tooltip("Determines whether an item in the inventory is stackable.")] [Min(0)] public bool stackable;

    #region NeededForMission Values
    [HideInInspector] [Tooltip("An array of missions, where the item is needed.")] [Min(0)] public GameObject[] missionsWhereItsNeeded; // MUSS DURCH MISSIONBASEPROFILES
                                                                                                                                        // ERSETZT WERDEN!
    #endregion

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

            if (iBP.neededForMissions)
            {
                EditorGUILayout.LabelField("Mission Values", EditorStyles.boldLabel);

                var serializedObject = new SerializedObject(target);
                var property = serializedObject.FindProperty("missionsWhereItsNeeded");
                serializedObject.Update();
                EditorGUILayout.PropertyField(property, true);
                serializedObject.ApplyModifiedProperties();
                // WIP: --> MissionBaseProfil hinzufügen ( sobald vorhanden )

                EditorGUILayout.Space();
            }

            if (iBP.itemType == ItemType.none)
            {
                //EditorGUILayout.Space();

                EditorGUILayout.HelpBox("ItemType: You need to set the type of the item.", MessageType.Warning);
            }
            else if (iBP.itemType == ItemType.food)
            {
                EditorGUILayout.LabelField("Food Values", EditorStyles.boldLabel);

                iBP.foodHealValue = EditorGUILayout.FloatField("Heal Value", iBP.foodHealValue);

                if (iBP.foodHealValue < 0)
                {
                    iBP.foodHealValue = 0;
                }

                iBP.stackable = true;
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

                iBP.stackable = false;
            }

            if (iBP.sellingPrice > iBP.buyPrice)
            {
                iBP.sellingPrice = iBP.buyPrice;
            }

            if (iBP.amountInInventory < 0)
            {
                iBP.amountInInventory = 0;
            }

            EditorUtility.SetDirty(target);
        }
    }
}
