using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "New Mission Task Base", menuName = "Scriptable Objects/Missions/Mission Task Base Profile", order = 1)]
public class MissionTaskBase : ScriptableObject
{
    public enum MissionTaskType
    {
        none,
        talk_To,
        collect,
        kill,
        read,
        go_To
    }

    public MissionTaskType missionTaskType = MissionTaskType.none;

    public bool completeAfterInteracted = true;

    public bool missionTaskCompleted = false;
    public CutsceneProfile cutsceneToTrigger;

    [Header("When Completed Mission Task")]
    public float moneyReward;
    public ItemReward[] itemRewards;

    [Header("Can be displayed")]
    public bool canNormallyBeDisplayed = true;
    public bool canBeDisplayed = true;

    public MissionTaskBase missionTaskToActivate;

    #region TalkTo Task Values
    [HideInInspector] public NPCBaseProfile nPCToTalkToBaseProfile;
    [HideInInspector] public CutsceneProfile dialogToPlayAfterInteracted;

    [HideInInspector] public bool talkToAllNPCs = false;
    [HideInInspector] public CutsceneProfile[] possibleDialoguesToAdd;
    #endregion

    #region Collect Task Values
    [HideInInspector] public ItemBaseProfile itemToCollectBase;
    [HideInInspector] public int howManyToCollect;
    [HideInInspector] public int howManyAlreadyCollected;
    [HideInInspector] public CutsceneProfile dialogAfterSeenFirstItem;
    [HideInInspector] public CutsceneProfile dialogAfterCollectedFirstItem;
    #endregion

    #region Kill Task Values
    [HideInInspector] public EnemyBaseProfile enemyToKillBase;
    [HideInInspector] public int howManyToKill;
    [HideInInspector] public int howManyAlreadyKilled;
    [HideInInspector] public CutsceneProfile dialogAfterSeenFirstEnemy;
    [HideInInspector] public CutsceneProfile dialogAfterKilledFirstEnemy;
    #endregion

    #region Read Task Values
    [HideInInspector] public ItemBaseProfile noteOrBookToReadIBP;
    #endregion

    #region GoTo Task Values
    [HideInInspector] public GameObject missionTriggerBoxToGoTo;
    #endregion

    [CustomEditor(typeof(MissionTaskBase))]
    public class MissionTaskEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            MissionTaskBase mTB = (MissionTaskBase)target;

            EditorGUILayout.Space();

            if (mTB.missionTaskType == MissionTaskType.talk_To)
            {
                EditorGUILayout.LabelField("Talk-To-Task-Values", EditorStyles.boldLabel);

                var serializedObject = new SerializedObject(target);
                var property = serializedObject.FindProperty("nPCToTalkToBaseProfile");
                serializedObject.Update();
                EditorGUILayout.PropertyField(property, true);
                serializedObject.ApplyModifiedProperties();

                var property2 = serializedObject.FindProperty("dialogToPlayAfterInteracted");
                serializedObject.Update();
                EditorGUILayout.PropertyField(property2, true);
                serializedObject.ApplyModifiedProperties();
            }
            else if (mTB.missionTaskType == MissionTaskType.collect)
            {
                EditorGUILayout.LabelField("Collect-Task-Values", EditorStyles.boldLabel);

                var serializedObject = new SerializedObject(target);
                var property = serializedObject.FindProperty("itemToCollectBase");
                serializedObject.Update();
                EditorGUILayout.PropertyField(property, true);
                serializedObject.ApplyModifiedProperties();

                mTB.howManyToCollect = EditorGUILayout.IntField("How Many To Collect", mTB.howManyToCollect);
                mTB.howManyAlreadyCollected = EditorGUILayout.IntField("How Many Already Collected", mTB.howManyAlreadyCollected);

                var property2 = serializedObject.FindProperty("dialogAfterSeenFirstItem");
                serializedObject.Update();
                EditorGUILayout.PropertyField(property2, true);
                serializedObject.ApplyModifiedProperties();

                var property3 = serializedObject.FindProperty("dialogAfterCollectedFirstItem");
                serializedObject.Update();
                EditorGUILayout.PropertyField(property3, true);
                serializedObject.ApplyModifiedProperties();
            }
            else if (mTB.missionTaskType == MissionTaskType.kill)
            {
                EditorGUILayout.LabelField("Kill-Task-Values", EditorStyles.boldLabel);

                var serializedObject = new SerializedObject(target);
                var property = serializedObject.FindProperty("enemyToKillBase");
                serializedObject.Update();
                EditorGUILayout.PropertyField(property, true);
                serializedObject.ApplyModifiedProperties();

                mTB.howManyToKill = EditorGUILayout.IntField("How Many To Kill", mTB.howManyToKill);
                mTB.howManyAlreadyKilled = EditorGUILayout.IntField("How Many Already Killed", mTB.howManyAlreadyKilled);

                var property2 = serializedObject.FindProperty("dialogAfterSeenFirstEnemy");
                serializedObject.Update();
                EditorGUILayout.PropertyField(property2, true);
                serializedObject.ApplyModifiedProperties();

                var property3 = serializedObject.FindProperty("dialogAfterKilledFirstEnemy");
                serializedObject.Update();
                EditorGUILayout.PropertyField(property3, true);
                serializedObject.ApplyModifiedProperties();
            }
            else if (mTB.missionTaskType == MissionTaskType.read)
            {
                EditorGUILayout.LabelField("Read-Task-Values", EditorStyles.boldLabel);

                var serializedObject = new SerializedObject(target);
                var property = serializedObject.FindProperty("noteOrBookToReadIBP");
                serializedObject.Update();
                EditorGUILayout.PropertyField(property, true);
                serializedObject.ApplyModifiedProperties();
            }
            else if (mTB.missionTaskType == MissionTaskType.go_To)
            {
                EditorGUILayout.LabelField("Go-To-Task-Values", EditorStyles.boldLabel);

                var serializedObject = new SerializedObject(target);
                var property = serializedObject.FindProperty("missionTriggerBoxToGoTo");
                serializedObject.Update();
                EditorGUILayout.PropertyField(property, true);
                serializedObject.ApplyModifiedProperties();
            }

            EditorUtility.SetDirty(target);
        }
    }
}
