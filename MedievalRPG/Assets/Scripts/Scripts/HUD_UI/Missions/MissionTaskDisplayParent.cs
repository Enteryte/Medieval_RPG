using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionTaskDisplayParent : MonoBehaviour
{
    public static int missionTaskObjNumber = 0;

    public void ChangeObjectNames()
    {
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
}
