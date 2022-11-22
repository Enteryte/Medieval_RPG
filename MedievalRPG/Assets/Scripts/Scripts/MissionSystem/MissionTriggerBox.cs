using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTriggerBox : MonoBehaviour
{
    public MissionBaseProfile correspondingMission;
    public MissionTaskBase correspondingMissionTask;
    public CutsceneProfile cutsceneToTrigger;

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

                if (cutsceneToTrigger != null)
                {
                    CutsceneManager.instance.currCP = cutsceneToTrigger;
                    CutsceneManager.instance.playableDirector.playableAsset = cutsceneToTrigger.cutscene;
                    CutsceneManager.instance.playableDirector.Play();
                }
            }
        }
    }
}
