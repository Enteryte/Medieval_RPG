using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    public static CutsceneManager instance;

    public GameObject cutsceneCam;
    public PlayableDirector playableDirector;

    public CutsceneProfile currCP;

    public float timeToPressToSkipCS = 3;
    public float pressedTime = 0;

    public GameObject decisionBtnPrefab;
    public Transform decisionBtnParentTrans;

    public Image cutsceneBlackBackground;

    public Vector3 playerGOCutscenePos;

    public Transform playerBaseMeshParentTrans;

    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playableDirector.playableAsset != null)
        {
            if (Input.GetKey(KeyCode.Escape) && currCP.isNotADialogue && currCP.cantBeSkipped)
            {
                if (!GameManager.instance.playedTheGameThrough)
                {
                    pressedTime += Time.deltaTime;

                    if (pressedTime >= timeToPressToSkipCS)
                    {
                        SkipCutscene(currCP.timeTillWhereToSkip);

                        pressedTime = 0;
                    }
                }
                else
                {
                    SkipCutscene(currCP.timeTillWhereToSkip);
                }   
            }

            if (Input.GetKeyDown(KeyCode.Return) && !currCP.isNotADialogue && currCP.cantBeSkipped)
            {
                SkipSentenceInDialogue();
            }

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                pressedTime = 0;
            }
        }
    }

    public void StartPlayingCutscene()
    {
        playableDirector.Play(currCP.cutscene);
    }

    public void StartPlayCutsceneCouroutine(CutsceneProfile cutsceneProfile)
    {
        currCP = cutsceneProfile;

        StartCoroutine(StartCutsceneFadeIn(cutsceneProfile));
    }

    public void SetAndPlayCutscene()
    {
        //if (currCP.changeParentTrans)
        //{
        //    GameManager.instance.playerGO.transform.parent = Interacting.instance.currInteractedObjTrans;
        //    //cutsceneCam.transform.parent = Interacting.instance.currInteractedObjTrans;
        //}

        playableDirector.playableAsset = currCP.cutscene;
        playableDirector.Play();
    }

    public void ChangeCutsceneCamParentToPlayer()
    {
        cutsceneCam.transform.parent = GameManager.instance.playerGO.transform;
    }

    public void ChangeCutsceneCamParentToCurrInteractObj()
    {
        if (Interacting.instance.currInteractedObjTrans.gameObject.GetComponent<Merchant>() == null)
        {
            cutsceneCam.transform.parent = Interacting.instance.currInteractedObjTrans;
        }
        else
        {
            cutsceneCam.transform.parent = Interacting.instance.currInteractedObjTrans.GetComponent<Merchant>().whereToSetPlayerTrans;
        }
    }

    public void ChangePlayerParentToCurrInteractObj()
    {
        if (Interacting.instance.currInteractedObjTrans.gameObject.GetComponent<Merchant>() == null)
        {
            GameManager.instance.playerGO.transform.parent = Interacting.instance.currInteractedObjTrans;
        }
        else
        {
            GameManager.instance.playerGO.transform.parent = Interacting.instance.currInteractedObjTrans.GetComponent<Merchant>().whereToSetPlayerTrans;
        }
        
        //GameManager.instance.playerGOParent.transform.localPosition = playerGOCutscenePos;
    }

    public void ChangePlayerParentToNull()
    {
        GameManager.instance.playerGO.transform.parent = Interacting.instance.currInteractedObjTrans;
    }

    public void ChangeCutsceneCamParentToNull()
    {
        cutsceneCam.transform.parent = null;
    }

    public IEnumerator StartCutsceneFadeIn(CutsceneProfile cutsceneProfile)
    {
        currCP = cutsceneProfile;

        //if (cutsceneProfile.fadeIn)
        //{
        //    Debug.Log("FADE");

        //    cutsceneBlackBackground.gameObject.SetActive(true);

        //    var color = cutsceneBlackBackground.color;

        //    while (cutsceneBlackBackground.color.a < 1)
        //    {
        //        color.a += Time.deltaTime;

        //        cutsceneBlackBackground.color = color;

        //        yield return null;

        //        if (cutsceneBlackBackground.color.a >= 1)
        //        {
        //            Debug.Log("JJJ");
        //        }
        //    }

        //    if (cutsceneBlackBackground.color.a >= 1)
        //    {
                Debug.Log(cutsceneBlackBackground.color.a);

                playableDirector.playableAsset = currCP.cutscene;
                playableDirector.Play();
                SetAndPlayCutscene();

        yield return null;
        //    }
        //}
        //else
        //{
        //    Debug.Log("FADE NOT");

        //    yield return null;

        //    SetAndPlayCutscene();
        //}
    }

    public void SkipSentenceInDialogue()
    {
        for (int i = 0; i < currCP.timesWhenNewSentenceStarts.Count; i++)
        {
            if (playableDirector.time < currCP.timesWhenNewSentenceStarts[i])
            {
                playableDirector.time = currCP.timesWhenNewSentenceStarts[i];

                return;
            }
        }
    }

    public void SkipCutscene(float timeTillWhereToSkip)
    {
        playableDirector.time = timeTillWhereToSkip;
    }

    public void BackgroundFadeIn()
    {
        cutsceneBlackBackground.GetComponent<Animator>().Play("BackgroundFadeIn");
    }

    public void BackgroundFadeOut()
    {
        cutsceneBlackBackground.GetComponent<Animator>().Play("BackgroundFadeOut");
    }

    public void CloseCutscene()
    {
        cutsceneCam.SetActive(false);

        TavernKeeper.instance.getBeerScreen.SetActive(false);

        GameManager.instance.playerGO.transform.parent = playerBaseMeshParentTrans;

        ThirdPersonController.instance.canMove = true;
    }

    public void DisplayDecisions()
    {
        GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, false);

        for (int i = 0; i < currCP.allDecisions.Length; i++)
        {
            if (currCP.allDecisions[i].cutsceneToPlay.playCutsceneMoreThanOnce)
            {
                var newDecisionButton = Instantiate(decisionBtnPrefab, decisionBtnParentTrans);

                newDecisionButton.GetComponent<CutsceneDecisionButton>().SetAndDisplayDecision(currCP.allDecisions[i]);
            }
            else if (!currCP.allDecisions[i].cutsceneToPlay.alreadyPlayedCutscene)
            {
                var newDecisionButton = Instantiate(decisionBtnPrefab, decisionBtnParentTrans);

                newDecisionButton.GetComponent<CutsceneDecisionButton>().SetAndDisplayDecision(currCP.allDecisions[i]);
            }
        }

        if (!decisionBtnParentTrans.GetChild(0).gameObject.GetComponent<CutsceneDecisionButton>().storedDecision.needsToBeClicked)
        {
            Destroy(decisionBtnParentTrans.GetChild(0).gameObject);

            cutsceneCam.SetActive(false);

            GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, true);
        }
    }

    public void ResetNPCAfterDialogue()
    {
        for (int i = 0; i < GameManager.instance.allVillageNPCs.Count; i++)
        {
            if (GameManager.instance.allVillageNPCs[i].isInDialogue)
            {
                GameManager.instance.allVillageNPCs[i].isInDialogue = false;

                if (GameManager.instance.allVillageNPCs[i].navMeshAgent != null)
                {
                    GameManager.instance.allVillageNPCs[i].navMeshAgent.isStopped = false;
                    GameManager.instance.allVillageNPCs[i].animator.SetBool("IsStanding", false);
                }

                GameManager.instance.allVillageNPCs[i].transform.LookAt(null);                
            }
        }

        ThirdPersonController.instance.canMove = true;
        GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, true);
    }

    #region TimelineSignals: Optional
    public void CheckIfPlayerWasAlreadyAtThava()
    {
        Debug.Log(currCP);

        if (currCP.mBTToCheck != null && !currCP.mBTToCheck.missionTaskCompleted)
        {
            //playableDirector.Stop();
            currCP = currCP.cutsceneToChangeTo;

            //Debug.Log(currCP);
            //Debug.Log(currCP.cutsceneToChangeTo);
            playableDirector.playableAsset = currCP.cutscene;
            playableDirector.Play();
        }
    }

    public void CheckIfPlayerCompletedSideQuestOfBlacksmith()
    {
        if (currCP.mBTToCheck != null && currCP.mBTToCheck.missionTaskCompleted)
        {
            //playableDirector.Stop();
            currCP = currCP.cutsceneToChangeTo;

            //Debug.Log(currCP);
            //Debug.Log(currCP.cutsceneToChangeTo);
            playableDirector.playableAsset = currCP.cutscene;
            playableDirector.Play();
        }
    }

    public void CheckIfPlayerHasGotDamage()
    {
        if (PlayerValueManager.instance.currHP == PlayerValueManager.instance.normalHP)
        {
            //playableDirector.Stop();
            currCP = currCP.cutsceneToChangeTo;

            //Debug.Log(currCP);
            //Debug.Log(currCP.cutsceneToChangeTo);
            playableDirector.playableAsset = currCP.cutscene;
            playableDirector.Play();
        }
    }

    public void ActivateNewTask()
    {
        currCP.missionTaskToActivate.canBeDisplayed = true;

        if (UIManager.missionToDisplay != null && UIManager.missionToDisplay == currCP.corresspondingMission)
        {
            var taskNumber = -1;

            for (int i = 0; i < currCP.corresspondingMission.allMissionTasks.Length; i++)
            {
                if (currCP.corresspondingMission.allMissionTasks[i].mTB == currCP.missionTaskToActivate)
                {
                    taskNumber = i;

                    Debug.Log(currCP.missionTaskToActivate);
                    Debug.Log(currCP);

                    break;
                }
            }

            UIManager.instance.AddAndUpdateMissionDisplayTasks(currCP.missionTaskToActivate, currCP.corresspondingMission.allMissionTasks[taskNumber].taskDescription);
        }
    }

    public void CompleteMissionOrMissionTask()
    {
        if (currCP.missionTaskToComplete != null)
        {
            MissionManager.instance.CompleteMissionTask(currCP.missionToComplete, currCP.missionTaskToComplete);
        }
        else if (currCP.missionToComplete != null)
        {
            MissionManager.instance.CompleteMission(currCP.missionToComplete);
        }
    }

    public void OpenBeerScreen()
    {
        Debug.Log("HJN33333333333333333333333KL");

        //BeerScreenMissionButton.instance.gameObject.SetActive(TavernKeeper.instance.CheckIfNeededForMission());
        TavernKeeper.instance.getBeerScreen.SetActive(true);

        GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, false);

        ThirdPersonController.instance.canMove = false;
        ThirdPersonController.instance._animator.SetFloat("Speed", 0);

        //BeerScreenMissionButton.instance.currStoredMissionTaskBase = null;
    }

    public void CheckExamineTaskProgress()
    {
        currCP.missionTaskToComplete.howManyAlreadyExamined += 1;

        MissionManager.instance.CheckMissionTaskProgress(currCP.corresspondingMission, currCP.missionTaskToComplete);
    }

    public void CheckCollectTaskProgress()
    {
        currCP.missionTaskToComplete.howManyAlreadyCollected += 1;

        MissionManager.instance.CheckMissionTaskProgress(currCP.corresspondingMission, currCP.missionTaskToComplete);
    }

    public void PlayIdleTimeline()
    {
        var go = Interacting.instance.currInteractedObjTrans.gameObject;

        if (go.GetComponent<TavernKeeper>() != null)
        {
            playableDirector.playableAsset = go.GetComponent<TavernKeeper>().idleTimeline;
            playableDirector.Play();
        }
        else if (go.GetComponent<Merchant>() != null)
        {
            playableDirector.playableAsset = go.GetComponent<Merchant>().idleTimeline;
            playableDirector.Play();
        }
    }

    public void CheckArgueMissionTask()
    {
        if (currCP.mBTToCheck.pointsToGainForWin > currCP.mBTToCheck.currGainedPoints)
        {
            currCP = currCP.cutsceneToChangeTo;

            playableDirector.playableAsset = currCP.cutscene;
            playableDirector.Play();
        }
    }

    public void CloseCutsceneTimline()
    {
        cutsceneCam.SetActive(false);

        GameManager.instance.playerGO.transform.parent = playerBaseMeshParentTrans;

        ThirdPersonController.instance.canMove = true;

        GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, true);
    }
    #endregion
}
