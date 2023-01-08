using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCMissionButton : MonoBehaviour
{
    public MissionTask storedMT;
    public MissionTaskBase storedMTB;

    public void Awake()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(PlayMissionTaskCutscene);
    }

    public void PlayMissionTaskCutscene()
    {
        if (Interacting.instance.currInteractedObjTrans.gameObject.GetComponent<NPC>().npcAudioType == NPC.NPCAudioType.male)
        {
            var randomCutscene = Random.Range(0, storedMTB.possibleDialoguesToAddMale.Length);

            CutsceneManager.instance.currCP = storedMTB.possibleDialoguesToAddMale[randomCutscene];
            CutsceneManager.instance.playableDirector.playableAsset = storedMTB.possibleDialoguesToAddMale[randomCutscene].cutscene;
        }
        else
        {
            var randomCutscene = Random.Range(0, storedMTB.possibleDialoguesToAddFemale.Length);

            CutsceneManager.instance.currCP = storedMTB.possibleDialoguesToAddFemale[randomCutscene];
            CutsceneManager.instance.playableDirector.playableAsset = storedMTB.possibleDialoguesToAddFemale[randomCutscene].cutscene;
        }

        //Interacting.instance.currInteractedObjTrans.gameObject.GetComponent<NPC>().nPCCVC.enabled = true;
        //Interacting.instance.currInteractedObjTrans.gameObject.GetComponent<NPC>().nPCCVC.gameObject.SetActive(true);

        CutsceneManager.instance.playableDirector.Play();

        UIManager.instance.npcMissionButtonParentObjTrans.gameObject.SetActive(false);

        Interacting.instance.nearestObjTrans = null;        
    }
}
