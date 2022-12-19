using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MissionTaskSlot : MonoBehaviour
{
    public TMP_Text mTaskDescriptionTxt;
    public GameObject checkMark;

    public void DisplayTaskInformation(MissionTask mT)
    {
        mTaskDescriptionTxt.text = mT.taskDescription;

        checkMark.SetActive(mT.mTB.missionTaskCompleted);
    }
}
