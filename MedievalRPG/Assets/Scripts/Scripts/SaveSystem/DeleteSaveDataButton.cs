using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteSaveDataButton : MonoBehaviour
{
    public LoadSlot correspondingLoadSlot;
    public AnimationClip openAreYouSureDeleteScreenAnim;

    public void OpenAreYouSureToDeleteScreen()
    {
        StartScreenManager.currClickedLoadSlot = correspondingLoadSlot;

        if (!StartScreenManager.instance.dontAskOnDeleteDataAgain)
        {
            StartScreenManager.instance.mainAnimator.Play(openAreYouSureDeleteScreenAnim.name);
        }
        else
        {
            // WIP: Delete SaveData of currClickedLoadSlot

            StartScreenManager.currClickedLoadSlot = null;
            Destroy(correspondingLoadSlot.gameObject);
        }
    }
}
