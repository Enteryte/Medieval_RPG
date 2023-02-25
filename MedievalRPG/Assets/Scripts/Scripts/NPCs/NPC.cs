using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

[RequireComponent(typeof(Collider), typeof(NavMeshAgent))]
public class NPC : MonoBehaviour, IInteractable
{
    public NPCBaseProfile nPCBP;

    [HideInInspector] public InteractableObjectCanvas iOCanvas;

    public Animator animator;

    public List<CutsceneProfile> possibleNormalDialogues;
    public List<MissionBaseProfile> allSideMissionsWithNPC;

    public NPCWaypoint firstWaypoint;
    public NPCWaypoint currWaypoint;

    public NavMeshAgent navMeshAgent;

    public GameObject iOCanvasLookAtObj;

    public bool isInDialogue = false;
    public bool isNeeded = false;

    [HideInInspector] public List<NPCWaypoint> allCorrWaypoints;

    public CinemachineVirtualCamera nPCCVC;

    public GameObject nPCBaseMesh;

    public bool deactivateOnStart = false;

    [Header("NPC One-Liner")]
    public List<NPCOneLinerProfile> allPossibleOL = new List<NPCOneLinerProfile>();

    public enum NPCAudioType
    {
        none,
        male,
        female
    }

    public NPCAudioType npcAudioType = NPCAudioType.none;

    public NPCOneLinerProfile choosenOL;

    public AudioSource nPCAudioSource;

    [Header("Use For Mission")]
    public bool isUsedForMissions = false;

    public PlayableAsset idleTimeline;

    public CutsceneProfile cPMissionTaskSymon;
    public CutsceneProfile cPMissionTaskMya;

    void Start()
    {
        InstantiateIOCanvas();

        if (firstWaypoint != null)
        {
            SetNewWaypointWithoutStopping(firstWaypoint);

            GameManager.instance.allWalkingNPCs.Add(this);
        }

        GameManager.instance.allVillageNPCs.Add(this);

        if (npcAudioType == NPCAudioType.male)
        {
            for (int i = 0; i < GameManager.instance.allMaleProfiles.Length; i++)
            {
                allPossibleOL.Add(GameManager.instance.allMaleProfiles[i]);
            }

            SetOneLiner();
        }
        else if (npcAudioType == NPCAudioType.female)
        {
            for (int i = 0; i < GameManager.instance.allFemaleProfiles.Length; i++)
            {
                allPossibleOL.Add(GameManager.instance.allFemaleProfiles[i]);
            }

            SetOneLiner();
        }

        if (this.gameObject.GetComponent<NPCScreamingHandler>() == null)
        {
            nPCAudioSource = this.gameObject.GetComponent<AudioSource>();
        }

        if (deactivateOnStart)
        {
            this.gameObject.SetActive(false);
        }

        //if (nPCCVC != null && nPCCVC.m_Follow == null)
        //{
        //    nPCCVC.m_Follow = GameManager.instance.playerGO.transform;
        //}
    }

    public void Update()
    {
        if (currWaypoint != null && !GameManager.instance.gameIsPaused && navMeshAgent.isActiveAndEnabled)
        {
            navMeshAgent.SetDestination(currWaypoint.transform.position);

            if (nPCBaseMesh.transform.position != Vector3.zero)
            {
                nPCBaseMesh.transform.localPosition = Vector3.zero;
                nPCBaseMesh.transform.localRotation = Quaternion.identity;
            }

            //if (Vector3.Distance(transform.position, currWaypoint.transform.position) <= 1f)
            //{
            //    transform.LookAt(currWaypoint.transform);
            //}
        }
    }

    public IEnumerator SetNewWaypoint(NPCWaypoint newWaypoint)
    {
        //transform.LookAt(newWaypoint.transform);

        animator.SetBool("IsStanding", true);

        while (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(newWaypoint.transform.position - transform.position)) > 4f)
        {
            var targetRotation = Quaternion.LookRotation(newWaypoint.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5 * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(newWaypoint.transform.position - transform.position)) <= 4f)
            {
                currWaypoint = newWaypoint;
            }

