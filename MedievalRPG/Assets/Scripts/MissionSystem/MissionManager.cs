using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;

    public List<MissionBaseProfile> allMissions;

    public List<MissionBaseProfile> allCurrAcceptedMissions;
    public List<MissionBaseProfile> allCurrOpenNotAcceptedMissions;

    public List<GameObject> allItemsToChangeAfterMissions;

    public void Awake()
    {
        instance = this;
    }

    public void AddMission(MissionBaseProfile missionToAdd)
    {
        allCurrAcceptedMissions.Add(missionToAdd);

        if (allCurrOpenNotAcceptedMissions.Contains(missionToAdd))
        {
            allCurrOpenNotAcceptedMissions.Remove(missionToAdd);
        }
    }

    public void RemoveMission(MissionBaseProfile missionToRemove)
    {
        allCurrAcceptedMissions.Remove(missionToRemove);
    }

    public void CheckMissionTaskProgress(MissionBaseProfile mBP, MissionTaskBase missionTaskToCheck)
    {
        if (missionTaskToCheck.missionTaskType == MissionTaskBase.MissionTaskType.kill)
        {
            if (missionTaskToCheck.howManyAlreadyKilled >= missionTaskToCheck.howManyToKill)
            {
                CompleteMissionTask(mBP, missionTaskToCheck);
            }
        }
        else if (missionTaskToCheck.missionTaskType == MissionTaskBase.MissionTaskType.collect)
        {
            if (missionTaskToCheck.howManyAlreadyCollected >= missionTaskToCheck.howManyToCollect)
            {
                CompleteMissionTask(mBP, missionTaskToCheck);
            }
        }
    }

    public void CompleteMissionTask(MissionBaseProfile mBP, MissionTaskBase missionTaskToComplete)
    {
        missionTaskToComplete.missionTaskCompleted = true;

        if (missionTaskToComplete.dialogToTrigger != null)
        {
            CutsceneManager.instance.currCP = missionTaskToComplete.dialogToTrigger;
            CutsceneManager.instance.playableDirector.playableAsset = missionTaskToComplete.dialogToTrigger.cutscene;
            CutsceneManager.instance.playableDirector.Play();
        }

        CheckMissionProgress(mBP);
    }

    public void CheckMissionProgress(MissionBaseProfile missionToCheck)
    {
        for (int i = 0; i < missionToCheck.allMissionTasks.Length; i++)
        {
            if (!missionToCheck.allMissionTasks[i].mTB.missionTaskCompleted)
            {
                return;
            }
        }

        CompleteMission(missionToCheck);
    }

    public void CompleteMission(MissionBaseProfile missionToComplete)
    {
        allCurrAcceptedMissions.Remove(missionToComplete);

        missionToComplete.missionCompleted = true;

        if (missionToComplete.nextMissionToTrigger != null)
        {
            allCurrAcceptedMissions.Add(missionToComplete.nextMissionToTrigger);
        }

        if (missionToComplete.cutsceneToTrigger != null)
        {
            CutsceneManager.instance.currCP = missionToComplete.cutsceneToTrigger;
            CutsceneManager.instance.playableDirector.playableAsset = missionToComplete.cutsceneToTrigger.cutscene;
            CutsceneManager.instance.playableDirector.Play();
        }

        PlayerValueManager.instance.money += missionToComplete.moneyReward;

        for (int i = 0; i < missionToComplete.itemRewards.Length; i++)
        {
            InventoryManager.instance.inventory.AddItem(missionToComplete.itemRewards[i].iBP, missionToComplete.itemRewards[i].howManyToGet);
        }

        if (missionToComplete.changeEnvironment)
        {
            ChangeEnvironmentAfterMission(missionToComplete);
        }
    }

    public void ChangeEnvironmentAfterMission(MissionBaseProfile mission)
    {
        for (int i = 0; i < allItemsToChangeAfterMissions.Count; i++)
        {
            for (int y = 0; y < mission.itemToChangeAfterMissionNames.Length; y++)
            {
                if (allItemsToChangeAfterMissions[i].name == mission.itemToChangeAfterMissionNames[y])
                {
                    allItemsToChangeAfterMissions[i].SetActive(!allItemsToChangeAfterMissions[i].activeSelf);

                    allItemsToChangeAfterMissions.Remove(allItemsToChangeAfterMissions[i]);
                }
            }
        }
    }
}
