using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCMissionButton : MonoBehaviour
{
    public MissionTask storedMT;
    public MissionTaskBase storedMTB;

    public Image decisionImg;

    [Header("Check Money")]
    public bool checkMoney = false;
    public int minMoneyValue;

    [Header("Stored Possible Cutscenes ( SQ3 T. w. Kilian )")]
    public CutsceneProfile cutsceneToPlay;

    public void Awake()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(PlayMissionTaskCutscene);
    }

    public void Start()
    {
        if (checkMoney)
        {
            if (PlayerValueManager.instance.money < minMoneyValue)
            {
                this.gameObject.GetComponent<Button>().interactable = false;

                Debug.Log("NOT INTERACTABLE");
            }
        }
    }

    public void PlayMissionTaskCutscene()
    {
        //if (Interacting.instance.currInteractedObjTrans.gameObject.GetComponent<NPC>().npcAudioType == NPC.NPCAudioType.male)
        //{
        //    var randomCutscene = Random.Range(0, storedMTB.possibleDialoguesToAddMale.Length);

        //    CutsceneManager.instance.currCP = storedMTB.possibleDialoguesToAddMale[randomCutscene];
        //    CutsceneManager.instance.playableDirector.playableAsset = storedMTB.possibleDialoguesToAddMale[randomCutscene].cutscene;
        //}
        //else
        //{
        //    var randomCutscene = Random.Range(0, storedMTB.possibleDialoguesToAddFemale.Length);

        //    CutsceneManager.instance.currCP = storedMTB.possibleDialoguesToAddFemale[randomCutscene];
        //    CutsceneManager.instance.playableDirector.playableAsset = storedMTB.possibleDialoguesToAddFemale[randomCutscene].cutscene;
        //}

        if (storedMTB == MissionManager.instance.mTBWSymon)
        {
            CutsceneManager.instance.currCP = Interacting.instance.currInteractedObjTrans.gameObject.GetComponent<NPC>().cPMissionTaskSymon;
            CutsceneManager.instance.playableDirector.playableAsset = Interacting.instance.currInteractedObjTrans.gameObject.GetComponent<NPC>().cPMissionTaskSymon.cutscene;

            CutsceneManager.instance.playableDirector.Play();
        }
        else if (storedMTB == MissionManager.instance.mTBWMya)
        {
            CutsceneManager.instance.currCP = Interacting.instance.currInteractedObjTrans.gameObject.GetComponent<NPC>().cPMissionTaskMya;
            CutsceneManager.instance.playableDirector.playableAsset = Interacting.instance.currInteractedObjTrans.gameObject.GetComponent<NPC>().cPMissionTaskMya.cutscene;

            CutsceneManager.instance.playableDirector.Play();

            ThirdPersonController.instance.canMove = false;
            GameManager.instance.gameIsPaused = true;
        }
        else if (cutsceneToPlay != null)
        {
            CutsceneManager.instance.currCP = cutsceneToPlay;
            CutsceneManager.instance.playableDirector.playableAsset = cutsceneToPlay.cutscene;

            CutsceneManager.instance.playableDirector.Play();

            Debug.Log("hujkml,fe");
        }
        else
        {
            Debug.Log("CLOSE");

            GameManager.instance.cantPauseRN = false;
            CutsceneManager.instance.CloseCutscene();

            CutsceneManager.instance.currCP = null;
            CutsceneManager.instance.playableDirector.playableAsset = null;
        }

        //Interacting.instance.currInteractedObjTrans.gameObject.GetComponent<NPC>().nPCCVC.enabled = true;
        //Interacting.instance.currInteractedObjTrans.gameObject.GetComponent<NPC>().nPCCVC.gameObject.SetActive(true);

        UIManager.instance.npcMissionButtonParentObjTrans.gameObject.SetActive(false);

        if (UIManager.instance.npcBtnKillianGOs.Length > 0)
        {
            for (int i = 0; i < UIManager.instance.npcBtnKillianGOs.Length; i++)
            {
                if (UIManager.instance.npcBtnKillianGOs[i] != null)
                {
                    UIManager.instance.npcBtnKillianGOs[i].SetActive(false);
                }
            }
        }

        Interacting.instance.nearestObjTrans = null;
    }
}
