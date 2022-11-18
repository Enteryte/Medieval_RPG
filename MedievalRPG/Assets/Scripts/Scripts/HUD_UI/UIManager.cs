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
            if (missionToDisplay.allMissionTasks[i].mTB.canBeDisplayed)
            {
                var newMissionTaskObj = Instantiate(missionTaskObjPrefab, missionTaskObjParentObj.transform);

                newMissionTaskObj.GetComponent<MissionTaskDisplayText>().storedMissionTaskBase = missionToDisplay.allMissionTasks[i].mTB;
                newMissionTaskObj.GetComponent<MissionTaskDisplayText>().DisplayTaskDescription(missionToDisplay.allMissionTasks[i].taskDescription);

                //newMissionTaskObj.SetActive(true);
            }
        }

        //missionTaskObjParentObj.GetComponent<GridLayoutGroup>().enabled = false;
    }

    public void UpdateMissionDisplayTasks(MissionTaskBase finishedMissionTask, MissionTaskBase newMissionBase, string newMissionTaskDescription)
    {
        Debug.Log("HERE");

        for (int i = 0; i < missionTaskObjParentObj.transform.childCount; i++)
        {
            if (missionTaskObjParentObj.transform.GetChild(i).gameObject.GetComponent<MissionTaskDisplayText>().storedMissionTaskBase == finishedMissionTask)
            {
                Debug.Log(finishedMissionTask);

                //missionTaskObjParentObj.transform.GetChild(i).gameObject.name = UIAnimationHandler.instance.gONameToAnim;

                MissionTaskDisplayParent.missionTaskObjNumber = i;

                //UpdateAndAddMissionDisplayTasks(newMissionBase, newMissionTaskDescription, false);

                //var siblingNumb = missionTaskObjParentObj.transform.GetChild(i).GetSiblingIndex();

                //if (newInstMissionTaskObj != null)
                //{
                //    newInstMissionTaskObj.transform.position = missionTaskObjParentObj.transform.GetChild(i).gameObject.transform.position;
                //    //    newInstMissionTaskObj.transform.SetSiblingIndex(siblingNumb);

                //    //    newInstMissionTaskObj = null;
                //}

                UIAnimationHandler.instance.AnimateUpdatedMissionTask(missionTaskObjParentObj.transform.GetChild(i).gameObject);
            }
        }
    }

    public void UpdateAndAddMissionDisplayTasks(MissionTaskBase newMissionBase, string taskDescrption, bool addedTask)
    {
        var newMissionTaskObj = Instantiate(missionTaskObjPrefab, missionTaskObjParentObj.transform);

        newMissionTaskObj.GetComponent<MissionTaskDisplayText>().storedMissionTaskBase = newMissionBase;
        newMissionTaskObj.GetComponent<MissionTaskDisplayText>().DisplayTaskDescription(taskDescrption);

        if (!newMissionBase.isActiveAtStart)
        {
            newMissionTaskObj.SetActive(false);
        }

        newMissionTaskObj.name = UIAnimationHandler.instance.newGONameToAnim;

        if (addedTask)
        {
            newMissionTaskObj.name = UIAnimationHandler.instance.goNameToAnim2;
        }

        newInstMissionTaskObj = newMissionTaskObj;

        if (addedTask)
        {
            UIAnimationHandler.instance.AnimateAddedMissionTask();
        }

        //newInstMissionTaskObj = null;
    }
}
