using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TavernSideMissionButton : MonoBehaviour
{
    public MissionBaseProfile corrMissionBP;

    public Button missionBtn;

    // Start is called before the first frame update
    void Start()
    {
        missionBtn = this.gameObject.GetComponent<Button>();

        missionBtn.onClick.AddListener(PlaySideMissionCutscene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySideMissionCutscene()
    {
        CutsceneManager.instance.currCP = corrMissionBP.cutsceneToTriggerAfterPressedButton;
        CutsceneManager.instance.playableDirector.playableAsset = corrMissionBP.cutsceneToTriggerAfterPressedButton.cutscene;
        CutsceneManager.instance.playableDirector.Play();

        TavernKeeper.instance.getBeerScreen.SetActive(false);

        Destroy(this.gameObject);
    }

    public void CheckSideMissionState()
    {
        if (MissionManager.instance.allCurrAcceptedMissions.Contains(corrMissionBP))
        {
            var howManyCollectMissions = 0;
            var completedTaskNumbers = 0;

            for (int i = 0; i < corrMissionBP.allMissionTasks.Length; i++)
            {
                if (corrMissionBP.allMissionTasks[i].mTB.missionTaskType == MissionTaskBase.MissionTaskType.collect)
                {
                    howManyCollectMissions += 1;

                    if (corrMissionBP.allMissionTasks[i].mTB.howManyAlreadyCollected >= corrMissionBP.allMissionTasks[i].mTB.howManyToCollect)
                    {
                        completedTaskNumbers += 1;
                    }
                }
            }

            if (completedTaskNumbers > 0 && completedTaskNumbers == howManyCollectMissions)
            {
                missionBtn.interactable = true;

                this.gameObject.SetActive(true);

                Debug.Log("YEEEEEEEEEEENOOOOOOOOOOOOOOOOOOOO");
            }
            else
            {
                this.gameObject.SetActive(true);
                //this.gameObject.SetActive(false);

                missionBtn.interactable = false;

                Debug.Log("NOOOOOOOOOOOOOOOOOOOO");
            }
        }
        else
        {
            this.gameObject.SetActive(false);

            missionBtn.interactable = false;

            Debug.Log("NOOOOOOOOOOOOOOOOOOO232424rO");
        }
    }
}
