using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TavernMissionButton : MonoBehaviour
{
    public MissionTaskBase storedMissionTask;

    public TMP_Text missionDescriptionTxt;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(StartMissionDialogue);
    }

    public void Update()
    {
        if (storedMissionTask == null || storedMissionTask.missionTaskCompleted)
        {
            Destroy(this.gameObject);
        }
    }

    public void StartMissionDialogue()
    {
        CutsceneManager.instance.currCP = storedMissionTask.dialogToPlayAfterPressedButton;
        CutsceneManager.instance.playableDirector.playableAsset = CutsceneManager.instance.currCP.cutscene;
        CutsceneManager.instance.playableDirector.Play();

        TavernKeeper.instance.getBeerScreen.SetActive(false);
    }
}
