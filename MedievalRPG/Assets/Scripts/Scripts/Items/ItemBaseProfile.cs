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

    public float weight;

    public enum ItemType
    {
        none,
        food,
        potion,
        weapon,
        bookOrNote
    }

    [Tooltip("The type of the item.")] public ItemType itemType;
    //[Tooltip("Determines whether an item is used for or in a mission.")] public int minLvlToUse;
    [Tooltip("Determines whether an item is used for or in a mission.")] public bool neededForMissions = false;
    [Tooltip("Determines whether an item was in the players inventory before.")] public bool isNew = false;

    [Header("Visuals")]
    [Tooltip("The 2D-sprite of the item.")] public Sprite itemSprite;
    [Tooltip("The 3D-prefab of the item.")] public GameObject itemPrefab;

    [Header("Shop Values")]
    [Tooltip("The purchase price of the item.")] [Min(0)] public int buyPrice;
    [Tooltip("The selling price of the item.")] public int sellingPrice;

    //public float previewSpawnPositionZ;

    [Header("Inventory Values")]
    //[Tooltip("Indicates how often the item is currently in the inventory.")] [Min(0)] public int amountInInventory;
    [Tooltip("Determines whether an item in the inventory is stackable.")] [Min(0)] public bool stackable;

    [Header("Needed Items For Buying")]
    public ItemBaseProfile[] itemsNeededForBuying;

    //[Header("Food Item")]
    //public float healingTime;

    #region NeededForMission Values
    [HideInInspector] [Tooltip("An array of missions, where the item is needed.")] [Min(0)] public GameObject[] missionsWhereItsNeeded; // MUSS DURCH MISSIONBASEPROFILES
                                                                                                                                        // ERSETZT WERDEN!
    #endregion

    #region FoodItem Values
    [HideInInspector] public float foodHealValue;
    [HideInInspector] public float timeTillValueIsHealed;
    #endregion

    #region WeaponItem Values
    public enum WeaponType
    {
        none,
        sword,
        shield,
        bow,
        greatsword,
        axe,
        torch,
        stone,
        mace,
        arrow
    }

    [HideInInspector] [Tooltip("The type of the weapon.")] public WeaponType weaponType;

    [HideInInspector] public bool isTwoHand = false;
    [HideInInspector] [Tooltip("The normal damage value of the weapon.")] [Min(0)] public float normalDamage;
    public ItemStat[] otherItemStats;
    #endregion

    #region PotionItem Values
    public enum PotionType
    {
        none,
        healing,
        stamina,
        speed,
        strength
    }

    [HideInInspector] [Tooltip("The type of the potion.")] public PotionType potionType;
    [HideInInspector] [Tooltip("The normal damage value of the weapon.")] [Min(0)] public float potionBuffValue;
    [HideInInspector] [Tooltip("The normal damage value of the weapon.")] [Min(0)] public float potionBuffLastingTime;
    #endregion

    #region
    public bool hasBeenRead = false;
    [HideInInspector] public MissionTaskBase corresspondingMissionTask;
    [HideInInspector] public CutsceneProfile cutsceneToPlayAfterCloseReadScreen;

    public enum ReadType
    {
        none,
        book,
        scroll,
        note
    }

    public ReadType readType;

    [TextArea(0, 50)] public string noteTxtString;
    #endregion

    public bool CheckNeededItemsForBuying()
    {
        var hasItemNumbers = 0;

        for (int i = 0; i < itemsNeededForBuying.Length; i++)
        {
            for (int y = 0; y < InventoryManager.instance.inventory.slots.Count; y++)
            {
                if (InventoryManager.instance.inventory.slots[y].itemBase == itemsNeededForBuying[i])
                {
                    hasItemNumbers += 1;
                }
            }
        }

        if (hasItemNumbers == itemsNeededForBuying.Length)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ItemBaseProfile))]
    public class ItemBaseProfileEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            ItemBaseProfile iBP = (ItemBaseProfile)target;

            EditorGUILayout.Space();

            iBP.sellingPrice = iBP.buyPrice - ((iBP.buyPrice * 30) / 70);

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

                CheckIfItemValueIsZero(iBP.foodHealValue);
                CheckIfItemValueIsZero(iBP.timeTillValueIsHealed);

                iBP.stackable = true;
            }
            else if (iBP.itemType == ItemType.weapon)
            {
                EditorGUILayout.LabelField("Weapon Values", EditorStyles.boldLabel);

                iBP.weaponType = (ItemBaseProfile.WeaponType)EditorGUILayout.EnumPopup("Weapon Type", iBP.weaponType);
                iBP.isTwoHand = EditorGUILayout.Toggle("Two-Hand-Weapon", iBP.isTwoHand);
                iBP.normalDamage = EditorGUILayout.FloatField("Normal Damage", iBP.normalDamage);

                CheckIfItemValueIsZero(iBP.normalDamage);

                if (iBP.weaponType == WeaponType.arrow)
                {
                    iBP.stackable = true;
                }
                else
                {
                    iBP.stackable = false;
                }
            }
            else if (iBP.itemType == ItemType.potion)
            {
                EditorGUILayout.LabelField("Potion Values", EditorStyles.boldLabel);

                iBP.potionType = (ItemBaseProfile.PotionType)EditorGUILayout.EnumPopup("Potion Type", iBP.potionType);
                iBP.potionBuffValue = EditorGUILayout.FloatField("Buff Value", iBP.potionBuffValue);

                CheckIfItemValueIsZero(iBP.potionBuffValue);

                iBP.stackable = true;

                if (iBP.potionType == PotionType.none)
                {
                    EditorGUILayout.Space();

                    EditorGUILayout.HelpBox("PotionType: You need to set the type of the potion.", MessageType.Warning);
                }
            }
            else if (iBP.itemType == ItemType.bookOrNote)
            {
                var serializedObject = new SerializedObject(target);
                var property = serializedObject.FindProperty("corresspondingMissionTask");
                serializedObject.Update();
                EditorGUILayout.PropertyField(property, true);
                serializedObject.ApplyModifiedProperties();

                var property2 = serializedObject.FindProperty("cutsceneToPlayAfterCloseReadScreen");
                serializedObject.Update();
                EditorGUILayout.PropertyField(property2, true);
                serializedObject.ApplyModifiedProperties();
            }

            CheckIfItemValueIsZero(iBP.buyPrice);
            CheckIfItemValueIsZero(iBP.sellingPrice);

            if (iBP.sellingPrice > iBP.buyPrice)
            {
                iBP.sellingPrice = iBP.buyPrice;
            }

            EditorUtility.SetDirty(target);
        }

        public void CheckIfItemValueIsZero(float valueToCheck)
        {
            if (valueToCheck < 0)
            {
                valueToCheck = 0;
            }
        }
    }
#endif
}

public class ItemStat
{
    public enum ItemStatType
    {
        none,
        plusProcentDmg,
        minusProcentDmg,
        plusProcentDmgSkeleton,
        minusProcentDmgSkeleton,
        plusProcentDmgUndead,
        minusProcentDmgUndead
    }

    public float statValue;

    public ItemStatType itemStatType;
}
