using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipCutsceneAnimationHandler : MonoBehaviour
{
    public void SkipCutscene(/*float timeTillWhereToSkip*/)
    {
        CutsceneManager.instance.SkipCutscene();
    }
}
