using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTaskDisplayParent : MonoBehaviour
{
    public static int missionTaskObjNumber = 0;

    public MissionTaskBase taskNumber4;

    public void ChangeObjectNames()
    {
        Debug.Log("9999999999999999999999");

        for (int i = 0; i < UIManager.instance.missionTaskObjParentObj.transform.childCount; i++)
        {
            UIManager.instance.missionTaskObjParentObj.transform.GetChild(i).gameObject.name = "MissionTaskDisplay";
        }
    }

    public void DestroyObject()
    {
        ChangeObjectNames();

        Destroy(UIManager.instance.missionTaskObjParentObj.transform.GetChild(missionTaskObjNumber).gameObject);
    }

    public void ActivateTaskNumber4()
    {
        for (int i = 0; i < UIManager.instance.missionTaskObjParentObj.transform.childCount; i++)
        {
            Debug.Log("!!!!!!!!!");

            if (UIManager.instance.missionTaskObjParentObj.transform.GetChild(i).GetComponent<MissionTaskDisplayText>().storedMissionTaskBase != null 
                && UIManager.instance.missionTaskObjParentObj.transform.GetChild(i).GetComponent<MissionTaskDisplayText>().storedMissionTaskBase.canBeDisplayed)
            {
                UIManager.instance.missionTaskObjParentObj.transform.GetChild(i).gameObject.SetActive(true);

                Debug.Log("!!!!!!!!!2");
            }
        }
    }
}
