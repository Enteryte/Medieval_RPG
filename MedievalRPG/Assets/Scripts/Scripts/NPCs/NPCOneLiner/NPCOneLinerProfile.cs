using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "New NPC One-Liner Profile", menuName = "Scriptable Objects/NPCs/NPC One-Liner Profile")]
public class NPCOneLinerProfile : ScriptableObject
{
    public TimelineAsset timelineWSubtitles;
    public AudioClip audioCorrToTimeline;
}
