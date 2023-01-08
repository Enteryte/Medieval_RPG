using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Merchant : MonoBehaviour, IInteractable
{
    public MerchantBaseProfile mBP;
    public NPCBaseProfile nPCBP;
    [HideInInspector] public InteractableObjectCanvas iOCanvas;

    public GameObject iOCanvasLookAtObj;

    [Header("Missions")]
    public List<MissionBaseProfile> allCorrMissions;
    public MissionTaskBase currCorrTask;
    public List<MissionTaskBase> allCurrCorrTasks;

    [HideInInspector] public bool neededForMission = false;

    [Header("Cutscene Values")]
    public bool isLookingNPC = false;
    public GameObject normalMerchantObj;
    public Transform whereToSetPlayerTrans;

    public PlayableAsset idleTimeline;

    public CinemachineVirtualCamera idleCVC;

    [Header("Shop-Audio-Files")]
    public PlayableAsset[] mStartShopPA;
    public PlayableAsset[] mAfterBoughtShopPA;
    public PlayableAsset[] mEndBuyingShopPA;

    //public float maxMoneyMerchantCanSpend;
    //public float currMoneyMerchantSpend = 0;

    //public float timeTillMoneyResets;
    //[HideInInspector] public float currPassedTime;

    // Start is called before the first frame update
    void Start()
    {
        InstantiateIOCanvas();

        GameManager.instance.allMerchants.Add(this);
    }

    public void SetShopAudioFile(PlayableAsset[] timelinesToChooseFrom)
    {
        var timelineNumber = Random.Range(0, timelinesToChooseFrom.Length);

        CutsceneManager.instance.playableDirector.playableAsset = timelinesToChooseFrom[timelineNumber];
        CutsceneManager.instance.playableDirector.Play();
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    if (currMoneyMerchantSpend > 0)
    //    {
    //        currPassedTime += Time.deltaTime;

    //        if (currPassedTime >= timeTillMoneyResets)
    //        {
    //            ResetMerchantSpendableMoney();
    //        }
    //    }
    //}

    //public void ResetMerchantSpendableMoney()
    //{
    //    currMoneyMerchantSpend = 0;
    //    currPassedTime = 0;

    //    Debug.Log("RESET");
    //}

    //public void AddSpendableMoney(float moneyToAdd)
    //{
    //    currMoneyMerchantSpend -= moneyToAdd;

    //    if (currMoneyMerchantSpend <= 0)
    //    {
    //        currPassedTime = 0;

    //        if (currMoneyMerchantSpend < 0)
    //        {
    //            currMoneyMerchantSpend = 0;
    //        }
    //    }
    //}

    //public void RemoveSpendableMoney(float moneyToRemove)
    //{
    //    currMoneyMerchantSpend += moneyToRemove;

    //    if (currMoneyMerchantSpend > maxMoneyMerchantCanSpend)
    //    {
    //        Debug.Log("SPENDED MONEY IS TO HIGH!");
    //    }
    //}

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
        neededForMission = CheckIfNeededForMission();

        whereToSetPlayerTrans.gameObject.SetActive(true);
        Interacting.instance.currInteractedObjTrans = this.transform;

        //else
        //{
        ShopManager.instance.currMerchant = this;

        ShopManager.currMBP = mBP;

        if (mBP.changesItems)
        {
            // WIP
            Debug.Log("WIP!");
        }
        else
        {
            ShopManager.instance.currSLBP = mBP.shopListBaseProfile;
        }

        //ShopManager.instance.DisplayMainScreenButtons();
        //ShopManager.instance.DisplayShopItems();

        ThirdPersonController.instance.canMove = false;
        //ShopManager.instance.shopScreen.SetActive(true);

        //ThirdPersonController.instance.canMove = false;
        ThirdPersonController.instance._animator.SetFloat("Speed", 0);

        for (int i = 0; i < MessageManager.instance.collectedMessageParentObj.transform.childCount; i++)
        {
            Destroy(MessageManager.instance.collectedMessageParentObj.transform.GetChild(i).gameObject);
        }

        if (neededForMission && !currCorrTask.dialogToPlayAfterInteracted.alreadyPlayedCutscene)
        {
            StartCoroutine(CutsceneManager.instance.StartCutsceneFadeIn(currCorrTask.dialogToPlayAfterInteracted));
            //CutsceneManager.instance.currCP = currCorrTask.dialogToPlayAfterInteracted;
            //CutsceneManager.instance.playableDirector.playableAsset = CutsceneManager.instance.currCP.cutscene;
            //CutsceneManager.instance.playableDirector.Play();

            currCorrTask.dialogToPlayAfterInteracted.alreadyPlayedCutscene = true;
        }
        else
        {
            ShopManager.instance.DisplayMainScreenButtons();

            CutsceneManager.instance.ChangePlayerParentToCurrInteractObj();
            CutsceneManager.instance.DeactivateHUDUI();

            normalMerchantObj.SetActive(false);

            var randomMerchantDialogue = Random.Range(0, mStartShopPA.Length);

            CutsceneManager.instance.playableDirector.playableAsset = mStartShopPA[randomMerchantDialogue];
            CutsceneManager.instance.playableDirector.Play();

            //CutsceneManager.instance.SetAndPlayCutscene();
        }

        for (int i = 0; i < GameManager.instance.allNPCScreamingHandler.Count; i++)
        {
            GameManager.instance.allNPCScreamingHandler[i].nPCAudioSource.Pause();
            GameManager.instance.allNPCScreamingHandler[i].isPlayingAudio = false;
        }

        //CheckIfNeededForMission();
        //}
    }

    InteractableObjectCanvas IInteractable.iOCanvas()
    {
        return this.iOCanvas;
    }

    public bool CheckIfNeededForMission()
    {
        allCorrMissions.Clear();
        allCurrCorrTasks.Clear();

        for (int i = 0; i < MissionManager.instance.allCurrAcceptedMissions.Count; i++)
        {
            if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks.Length > 1)
            {
                for (int y = 0; y < MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks.Length; y++)
                {
                    if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.missionTaskType == MissionTaskBase.MissionTaskType.talk_To
                        && MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.canBeDisplayed)
                    {
                        if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.nPCToTalkToBaseProfile == nPCBP 
                            && !MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.missionTaskCompleted)
                        {
                            if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.completeAfterInteracted)
                            {
                                MissionManager.instance.CompleteMissionTask(MissionManager.instance.allCurrAcceptedMissions[i], MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB);
                            }

                            currCorrTask = MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB;
                            allCurrCorrTasks.Add(MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB);
                        }
                    }
                }               
            }
            else
            {
                if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks.Length > 0)
                {
                    if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.missionTaskType == MissionTaskBase.MissionTaskType.talk_To
                        && MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.canBeDisplayed)
                    {
                        if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.nPCToTalkToBaseProfile == nPCBP
                            && !MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.missionTaskCompleted)
                        {
                            if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.completeAfterInteracted)
                            {
                                MissionManager.instance.CompleteMissionTask(MissionManager.instance.allCurrAcceptedMissions[i], MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB);
                            }

                            currCorrTask = MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB;
                            allCurrCorrTasks.Add(MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB);
                        }
                    }
                }
            }
        }

        for (int i = 0; i < MissionManager.instance.allCurrOpenNotAcceptedMissions.Count; i++)
        {
            if (MissionManager.instance.allCurrOpenNotAcceptedMissions[i].nPCWhereToGetMissionFrom == nPCBP
                /*&& MissionManager.instance.allCurrOpenNotAcceptedMissions[i].isActive*/)
            {
                allCorrMissions.Add(MissionManager.instance.allCurrOpenNotAcceptedMissions[i]);

                Debug.Log(MissionManager.instance.allCurrOpenNotAcceptedMissions[i].nPCWhereToGetMissionFrom);
                Debug.Log(nPCBP);
            }
        }

        if (allCurrCorrTasks.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //public void CheckIfNeededForMission()
    //{
    //    for (int i = 0; i < MissionManager.instance.allCurrAcceptedMissions.Count; i++)
    //    {
    //        if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks.Length > 1)
    //        {
    //            for (int y = 0; y < MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks.Length; y++)
    //            {
    //                if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.missionTaskType == MissionTaskBase.MissionTaskType.talk_To)
    //                {
    //                    if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.nPCToTalkToBaseProfile == nPCBP)
    //                    {
    //                        MissionManager.instance.CompleteMissionTask(MissionManager.instance.allCurrAcceptedMissions[i], MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB);
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.missionTaskType == MissionTaskBase.MissionTaskType.talk_To)
    //            {
    //                if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.nPCToTalkToBaseProfile == nPCBP)
    //                {
    //                    MissionManager.instance.CompleteMissionTask(MissionManager.instance.allCurrAcceptedMissions[i], MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB);
    //                }
    //            }
    //        }
    //    }
    //}
}
