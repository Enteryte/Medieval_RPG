using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTriggerBox : MonoBehaviour
{
    public MissionBaseProfile correspondingMission;
    public MissionTaskBase correspondingMissionTask;

    public void Start()
    {
        if (correspondingMission == null || correspondingMissionTask == null)
        {
            Debug.Log("MissionTriggerBox: IS NULL!");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.instance.playerGO)
        {
            if (MissionManager.instance.allCurrAcceptedMissions.Contains(correspondingMission))
            {
                MissionManager.instance.CompleteMissionTask(correspondingMission, correspondingMissionTask);
            }
        }
    }
}
