using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DifficultyBackButton : MonoBehaviour, IPointerClickHandler
{
    public AnimationClip closeChooseDifficultyScreenAnim;

    public void OnPointerClick(PointerEventData eventData)
    {
        StartScreenManager.instance.mainAnimator.enabled = true;
        StartScreenManager.instance.mainAnimator.Rebind();
        StartScreenManager.instance.mainAnimator.Play(closeChooseDifficultyScreenAnim.name);
    }
}
