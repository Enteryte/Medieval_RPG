using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "New Cutscene Profile", menuName = "Scriptable Objects/Cutscenes/Cutscene Profile", order = 0)]
public class CutsceneProfile : ScriptableObject
{
    public TimelineAsset cutscene;

    public bool isNotADialogue = false;

    public float timeTillWhereToSkip;

    public List<float> timesWhenNewSentenceStarts;

    [Header("Decisions")]
    public bool hasDecisions = false;

    /*[HideInInspector] */
    public CutsceneDecision[] allDecisions;

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

    public CutsceneProfile cutsceneToPlay;
    public MissionBaseProfile missionToActivate;

    public ItemBaseProfile itemToGet;
    public int amountToGet;
}
