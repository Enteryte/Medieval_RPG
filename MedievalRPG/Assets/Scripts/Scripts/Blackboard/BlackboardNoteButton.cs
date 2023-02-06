using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackboardNoteButton : MonoBehaviour
{
    [TextArea(0, 50)] public string noteTxtString;

    public void DisplayNoteText()
    {
        Blackboard.instance.currNoteTxt.text = noteTxtString;
        Blackboard.instance.noteTxtParentGO.SetActive(true);
    }
}
