using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimationHandler : MonoBehaviour
{
    public static UIAnimationHandler instance;

    [Header("Displayed Mission")]
    public Animator missionDisplayAnimator;
    public string gONameToAnim = "Empty:MissionTaskHUDDisplay";
    public string newGONameToAnim = "Empty:MissionTaskHUDDisplay_NEW";
    public string goNameToAnim2 = "Empty:MissionTaskHUDDisplay2";

    public AnimationClip updateMissionTaskAnim;
    public AnimationClip addMissionTaskAnim;

    public void Awake()
    {
        instance = this;
    }

    public void AnimateUpdatedMissionTask(GameObject missionTaskHolderObj)
    {
        missionTaskHolderObj.name = gONameToAnim;

        missionDisplayAnimator.Rebind();
        //missionDisplayAnimator.enabled = true;
        missionDisplayAnimator.Play(updateMissionTaskAnim.name);
    }

    public void AnimateAddedMissionTask(/*GameObject missionTaskHolderObj*/)
    {
        //missionTaskHolderObj.name = gONameToAnim;

        missionDisplayAnimator.Rebind();
        //missionDisplayAnimator.enabled = true;
        missionDisplayAnimator.Play(addMissionTaskAnim.name);
    }
}
