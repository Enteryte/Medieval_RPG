using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyBaseProfile eBP;

    public void EnemyDie()
    {
        CheckIfNeededForMission();

        Destroy(this.gameObject);
    }

    public void CheckIfNeededForMission()
    {
        for (int i = 0; i < MissionManager.instance.allCurrAcceptedMissions.Count; i++)
        {
            if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks.Length > 1)
            {
                for (int y = 0; y < MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks.Length; y++)
                {
                    if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.missionTaskType == MissionTaskBase.MissionTaskType.kill)
                    {
                        if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.enemyToKillBase == eBP)
                        {
                            MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.howManyAlreadyKilled += 1;

                            MissionManager.instance.CheckMissionTaskProgress(MissionManager.instance.allCurrAcceptedMissions[i], MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB);
                        }
                    }
                }
            }
            else
            {
                if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.missionTaskType == MissionTaskBase.MissionTaskType.kill)
                {
                    if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.enemyToKillBase == eBP)
                    {
                        MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.howManyAlreadyKilled += 1;

                        MissionManager.instance.CheckMissionTaskProgress(MissionManager.instance.allCurrAcceptedMissions[i], MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB);
                    }
                }
            }
        }
    }
}
