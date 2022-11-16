using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionTaskDisplayText : MonoBehaviour
{
    public MissionTaskBase storedMissionTaskBase;
    public TMP_Text taskDescriptionTxt;

    public void DisplayTaskDescription(string textToDisplay)
    {
        taskDescriptionTxt.text = textToDisplay;
    }
}
