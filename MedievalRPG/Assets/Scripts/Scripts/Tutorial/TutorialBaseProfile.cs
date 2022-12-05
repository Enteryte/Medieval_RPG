using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tutorial Base Profile", menuName = "Scriptable Objects/Tutorial/Tutorial Base Profile")]
public class TutorialBaseProfile : ScriptableObject
{
    public string tutorialName;

    [TextArea(0, 15)]
    public string tutorialDescription;

    public bool hasElementToPointAt = false;
    public bool useSmallTutorialUI = false;
}