            yield return null;
        }

        animator.SetBool("IsStanding", false);
    }

    public void SetNewWaypointWithoutStopping(NPCWaypoint newWaypoint)
    {
        transform.LookAt(newWaypoint.transform);

        currWaypoint = newWaypoint;
    }

    public void SetOneLiner()
    {
        var randomOLNumber = Random.Range(0, allPossibleOL.Count - 1);

        choosenOL = allPossibleOL[randomOLNumber];
    }

    public void PlayOneLiner()
    {
        CutsceneManager.instance.currCP = null;

        var randomNumber = Random.Range(0, 100);

        if (randomNumber > 75)
        {
            CutsceneManager.instance.playableDirector.playableAsset = choosenOL.timelineWSubtitles;
            nPCAudioSource.clip = choosenOL.audioCorrToTimeline;
        }
        else
        {
            if (npcAudioType == NPCAudioType.male)
            {
                CutsceneManager.instance.playableDirector.playableAsset = GameManager.instance.quietMaleOL.timelineWSubtitles;
                nPCAudioSource.clip = GameManager.instance.quietMaleOL.audioCorrToTimeline;
            }
            else if (npcAudioType == NPCAudioType.female)
            {
                CutsceneManager.instance.playableDirector.playableAsset = GameManager.instance.quietFemaleOL.timelineWSubtitles;
                nPCAudioSource.clip = GameManager.instance.quietFemaleOL.audioCorrToTimeline;
            }
        }

        CutsceneManager.instance.playableDirector.Play();
        nPCAudioSource.Play();
    }

    public void InstantiateIOCanvas()
    {
        GameObject newIOCanvas = Instantiate(Interacting.instance.interactCanvasPrefab, Interacting.instance.iOCSParentObj.transform);

        newIOCanvas.GetComponent<InteractableObjectCanvas>().correspondingGO = this.gameObject;

        iOCanvas = newIOCanvas.GetComponent<InteractableObjectCanvas>();

        newIOCanvas.transform.SetAsFirstSibling();
    }

    public string GetInteractUIText()
    {
        return "Sprechen";
    }

    public float GetTimeTillInteract()
    {
        return 0;
    }

    public void Interact(Transform transform)
    {
        Interacting.instance.currInteractedObjTrans = this.transform;

        //CutsceneManager.instance.currCP = possibleNormalDialogues[Random.Range(0, possibleNormalDialogues.Count)];
        //CutsceneManager.instance.playableDirector.playableAsset = CutsceneManager.instance.currCP.cutscene;
        //CutsceneManager.instance.playableDirector.Play();

        //if (navMeshAgent != null)
        //{
        //    navMeshAgent.isStopped = true;

        //    animator.SetBool("IsStanding", true);
        //}

        //transform.LookAt(GameManager.instance.playerGO.transform);

        isInDialogue = true;

        //ThirdPersonController.instance.canMove = false;
        ////ShopManager.instance.shopScreen.SetActive(true);

        ////ThirdPersonController.instance.canMove = false;
        //ThirdPersonController.instance._animator.SetFloat("Speed", 0);

        //GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, false);

        for (int i = 0; i < MessageManager.instance.collectedMessageParentObj.transform.childCount; i++)
        {
            Destroy(MessageManager.instance.collectedMessageParentObj.transform.GetChild(i).gameObject);
        }

        CheckIfNeededForMission(true);
    }

    InteractableObjectCanvas IInteractable.iOCanvas()
    {
        return this.iOCanvas;
    }

    public void CheckIfNeededForMission(bool doActions)
    {
        bool isNeeded = false;

        if (doActions)
        {
            CutsceneManager.instance.playableDirector.time = 0;
        }

        //if (UIManager.instance.npcMissionButtonParentObjTrans.childCount > 3)
        //{
        //UIManager.instance.npcMissionButtonParentObjTrans.gameObject.SetActive(true);

            for (int i = 3; i < UIManager.instance.npcMissionButtonParentObjTrans.childCount; i++)
            {
                //for (int y = 0; y < UIManager.instance.npcBtnKillianGOs.Length; y++)
                //{
                Destroy(UIManager.instance.npcMissionButtonParentObjTrans.GetChild(i).gameObject);
                //}
            }
        //}

        for (int i = 0; i < MissionManager.instance.allCurrAcceptedMissions.Count; i++)
        {
            if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks.Length > 1)
            {
                for (int y = 0; y < MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks.Length; y++)
                {
                    if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.missionTaskType == MissionTaskBase.MissionTaskType.talk_To
                        && !MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.missionTaskCompleted
                        && MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.canBeDisplayed)
                    {
                        if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.nPCToTalkToBaseProfile == nPCBP)
                        {
                            if (doActions)
                            {
                                if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.completeAfterInteracted)
                                {
                                    MissionManager.instance.CompleteMissionTask(MissionManager.instance.allCurrAcceptedMissions[i], MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB);
                                }

                                UIManager.instance.npcMissionButtonParentObjTrans.gameObject.SetActive(false);

                                for (int x = 0; x < UIManager.instance.npcBtnKillianGOs.Length; x++)
                                {
                                    UIManager.instance.npcBtnKillianGOs[x].SetActive(false);
                                }

                                if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.dialogToPlayAfterInteracted != null)
                                {
                                    Debug.Log(Interacting.instance.currInteractedObjTrans.gameObject);
                                    GameManager.instance.playerGO.transform.parent = Interacting.instance.currInteractedObjTrans;

                                    CutsceneManager.instance.currCP = MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.dialogToPlayAfterInteracted;

                                    CutsceneManager.instance.playableDirector.playableAsset = CutsceneManager.instance.currCP.cutscene;
                                    CutsceneManager.instance.playableDirector.Play();
                                }
                                else if (MissionManager.instance.allCurrAcceptedMissions[i].missionName == "Mya in der Klemme")
                                {
                                    Debug.Log(Interacting.instance.currInteractedObjTrans.gameObject);
                                    GameManager.instance.playerGO.transform.parent = Interacting.instance.currInteractedObjTrans;

                                    CutsceneManager.instance.playableDirector.playableAsset = Interacting.instance.currInteractedObjTrans.GetComponent<NPC>().idleTimeline;
                                    CutsceneManager.instance.playableDirector.Play();

                                    //for (int x = 0; x < UIManager.instance.npcBtnKillianGOs.Length; x++)
                                    //{
                                    UIManager.instance.npcBtnKillianGOs[0].SetActive(true);
                                    UIManager.instance.npcBtnKillianGOs[1].SetActive(!GameManager.instance.alreadyPlayedAgainstKilian);
                                    //}

                                    Instantiate(UIManager.instance.npcUICloseBtnPrefab, UIManager.instance.npcMissionButtonParentObjTrans);

                                    UIManager.instance.npcMissionButtonParentObjTrans.gameObject.SetActive(true);

                                    ThirdPersonController.instance.canMove = false;

                                    ThirdPersonController.instance._animator.SetFloat("Speed", 0);

                                    GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, false);
                                }
                            }
                            
                            isNeeded = true;

                            break;
                        }
                        else if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.talkToAllNPCs 
                            && MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.canBeDisplayed && isUsedForMissions)
                        {
                            if (doActions)
                            {
                                for (int x = 0; x < UIManager.instance.npcBtnKillianGOs.Length; x++)
                                {
                                    UIManager.instance.npcBtnKillianGOs[x].SetActive(false);
                                }

                                CutsceneManager.instance.playableDirector.time = 0;

                                var newNPCMissionButton = Instantiate(UIManager.instance.npcMissionButtonPrefab, UIManager.instance.npcMissionButtonParentObjTrans);

                                newNPCMissionButton.GetComponent<NPCMissionButton>().storedMT = MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y];
                                newNPCMissionButton.GetComponent<NPCMissionButton>().storedMTB = MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB;

                                if (newNPCMissionButton.GetComponent<NPCMissionButton>().storedMTB != null
                                    && newNPCMissionButton.GetComponent<NPCMissionButton>().storedMTB.decisionSprite != null)
                                {
                                    newNPCMissionButton.GetComponent<NPCMissionButton>().decisionImg.sprite
                                        = newNPCMissionButton.GetComponent<NPCMissionButton>().storedMTB.decisionSprite;
                                }

                                Instantiate(UIManager.instance.npcUICloseBtnPrefab, UIManager.instance.npcMissionButtonParentObjTrans);

                                UIManager.instance.npcMissionButtonParentObjTrans.gameObject.SetActive(true);

                                CutsceneManager.instance.ChangePlayerParentToCurrInteractObj();

                                GameManager.instance.playerGO.transform.parent = Interacting.instance.currInteractedObjTrans;
                                CutsceneManager.instance.playableDirector.playableAsset = idleTimeline;
                                CutsceneManager.instance.playableDirector.Play();

                                ThirdPersonController.instance.canMove = false;

                                ThirdPersonController.instance._animator.SetFloat("Speed", 0);

                                GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, false);
                            }

                            isNeeded = true;
                        }
                    }
                }
            }
            else
            {
                if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks.Length > 0)
                {
                    if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB != null &&
                    MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.missionTaskType == MissionTaskBase.MissionTaskType.talk_To
                    && !MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.missionTaskCompleted)
                    {
                        if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.nPCToTalkToBaseProfile == nPCBP
                            && MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.canBeDisplayed)
                        {
                            if (doActions)
                            {
                                if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.completeAfterInteracted)
                                {
                                    MissionManager.instance.CompleteMissionTask(MissionManager.instance.allCurrAcceptedMissions[i], MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB);
                                }

                                UIManager.instance.npcMissionButtonParentObjTrans.gameObject.SetActive(false);

                                Debug.Log(MissionManager.instance.allCurrAcceptedMissions[i]);
                                Debug.Log(MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0]);
                                Debug.Log(MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.dialogToPlayAfterInteracted);

                                CutsceneManager.instance.currCP = MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.dialogToPlayAfterInteracted;

                                if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.dialogToPlayAfterInteracted != null)
                                {
                                    CutsceneManager.instance.currCP = MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.dialogToPlayAfterInteracted;

                                    GameManager.instance.playerGO.transform.parent = Interacting.instance.currInteractedObjTrans;
                                    CutsceneManager.instance.playableDirector.playableAsset = CutsceneManager.instance.currCP.cutscene;
                                    CutsceneManager.instance.playableDirector.Play();
                                }
                                else if (MissionManager.instance.allCurrAcceptedMissions[i].missionName == "Mya in der Klemme")
                                {
                                    CutsceneManager.instance.playableDirector.playableAsset = Interacting.instance.currInteractedObjTrans.GetComponent<NPC>().idleTimeline;
                                    CutsceneManager.instance.playableDirector.Play();

                                    //for (int x = 0; x < UIManager.instance.npcBtnKillianGOs.Length; x++)
                                    //{
                                    UIManager.instance.npcBtnKillianGOs[0].SetActive(true);
                                    UIManager.instance.npcBtnKillianGOs[1].SetActive(!GameManager.instance.alreadyPlayedAgainstKilian);
                                    //}

                                    Instantiate(UIManager.instance.npcUICloseBtnPrefab, UIManager.instance.npcMissionButtonParentObjTrans);

                                    UIManager.instance.npcMissionButtonParentObjTrans.gameObject.SetActive(true);

                                    ThirdPersonController.instance.canMove = false;

                                    ThirdPersonController.instance._animator.SetFloat("Speed", 0);

                                    GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, false);
                                }
                            }                            

                            isNeeded = true;

                            break;
                        }
                        else if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.talkToAllNPCs 
                            && MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.canBeDisplayed && isUsedForMissions)
                        {
                            if (doActions)
                            {
                                CutsceneManager.instance.playableDirector.time = 0;

                                var newNPCMissionButton = Instantiate(UIManager.instance.npcMissionButtonPrefab, UIManager.instance.npcMissionButtonParentObjTrans);

                                newNPCMissionButton.transform.GetChild(0).GetComponent<NPCMissionButton>().storedMT = MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0];
                                newNPCMissionButton.transform.GetChild(0).GetComponent<NPCMissionButton>().storedMTB = MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB;

                                if (newNPCMissionButton.transform.GetChild(0).GetComponent<NPCMissionButton>().storedMTB != null
                                  && newNPCMissionButton.transform.GetChild(0).GetComponent<NPCMissionButton>().storedMTB.decisionSprite != null)
                                {
                                    newNPCMissionButton.transform.GetChild(0).GetComponent<NPCMissionButton>().decisionImg.sprite
                                        = newNPCMissionButton.transform.GetChild(0).GetComponent<NPCMissionButton>().storedMTB.decisionSprite;
                                }

                                Instantiate(UIManager.instance.npcUICloseBtnPrefab, UIManager.instance.npcMissionButtonParentObjTrans);

                                UIManager.instance.npcMissionButtonParentObjTrans.gameObject.SetActive(true);

                                CutsceneManager.instance.ChangePlayerParentToCurrInteractObj();

                                CutsceneManager.instance.playableDirector.playableAsset = idleTimeline;
                                CutsceneManager.instance.playableDirector.Play();

                                ThirdPersonController.instance.canMove = false;

                                ThirdPersonController.instance._animator.SetFloat("Speed", 0);

                                GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, false);
                            }

                            isNeeded = true;
                        }
                    }
                }
            }
        }

        this.isNeeded = isNeeded;

        if (npcAudioType != NPCAudioType.none && !isNeeded && nPCAudioSource != null && doActions)
        {
            PlayOneLiner();
        }

        if (isNeeded && doActions)
        {
            CutsceneManager.instance.ChangePlayerParentToCurrInteractObj();

            if (this.gameObject.GetComponent<NPCScreamingHandler>() != null)
            {
                CutsceneManager.instance.DeactivateHUDUI();

                this.gameObject.GetComponent<NPCScreamingHandler>().nPCAudioSource.Stop();
                this.gameObject.GetComponent<NPCScreamingHandler>().enabled = false;
            }

            GameManager.instance.cantPauseRN = true;
            FightingActions.instance.PlayerCantMove();
        }
    }
}
