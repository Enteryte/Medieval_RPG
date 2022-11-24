using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TavernKeeper : MonoBehaviour, IInteractable
{
    public static TavernKeeper instance;

    public NPCBaseProfile nPCBP;

    [HideInInspector] public InteractableObjectCanvas iOCanvas;

    public GameObject iOCanvasLookAtObj;

    public Animator animator;

    public int maxDrunkBeer = 5;
    public int currBeerCount = 0;

    public float timeTillNotDrunk = 25;

    public Coroutine beereDebuffCoro;

    public int beerBuyPrice;

    public CutsceneProfile normalTalkCP;

    [Header("Get Beer UI")]
    public GameObject getBeerScreen;
    public GameObject missionButton;// if tk has a second mission

    [Header("Missions")]
    public List<MissionBaseProfile> allCorrMissions;
    public MissionTaskBase currCorrTask;

    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        InstantiateIOCanvas();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            BuyAndDrinkBeer();
        }
    }

    public void BuyAndDrinkBeer()
    {
        getBeerScreen.SetActive(false);

        PlayerValueManager.instance.money -= beerBuyPrice;

        currBeerCount += 1;

        if (beereDebuffCoro != null)
        {
            StopCoroutine(beereDebuffCoro);
        }

        var gettingDrunkChanceRndmValue = Random.value;

        if (currBeerCount <= maxDrunkBeer)
        {
            if (currBeerCount == 2)
            {
                if (gettingDrunkChanceRndmValue > 0.98f)
                {
                    FaintingAfterGotDrunk();

                    return;
                }
            }
            else if (currBeerCount == 3)
            {
                if (gettingDrunkChanceRndmValue > 0.75f)
                {
                    FaintingAfterGotDrunk();

                    return;
                }
            }
            else if (currBeerCount == 4)
            {
                if (gettingDrunkChanceRndmValue > 0.5f)
                {
                    FaintingAfterGotDrunk();

                    return;
                }
            }
            else if (currBeerCount == 5)
            {
                if (gettingDrunkChanceRndmValue > 0.25f)
                {
                    FaintingAfterGotDrunk();

                    return;
                }
            }
        }
        else
        {
            FaintingAfterGotDrunk();

            return;
        }

        beereDebuffCoro = StartCoroutine(ResetBeerDebuff());

        ThirdPersonController.instance.canMove = true;
        GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, true);

        CutsceneManager.instance.cutsceneCam.SetActive(false);
    }

    public void DontBuyBeer()
    {
        getBeerScreen.SetActive(false);

        ThirdPersonController.instance.canMove = true;
        GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, true);

        CutsceneManager.instance.cutsceneCam.SetActive(false);
    }

    public IEnumerator ResetBeerDebuff()
    {
        float time = 0;

        while (time < timeTillNotDrunk)
        {
            time += Time.deltaTime;

            yield return null;
        }

        currBeerCount = 0;

        beereDebuffCoro = null;
    }

    public void FaintingAfterGotDrunk() // If the player drank too much.
    {
        Debug.Log("DRANK TOO MUCH");

        StopCoroutine(beereDebuffCoro);
        beereDebuffCoro = null;

        currBeerCount = 0;

        RespawnManager.instance.RespawnPlayer(RespawnManager.instance.playerGotTooDrunkSpawnTrans);

        ThirdPersonController.instance.canMove = true;
        GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, true);

        CutsceneManager.instance.cutsceneCam.SetActive(false);
    }

    public void OpenGetBeerScreen()
    {
        getBeerScreen.SetActive(true);

        GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, false);
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
        var neededForMission = CheckIfNeededForMission();

        if (neededForMission)
        {
            if (currCorrTask.dialogToPlayAfterInteracted != null)
            {
                CutsceneManager.instance.currCP = currCorrTask.dialogToPlayAfterInteracted;
                CutsceneManager.instance.playableDirector.playableAsset = CutsceneManager.instance.currCP.cutscene;
            }
            else
            {
                CutsceneManager.instance.currCP = normalTalkCP;
                CutsceneManager.instance.playableDirector.playableAsset = CutsceneManager.instance.currCP.cutscene;
            }
            
            CutsceneManager.instance.playableDirector.Play();

            BeerScreenMissionButton.instance.gameObject.SetActive(true);
        }
        else if (!neededForMission)
        {
            CutsceneManager.instance.currCP = normalTalkCP;
            CutsceneManager.instance.playableDirector.playableAsset = CutsceneManager.instance.currCP.cutscene;
            CutsceneManager.instance.playableDirector.Play();

            BeerScreenMissionButton.instance.gameObject.SetActive(false);

            //if (navMeshAgent != null)
            //{
            //    navMeshAgent.isStopped = true;

            //    animator.SetBool("IsStanding", true);
            //}
        }

        transform.LookAt(GameManager.instance.playerGO.transform);

        //isInDialogue = true;

        GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, false);

        ThirdPersonController.instance.canMove = false;
        ThirdPersonController.instance._animator.SetFloat("Speed", 0);

        for (int i = 0; i < MessageManager.instance.collectedMessageParentObj.transform.childCount; i++)
        {
            Destroy(MessageManager.instance.collectedMessageParentObj.transform.GetChild(i).gameObject);
        }

        //CheckIfNeededForMission();
    }

    InteractableObjectCanvas IInteractable.iOCanvas()
    {
        return this.iOCanvas;
    }

    public bool CheckIfNeededForMission()
    {
        for (int i = 0; i < MissionManager.instance.allCurrAcceptedMissions.Count; i++)
        {
            if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks.Length > 1)
            {
                for (int y = 0; y < MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks.Length; y++)
                {
                    if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.missionTaskType == MissionTaskBase.MissionTaskType.talk_To)
                    {
                        if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.nPCToTalkToBaseProfile == nPCBP 
                            && !MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.missionTaskCompleted)
                        {
                            if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.completeAfterInteracted)
                            {
                                MissionManager.instance.CompleteMissionTask(MissionManager.instance.allCurrAcceptedMissions[i], MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB);
                            }

                            currCorrTask = MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB;
                            BeerScreenMissionButton.instance.currStoredMissionTaskBase = MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB;

                            return true;
                        }
                        //else if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.talkToAllNPCs)
                        //{
                        //    //var newNPCMissionButton = Instantiate(UIManager.instance.npcMissionButtonPrefab, UIManager.instance.npcMissionButtonParentObjTrans);

                        //    //newNPCMissionButton.GetComponent<NPCMissionButton>().storedMT = MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y];
                        //    //newNPCMissionButton.GetComponent<NPCMissionButton>().storedMTB = MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB;

                        //    currCorrTask = MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB;
                        //    BeerScreenMissionButton.instance.currStoredMissionTaskBase = MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB;

                        //    return true;
                        //}
                    }
                }
            }
            else
            {
                if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks.Length > 0 &&
                    MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.missionTaskType == MissionTaskBase.MissionTaskType.talk_To)
                {
                    if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.nPCToTalkToBaseProfile == nPCBP
                        && !MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.missionTaskCompleted)
                    {
                        if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.completeAfterInteracted)
                        {
                            MissionManager.instance.CompleteMissionTask(MissionManager.instance.allCurrAcceptedMissions[i], MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB);
                        }

                        currCorrTask = MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB;
                        BeerScreenMissionButton.instance.currStoredMissionTaskBase = MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB;

                        return true;
                    }
                    //else if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.talkToAllNPCs)
                    //{
                    //    //var newNPCMissionButton = Instantiate(UIManager.instance.npcMissionButtonPrefab, UIManager.instance.npcMissionButtonParentObjTrans);

                    //    //newNPCMissionButton.GetComponent<NPCMissionButton>().storedMT = MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0];
                    //    //newNPCMissionButton.GetComponent<NPCMissionButton>().storedMTB = MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB;

                    //    currCorrTask = MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB;
                    //    BeerScreenMissionButton.instance.currStoredMissionTaskBase = MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB;

                    //    return true;
                    //}
                }
            }
        }

        return false;
    }
}
