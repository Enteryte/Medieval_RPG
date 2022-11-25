using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopMissionButton : MonoBehaviour
{
    public MissionBaseProfile storedMission;
    public MissionTaskBase storedMissionTask;

    public TMP_Text missionDescriptionTxt;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<Button>().onClick.AddListener(StartMissionDialogue);
    }

    public void Update()
    {
        if (storedMissionTask != null && storedMissionTask.missionTaskCompleted)
        {
            Destroy(this.gameObject);
        }
        else if (storedMission != null && storedMission.isActive)
        {
            Destroy(this.gameObject);
        }
    }

    public void StartMissionDialogue()
    {
        if (storedMission != null)
        {
            CutsceneManager.instance.currCP = storedMission.cutsceneToTriggerAfterPressedButton;
        }
        else
        {
            CutsceneManager.instance.currCP = storedMissionTask.dialogToPlayAfterPressedButton;
        }

        CutsceneManager.instance.playableDirector.playableAsset = CutsceneManager.instance.currCP.cutscene;
        CutsceneManager.instance.playableDirector.Play();

        ShopManager.instance.shopScreen.SetActive(false);
    }
}
