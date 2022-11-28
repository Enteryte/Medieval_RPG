using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionTaskDisplayText : MonoBehaviour
{
    public MissionTaskBase storedMissionTaskBase;
    public TMP_Text taskDescriptionTxt;

    public void DisplayTaskDescription(string textToDisplay, bool isOptional)
    {
        if (isOptional)
        {
            taskDescriptionTxt.text = "(optional) " + textToDisplay;
        }
        else
        {
            taskDescriptionTxt.text = textToDisplay;
        }
    }
}
