using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
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

    public CutsceneProfile[] allCSWAllowedPausing;

    public LoadingScreenProfile afterCreditsLSP;

    [Header("CS w. Alchemist")]
    public GameObject alchemistGO;

    [Header("SQ w. Kilian")]
    public GameObject myaGO;

    [Header("Tutorial")]
    public TutorialBaseProfile decisionTutorial;

    [Header("Skip Cutscene")]
    public Animator cutsceneUIAnimator;
    public GameObject skipCutsceneUI;

    [Header("Player Death")]
    public TimelineAsset afterPlayerDiedTL;

    [Header("Options")]
    public GameObject subtitleTxtObj;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playableDirector.playableAsset != null && playableDirector.playableAsset.name == afterPlayerDiedTL.name)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && playableDirector.time < PlayerValueManager.instance.afterPlayerDiedSkippingTime1)
            {
                cutsceneUIAnimator.Rebind();
                cutsceneUIAnimator.enabled = true;

                if (!GameManager.instance.playedTheGameThrough)
                {
                    pressedTime += Time.deltaTime;

                    if (playableDirector.time > PlayerValueManager.instance.afterPlayerDiedSkippingTime1)
                    {
                        cutsceneUIAnimator.enabled = false;
                        skipCutsceneUI.SetActive(false);
                    }

                    if (pressedTime >= timeToPressToSkipCS)
                    {
                        SkipDeathCutscene();

                        pressedTime = 0;
                    }                   
                }
                else
                {
                    SkipDeathCutscene();
                }
            }

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                cutsceneUIAnimator.enabled = false;
                skipCutsceneUI.SetActive(false);
            }
        }
        else if (playableDirector.playableAsset != null && !TutorialManager.instance.bigTutorialUI.activeSelf /*&& !TutorialManager.instance.smallTutorialUI.activeSelf */
            && !GameManager.instance.pauseMenuScreen.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && currCP != null && currCP.isNotADialogue && !currCP.cantBeSkipped)
            {
                cutsceneUIAnimator.Rebind();
                cutsceneUIAnimator.enabled = true;

                if (!GameManager.instance.playedTheGameThrough)
                {
                    pressedTime += Time.deltaTime;

                    if (pressedTime >= timeToPressToSkipCS)
                    {
                        SkipCutscene();

                        pressedTime = 0;
                    }
                }
                else
                {
                    SkipCutscene();
                }
            }

            if (Input.GetKeyDown(KeyCode.Return) && !currCP.isNotADialogue && !currCP.cantBeSkipped)
            {
                SkipSentenceInDialogue();
            }

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                cutsceneUIAnimator.enabled = false;
                skipCutsceneUI.SetActive(false);

                //pressedTime = 0;
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
        if (currCP.timesWhenNewSentenceStarts.Count > 0)
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
    }

    public void SkipCutscene(/*float timeTillWhereToSkip*/)
    {
        if (CutsceneManager.instance.playableDirector.playableAsset != null && CutsceneManager.instance.playableDirector.playableAsset == CutsceneManager.instance.afterPlayerDiedTL)
        {
            playableDirector.time = PlayerValueManager.instance.afterPlayerDiedSkippingTime1;

            //Debug.Log(playableDirector.time);
            //Debug.Log(currCP.timeTillWhereToSkip);

            cutsceneUIAnimator.enabled = false;
            skipCutsceneUI.SetActive(false);

            return;
        }

        Debug.Log(playableDirector.time);
        Debug.Log(currCP.timeTillWhereToSkip);
        if (playableDirector.time <= currCP.timeTillWhereToSkip)
        {
            playableDirector.time = currCP.timeTillWhereToSkip;
        }
        else if (currCP.timeTillWhereToSkip2 > 0)
        {
            playableDirector.time = currCP.timeTillWhereToSkip2;
        }

        cutsceneUIAnimator.enabled = false;
        skipCutsceneUI.SetActive(false);
    }

    public void SkipDeathCutscene()
    {
        playableDirector.time = PlayerValueManager.instance.afterPlayerDiedSkippingTime1;

        Debug.Log(playableDirector.time);

        cutsceneUIAnimator.enabled = false;
        skipCutsceneUI.SetActive(false);
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

        ActivateHUDUI();

        playableDirector.Stop();

        GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, true);

        myaGO.transform.parent = null;

        playableDirector.time = 0;
    }

    public void DisplayDecisions()
    {
        playableDirector.time = 0;

        TutorialManager.instance.CheckIfTutorialIsAlreadyCompleted(decisionTutorial);

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

            Debug.Log("fgthjklödw-_______________________");

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

        GameManager.instance.gameIsPaused = false;
        GameManager.instance.cantPauseRN = false;
    }

    #region TimelineSignals: Optional
    public void CheckIfPlayerWasAlreadyAtThava()
    {
        if (currCP.mBTToCheck != null && !currCP.mBTToCheck.missionTaskCompleted)
        {
            playableDirector.time = 0;

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
        if (currCP == null)
        {
            return;
        }

        if (PlayerValueManager.instance.CurrHP == PlayerValueManager.instance.normalHP)
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

                    break;
                }
            }

            UIManager.instance.AddAndUpdateMissionDisplayTasks(currCP.missionTaskToActivate, currCP.corresspondingMission.allMissionTasks[taskNumber].taskDescription);
        }
        else
        {
            UIAnimationHandler.instance.howChangedMissionTxt.text = UIAnimationHandler.instance.updatedMissionString;
            UIAnimationHandler.instance.addedMissionTxt.text = currCP.corresspondingMission.missionName;
            UIAnimationHandler.instance.AnimateAddedNewMissionMessage();
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

    public void OpenPrickMinigameAfterCutscene()
    {
        PrickBoard.instance.isPlayingAgainstKilian = true;

        PrickBoard.instance.Interact(null);

        PrickMinigameManager.instance.StartNewMatch();
    }

    public void SetMyasParent()
    {
        myaGO.transform.parent = Interacting.instance.currInteractedObjTrans;
    }

    public void SetPlayerParentToAlchemist()
    {
        GameManager.instance.playerGO.transform.parent = alchemistGO.transform;
    }

    public void ChangeSceneToMainMenu()
    {
        StartScreenManager.currSceneIndex = 0;

        if (LoadingScreen.instance != null)
        {
            LoadingScreen.currLSP = afterCreditsLSP;
            LoadingScreen.instance.placeNameTxt.text = LoadingScreen.currLSP.placeName;
            LoadingScreen.instance.backgroundImg.sprite = LoadingScreen.currLSP.backgroundSprite;
            LoadingScreen.instance.descriptionTxt.text = LoadingScreen.currLSP.descriptionTextString;

            LoadingScreen.instance.gameObject.SetActive(true);
            LoadingScreen.instance.ActivateAnimator();

            //SceneChangeManager.instance.GetComponent<Animator>().enabled = false;
            SceneChangeManager.instance.GetComponent<Animator>().Rebind();
            //SceneChangeManager.instance.GetComponent<Animator>().enabled = true;

            SceneChangeManager.instance.loadingScreen.SetActive(true);
            SceneChangeManager.instance.gameObject.GetComponent<Animator>().Play("OpenLoadingScreenInStartScreenAnim");
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public void PlayIdleTimeline()
    {
        playableDirector.time = 0;

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
        else if (go.GetComponent<NPC>() != null)
        {
            playableDirector.playableAsset = go.GetComponent<NPC>().idleTimeline;
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

    public void SetCurrentCutsceneToNull()
    {
        currCP = null;
        playableDirector.playableAsset = null;
    }

    public void SetStartDeathCutscene()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            LoadingScreen.currLSP = PlayerValueManager.instance.deathLSP;
            LoadingScreen.currSpawnPos = new Vector3(0, 0, 0);
            LoadingScreen.currSpawnRot = Quaternion.identity;

            LoadingScreen.instance.placeNameTxt.text = PlayerValueManager.instance.deathLSP.placeName;
            LoadingScreen.instance.backgroundImg.sprite = PlayerValueManager.instance.deathLSP.backgroundSprite;
            LoadingScreen.instance.descriptionTxt.text = PlayerValueManager.instance.deathLSP.descriptionTextString;

            ////SceneChangeManager.instance.GetComponent<Animator>().enabled = false;
            //SceneChangeManager.instance.GetComponent<Animator>().Rebind();
            ////SceneChangeManager.instance.GetComponent<Animator>().enabled = true;

            //SceneChangeManager.instance.loadingScreen.SetActive(true);

            LoadingScreen.instance.gameObject.SetActive(true);
            LoadingScreen.instance.ActivateAnimator();
            SceneChangeManager.instance.gameObject.GetComponent<Animator>().Play("OpenLoadingScreenInStartScreenAnim");

            SceneChangeManager.instance.wentThroughTrigger = true;
        }
    }

    public void ActivateHUDUI()
    {
        GameManager.instance.interactCanvasasParentGO.SetActive(true);
        GameManager.instance.mapGO.SetActive(true);
        GameManager.instance.hotbarGO.SetActive(true);
        GameManager.instance.playerStatsGO.SetActive(true);

        LoadingScreen.instance.DeactivateAnimator();
    }

    public void DeactivateHUDUI()
    {
        GameManager.instance.interactCanvasasParentGO.SetActive(false);
        GameManager.instance.mapGO.SetActive(false);
        GameManager.instance.hotbarGO.SetActive(false);
        GameManager.instance.playerStatsGO.SetActive(false);
    }

    public void SetCurrentTimeoNull()
    {
        currCP = null;
    }

    public void ActivateChangingDaytime()
    {
        GameManager.instance.changeDaytime = true;
        TavernKeeper.instance.DisplayTavernKeeperUI();
        GameManager.instance.hdrpTOD.m_timeOfDayMultiplier = 1;
    }

    public void SleepTillMorning()
    {
        // -------------------> HIER Zeit ändern -> zum Morgen.
        GameManager.instance.hdrpTOD.TimeOfDay = 7.8f;

        DebuffManager.instance.StopAllBuffs();
        DebuffManager.instance.StopAllDebuffs();

        PlayerValueManager.instance.CurrHP = PlayerValueManager.instance.normalHP;
        PlayerValueManager.instance.healthSlider.value = PlayerValueManager.instance.CurrHP;

        PlayerValueManager.instance.currStamina = PlayerValueManager.instance.normalStamina;
        PlayerValueManager.instance.staminaSlider.value = PlayerValueManager.instance.currStamina;
    }

    public void SleepTillEvening()
    {
        // -------------------> HIER Zeit ändern -> zum Abend.
        GameManager.instance.hdrpTOD.TimeOfDay = 16.7f;

        DebuffManager.instance.StopAllBuffs();
        DebuffManager.instance.StopAllDebuffs();

        PlayerValueManager.instance.CurrHP = PlayerValueManager.instance.normalHP;
        PlayerValueManager.instance.healthSlider.value = PlayerValueManager.instance.CurrHP;

        PlayerValueManager.instance.currStamina = PlayerValueManager.instance.normalStamina;
        PlayerValueManager.instance.staminaSlider.value = PlayerValueManager.instance.currStamina;
    }

    public void FadeMusicOut()
    {
        StartCoroutine(FadeOldMusicOut());
    }

    public void FadeMusicIn()
    {
        Debug.Log("NEW MUSIC");

        StartCoroutine(FadeNewMusicIn());
    }

    public IEnumerator FadeOldMusicOut()
    {
        float currentTime = 0;
        float start = GameManager.instance.musicAudioSource.volume;

        while (currentTime < 1f)
        {
            currentTime += Time.deltaTime;
            GameManager.instance.musicAudioSource.volume = Mathf.Lerp(start, 0, currentTime / 1f);

            yield return null;
        }

        GameManager.instance.musicAudioSource.clip = null;

        yield break;
    }

    public IEnumerator FadeNewMusicIn()
    {
        Debug.Log("NEW MUSIC");

        float currentTime = 0;
        float start = 0;

        GameManager.instance.musicAudioSource.volume = 0;

        GameManager.instance.musicAudioSource.clip = currCP.musicAfterCSFinished;
        GameManager.instance.musicAudioSource.Play();

        while (currentTime < 1f)
        {
            currentTime += Time.deltaTime;
            GameManager.instance.musicAudioSource.volume = Mathf.Lerp(start, OptionManager.instance.musicSlider.value, currentTime / 1f);

            yield return null;
        }

        yield break;
    }
    #endregion
}
