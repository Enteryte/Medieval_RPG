using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerScreenMissionButton : MonoBehaviour
{
    public static BeerScreenMissionButton instance;

    public MissionTaskBase currStoredMissionTaskBase;

    public void Awake()
    {
        instance = this;
    }

    public void Update()
    {
        if (currStoredMissionTaskBase == null || currStoredMissionTaskBase.missionTaskCompleted)
        {
            currStoredMissionTaskBase = null;
            this.gameObject.SetActive(false);
        }
    }

    public void StartMissionDialogue()
    {
        //Debug.Log(currCorrTask);
        //Debug.Log(currCorrTask.dialogToPlayAfterPressedButton);

        CutsceneManager.instance.currCP = currStoredMissionTaskBase.dialogToPlayAfterPressedButton;
        CutsceneManager.instance.playableDirector.playableAsset = CutsceneManager.instance.currCP.cutscene;
        CutsceneManager.instance.playableDirector.Play();
    }
}
