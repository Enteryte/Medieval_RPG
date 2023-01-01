using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using StarterAssets;

public class CutsceneDecisionButton : MonoBehaviour
{
    public CutsceneDecision storedDecision;
    public TMP_Text decisionTxt;

    public void SetAndDisplayDecision(CutsceneDecision decisionToStore)
    {
        storedDecision = decisionToStore;

        decisionTxt.text = storedDecision.decisionText;
    }

    public void DoDecision()
    {
        GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, true);

        if (storedDecision.cutsceneToPlay != null)
        {
            CutsceneManager.instance.playableDirector.playableAsset = storedDecision.cutsceneToPlay.cutscene;
            CutsceneManager.instance.playableDirector.Play();
            CutsceneManager.instance.currCP = storedDecision.cutsceneToPlay;
        }

        if (storedDecision.missionToActivate != null)
        {
            MissionManager.instance.AddMission(storedDecision.missionToActivate);
        }

        if (storedDecision.itemToGet != null)
        {
            InventoryManager.instance.inventory.AddItem(storedDecision.itemToGet, storedDecision.amountToGet);
        }

        if (!storedDecision.cutsceneToPlay.playCutsceneMoreThanOnce)
        {
            storedDecision.cutsceneToPlay.alreadyPlayedCutscene = true;
        }

        if (storedDecision.arguePointsToGain > 0)
        {
            storedDecision.cutsceneToPlay.mBTToCheck.currGainedPoints += storedDecision.arguePointsToGain;
        }

        for (int i = 0; i < CutsceneManager.instance.decisionBtnParentTrans.childCount; i++)
        {
            Destroy(CutsceneManager.instance.decisionBtnParentTrans.GetChild(i).gameObject);
        }
    }
}
