using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Merchant Base Profile", menuName = "Scriptable Objects/Merchant/Merchant Base Profile")]
public class MerchantBaseProfile : ScriptableObject
{
    [Header("General Informations")]
    [Tooltip("The name of the merchant.")] public string merchantName;

    public enum MerchantType
    {
        none,
        normal_Merchant,
        blacksmith,
        healer
    }

    [Tooltip("The type of the merchant.")] public MerchantType merchantType;
    [Tooltip("Determines whether a merchant is used for or in a mission.")] public bool neededForMissions = false;

    [Header("Visuals")]
    [Tooltip("The 2D-sprite of the merchant.")] public Sprite merchantSprite;
    [Tooltip("The 3D-prefab of the merchant.")] public GameObject merchantPrefab;

    [Header("Shop Values")]
    [Tooltip("Determines whether a merchant changes the items in his shop.")] public bool changesItems = false;
    [HideInInspector] [Tooltip("The type of the merchant.")] public ShopListBaseProfile shopListBaseProfile;
    [HideInInspector] [Tooltip("The type of the merchant.")] public ShopListBaseProfile[] shopListBaseProfiles;

    #region NeededForMission Values
    [HideInInspector] [Tooltip("An array of missions, where the merchant is needed.")] [Min(0)] public GameObject[] missionsWhereMerchantsNeeded; // MUSS DURCH MISSIONBASEPROFILES
                                                                                                                                                  // ERSETZT WERDEN!
    #endregion

#if UNITY_EDITOR
    [CustomEditor(typeof(MerchantBaseProfile))]
    public class MerchantBaseProfileEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            MerchantBaseProfile mBP = (MerchantBaseProfile)target;

            if (mBP.changesItems)
            {
                var serializedObject = new SerializedObject(target);
                var property = serializedObject.FindProperty("shopListBaseProfiles");
                serializedObject.Update();
                EditorGUILayout.PropertyField(property, true);
                serializedObject.ApplyModifiedProperties();
            }
            else
            {
                var serializedObject = new SerializedObject(target);
                var property = serializedObject.FindProperty("shopListBaseProfile");
                serializedObject.Update();
                EditorGUILayout.PropertyField(property, true);
                serializedObject.ApplyModifiedProperties();
            }

            if (mBP.neededForMissions)
            {
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Mission Values", EditorStyles.boldLabel);

                var serializedObject = new SerializedObject(target);
                var property = serializedObject.FindProperty("missionsWhereMerchantsNeeded");
                serializedObject.Update();
                EditorGUILayout.PropertyField(property, true);
                serializedObject.ApplyModifiedProperties();
                // WIP: --> MissionBaseProfil hinzufügen ( sobald vorhanden )

                EditorGUILayout.Space();
            }

            if (mBP.merchantType == MerchantType.none)
            {
                EditorGUILayout.Space();

                EditorGUILayout.HelpBox("MerchantType: You need to set the type of the merchant.", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.Space();

                if (GUILayout.Button("Autocomplete Merchant's Name"))
                {
                    if (mBP.merchantType == MerchantType.blacksmith)
                    {
                        mBP.merchantName = "Blacksmith";
                    }
                    else if (mBP.merchantType == MerchantType.healer)
                    {
                        mBP.merchantName = "Healer";
                    }
                    else if (mBP.merchantType == MerchantType.normal_Merchant)
                    {
                        mBP.merchantName = "Merchant";
                    }
                }
            }

            EditorUtility.SetDirty(target);
        }
    }
#endif
}
