using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "New Cutscene Profile", menuName = "Scriptable Objects/Cutscenes/Cutscene Profile", order = 0)]
public class CutsceneProfile : ScriptableObject
{
    public TimelineAsset cutscene;

    public bool isNotADialogue = false;

    public float timeTillWhereToSkip;

    public List<float> timesWhenNewSentenceStarts;
}
