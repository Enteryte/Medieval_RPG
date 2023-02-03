using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "New Cutscene Profile", menuName = "Scriptable Objects/Cutscenes/Cutscene Profile", order = 0)]
public class CutsceneProfile : ScriptableObject
{
    public TimelineAsset cutscene;

    public bool fadeIn = true;

    public bool isNightCutscene = false;

    public bool isNotADialogue = false;
    public bool cantBeSkipped = false;

    public bool playCutsceneMoreThanOnce = true;
    public bool alreadyPlayedCutscene = false;

    public float timeTillWhereToSkip;

    public List<float> timesWhenNewSentenceStarts;

    public bool canPauseWhilePlaying = false;

    [Header("Tutorial")]
    public TutorialBaseProfile tutorialToTrigger;

    [Header("Change Transform At Start")]
    public bool changeParentTrans = false;

    [Header("Decisions")]
    public bool hasDecisions = false;

    /*[HideInInspector] */
    public CutsceneDecision[] allDecisions;

    [Header("Optional Dialog-Parts")]
    public MissionTaskBase mBTToCheck;
    public CutsceneProfile cutsceneToChangeTo;

    [Header("For Completing Tasks Or Missions")]
    public MissionBaseProfile missionToComplete;
    public MissionTaskBase missionTaskToComplete;

    [Header("After Cutscene")]
    public MissionBaseProfile missionToPlayAfter;
    public bool playNewCutsceneAfterDeactivatedObj = false;
    public string gameObjectToDeactivateName;
    public CutsceneProfile cutsceneToPlayAfter;

    public MissionBaseProfile corresspondingMission;
    public MissionTaskBase missionTaskToActivate;

    //[CustomEditor(typeof(CutsceneProfile))]
    //public class CutsceneProfileEditor : Editor
    //{
    //    public override void OnInspectorGUI()
    //    {
    //        base.OnInspectorGUI();

    //        CutsceneProfile cP = (CutsceneProfile)target;

    //        EditorGUILayout.Space();

    //        if (cP.hasDecisions)
    //        {
    //            EditorGUILayout.LabelField("Decisions", EditorStyles.boldLabel);

    //            var serializedObject = new SerializedObject(target);
    //            var property = serializedObject.FindProperty("allDecisions");
    //            serializedObject.Update();
    //            EditorGUILayout.PropertyField(property, true);
    //            serializedObject.ApplyModifiedProperties();
    //        }

    //        EditorUtility.SetDirty(target);
    //    }
    //}
}

[System.Serializable]
public class CutsceneDecision
{
    public string decisionText;
    public Sprite decisionSprite;

    public int arguePointsToGain;

    public bool needsToBeClicked = true;

    public CutsceneProfile cutsceneToPlay;
    public MissionBaseProfile missionToActivate;

    public bool isMissionRelevant = true;

    [Header("Has To Give Money")]
    public bool hasToGiveMoney = false;
    public int moneyAmountToGive;

    [Header("Check Money")]
    public bool checkMoney = false;
    public int minMoneyValue;

    [Header("Rewards")]
    public ItemBaseProfile itemToGet;
    public int amountToGet;
}
