using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "New Mission Base Profile", menuName = "Scriptable Objects/Missions/Mission Base Profile", order = 0)]
public class MissionBaseProfile : ScriptableObject
{
    public enum MissionType
    {
        none,
        main,
        side
    }

    public MissionType missionType = MissionType.none;

    public string missionName;
    public string missionDescription;

    public MissionTask[] allMissionTasks;

    public bool missionCompleted = false;

    [Header("When Completed Mission")]
    public float moneyReward;
    public ItemReward[] itemRewards;

    public MissionBaseProfile nextMissionToTrigger;
    public CutsceneProfile cutsceneToTrigger;

    [Header("Side mission")] // If it's a side mission.
    public NPCBaseProfile nPCWhereToGetMissionFrom;
    public CutsceneProfile cutsceneToTriggerAfterPressedButton;
    public MissionBaseProfile missionAfterItsActive; // Mission after which THIS mission gets activted.
    public bool isActive = false;

    [Header("Change Environment")]
    public bool changeEnvironment = false;

    public string[] itemToChangeAfterMissionNames;
}

[System.Serializable]
public class MissionTask
{
    public bool isOptional = false;

    public string taskName;
    public string taskDescription;

    public MissionTaskBase mTB;
}

[System.Serializable]
public class ItemReward
{
    public ItemBaseProfile iBP;
    public int howManyToGet;
}
