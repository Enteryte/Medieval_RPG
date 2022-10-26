using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTriggerBox : MonoBehaviour
{
    public MissionBaseProfile correspondingMission;
    public MissionTaskBase correspondingMissionTask;

    public bool triggerWithoutMission = false; // Triggers Cutscene without any mission or mission task.
    public bool triggerWhenMissionIsAccepted = false; // Triggers Cutscene if mission is accepted.
    public bool triggerWhenMissionTaskIsCompleted = false; // Triggers Cutscene if mission task is completed.

    public CutsceneProfile cutsceneToTrigger; // Better more a dialogue or monologue  than an actual cutscene.

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.instance.playerGO)
        {
            if (triggerWithoutMission)
            {
                CutsceneManager.instance.currCP = cutsceneToTrigger;
                CutsceneManager.instance.playableDirector.playableAsset = cutsceneToTrigger.cutscene;
                CutsceneManager.instance.playableDirector.Play();

                Destroy(this.gameObject);
            }
            else if (triggerWhenMissionIsAccepted)
            {
                if (MissionManager.instance.allCurrAcceptedMissions.Contains(correspondingMission))
                {
                    CutsceneManager.instance.currCP = cutsceneToTrigger;
                    CutsceneManager.instance.playableDirector.playableAsset = cutsceneToTrigger.cutscene;
                    CutsceneManager.instance.playableDirector.Play();

                    Destroy(this.gameObject);
                }
            }
            else if (triggerWhenMissionTaskIsCompleted)
            {
                if (MissionManager.instance.allCurrAcceptedMissions.Contains(correspondingMission))
                {
                    if (correspondingMissionTask.missionTaskCompleted)
                    {
                        CutsceneManager.instance.currCP = cutsceneToTrigger;
                        CutsceneManager.instance.playableDirector.playableAsset = cutsceneToTrigger.cutscene;
                        CutsceneManager.instance.playableDirector.Play();

                        Destroy(this.gameObject);
                    }
                }
            }
        }
    }
}
