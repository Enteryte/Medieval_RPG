using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIAnimationHandler : MonoBehaviour
{
    public static UIAnimationHandler instance;

    [Header("Displayed Mission")]
    public Animator missionDisplayAnimator;
    public string gONameToAnim = "Empty:MissionTaskHUDDisplay";
    public string newGONameToAnim = "Empty:MissionTaskHUDDisplay_NEW";
    public string goNameToAnim2 = "Empty:MissionTaskHUDDisplay2";

    public AnimationClip completeMissionTaskAnim;
    public AnimationClip updateMissionTaskAnim;
    public AnimationClip addMissionTaskAnim;

    [Header("New Mission Message")]
    public Animator addedMissionAnimator;
    public string addedMissionString = "Neue Quest";

    public TMP_Text howChangedMissionTxt;
    public TMP_Text addedMissionTxt;

    public AnimationClip addedNewMissionMessageAnim;

    [Header("Updated Mission Message")]
    public string updatedMissionString = "Quest aktualisiert";

    [Header("Completed Mission Message")]
    public string completedMissionString = "Quest abgeschlossen";

    public void Awake()
    {
        instance = this;
    }

    public void AnimateUpdatedMissionTask(GameObject missionTaskHolderObj)
    {
        missionTaskHolderObj.name = gONameToAnim;

        missionDisplayAnimator.Rebind();

        missionDisplayAnimator.Play(updateMissionTaskAnim.name);
    }

    public void AnimateCompletedMissionTask(GameObject missionTaskHolderObj)
    {
        missionTaskHolderObj.name = gONameToAnim;

        missionDisplayAnimator.Rebind();

        missionDisplayAnimator.Play(completeMissionTaskAnim.name);
    }

    public void AnimateAddedMissionTask(/*GameObject missionTaskHolderObj*/)
    {
        missionDisplayAnimator.Rebind();

        missionDisplayAnimator.Play(addMissionTaskAnim.name);
    }

    public void AnimateAddedNewMissionMessage()
    {
        addedMissionAnimator.Rebind();

        addedMissionAnimator.Play(addedNewMissionMessageAnim.name);
    }
}
