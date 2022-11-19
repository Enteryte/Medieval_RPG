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
        var randomCutscene = Random.Range(0, storedMTB.possibleDialoguesToAdd.Length);

        CutsceneManager.instance.currCP = storedMTB.possibleDialoguesToAdd[randomCutscene];
        CutsceneManager.instance.playableDirector.playableAsset = storedMTB.possibleDialoguesToAdd[randomCutscene].cutscene;
        CutsceneManager.instance.playableDirector.Play();

        UIManager.instance.npcMissionButtonParentObjTrans.gameObject.SetActive(false);

        Interacting.instance.nearestObjTrans = null;
    }
}
