using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Mission-Display")]
    public static MissionBaseProfile missionToDisplay;
    public GameObject missionTaskObjParentObj;
    public GameObject missionTaskObjPrefab;

    public static GameObject newInstMissionTaskObj;

    public void Awake()
    {
        instance = this;
    }

    public void CreateMissionDisplay()
    {
        missionTaskObjParentObj.gameObject.transform.parent.gameObject.SetActive(true);

        missionTaskObjParentObj.GetComponent<GridLayoutGroup>().enabled = true;

        for (int i = 0; i < missionToDisplay.allMissionTasks.Length; i++)
        {
            if (missionToDisplay.allMissionTasks[i].mTB.canBeDisplayed && !missionToDisplay.allMissionTasks[i].mTB.missionTaskCompleted)
            {
                var newMissionTaskObj = Instantiate(missionTaskObjPrefab, missionTaskObjParentObj.transform);

                newMissionTaskObj.GetComponent<MissionTaskDisplayText>().storedMissionTaskBase = missionToDisplay.allMissionTasks[i].mTB;
                newMissionTaskObj.GetComponent<MissionTaskDisplayText>().DisplayTaskDescription(missionToDisplay.allMissionTasks[i].taskDescription);

                //newMissionTaskObj.SetActive(true);
            }
        }

        //missionTaskObjParentObj.GetComponent<GridLayoutGroup>().enabled = false;
    }

    public void UpdateMissionDisplayTasks(MissionTaskBase finishedMissionTask, MissionTaskBase newMissionBase, MissionTask newMissionTask, string newMissionTaskDescription, bool hasNewTaskToActivate)
    {
        if (hasNewTaskToActivate)
        {
            var newMissionTaskObj = Instantiate(missionTaskObjPrefab, missionTaskObjParentObj.transform);

            newMissionTaskObj.GetComponent<MissionTaskDisplayText>().storedMissionTaskBase = newMissionBase;
            newMissionTaskObj.GetComponent<MissionTaskDisplayText>().DisplayTaskDescription(newMissionTask.taskDescription);

            newMissionTaskObj.name = UIAnimationHandler.instance.newGONameToAnim;
        }

        for (int i = 0; i < missionTaskObjParentObj.transform.childCount; i++)
        {
            if (missionTaskObjParentObj.transform.GetChild(i).gameObject.GetComponent<MissionTaskDisplayText>().storedMissionTaskBase == finishedMissionTask)
            {
                MissionTaskDisplayParent.missionTaskObjNumber = i;

                if (hasNewTaskToActivate)
                {
                    UIAnimationHandler.instance.AnimateUpdatedMissionTask(missionTaskObjParentObj.transform.GetChild(i).gameObject);
                }
                else
                {
                    UIAnimationHandler.instance.AnimateCompletedMissionTask(missionTaskObjParentObj.transform.GetChild(i).gameObject);
                }
            }
        }
    }

    public void AddAndUpdateMissionDisplayTasks(MissionTaskBase newMissionBase, string taskDescrption)
    {
        var newMissionTaskObj = Instantiate(missionTaskObjPrefab, missionTaskObjParentObj.transform);

        newMissionTaskObj.GetComponent<MissionTaskDisplayText>().storedMissionTaskBase = newMissionBase;
        newMissionTaskObj.GetComponent<MissionTaskDisplayText>().DisplayTaskDescription(taskDescrption);

        Debug.Log(newMissionTaskObj);

        newMissionTaskObj.name = UIAnimationHandler.instance.goNameToAnim2;

        newMissionTaskObj.SetActive(false);

        newInstMissionTaskObj = newMissionTaskObj;

        UIAnimationHandler.instance.AnimateAddedMissionTask();
    }
}
