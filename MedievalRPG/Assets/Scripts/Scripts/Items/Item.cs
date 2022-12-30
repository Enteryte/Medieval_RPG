using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Item : MonoBehaviour, IInteractable
{
    public ItemBaseProfile iBP;

    [HideInInspector] public InteractableObjectCanvas iOCanvas;

    public int amountToGet = 1;

    public Transform whereToGrabItemTrans;

    [Header("Collectable")]
    public MissionTaskBase corresspondingMissionTask;
    public CutsceneProfile cutsceneToPlayAfterCollected;

    [Header("Item To Examine")]
    public bool onlyExamineObject = false;
    public bool hasExaminedObject = false;
    public CutsceneProfile cutsceneToPlayAfterExamine;

    // Start is called before the first frame update
    void Start()
    {
        if (!onlyExamineObject/* && corresspondingMissionTask == null*/)
        {
            InstantiateIOCanvas();
        }
        else if (onlyExamineObject)
        {
            MissionManager.instance.objectsToExamine.Add(this);
        }

        GameManager.instance.allInteractableObjects.Add(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        if (onlyExamineObject)
        {
            return "Untersuchen";
        }
        else
        {
            return "Einsammeln";
        }
    }

    public float GetTimeTillInteract()
    {
        //return 1.5f;
        if (onlyExamineObject)
        {
            return 0;
        }
        else
        {
            return Interacting.instance.grabItemAnim.length;
        }
    }

    public void Interact(Transform transform)
    {
        if (!onlyExamineObject)
        {
            GameManager.instance.playerGO.GetComponent<ThirdPersonController>()._animator.SetBool("GrabItem", false);

            InventoryManager.instance.inventory.AddItem(iBP, amountToGet);

            MessageManager.instance.CreateCollectedMessage(iBP);

            CheckIfNeededForMission();

            Interacting.instance.rightHandParentRig.weight = 0;
            Interacting.instance.headRig.weight = 0;

            ThirdPersonController.instance._animator.SetLayerWeight(1, 0);

            if (cutsceneToPlayAfterCollected != null)
            {
                CutsceneManager.instance.currCP = cutsceneToPlayAfterCollected;
                CutsceneManager.instance.playableDirector.playableAsset = cutsceneToPlayAfterCollected.cutscene;
                CutsceneManager.instance.playableDirector.Play();
            }

            //Destroy(iOCanvas.gameObject);
            //Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }
        else
        {
            CutsceneManager.instance.currCP = cutsceneToPlayAfterExamine;
            CutsceneManager.instance.playableDirector.playableAsset = cutsceneToPlayAfterExamine.cutscene;
            CutsceneManager.instance.playableDirector.Play();

            //Destroy(iOCanvas.gameObject);
            this.gameObject.SetActive(false);

            Interacting.instance.howToInteractGO.SetActive(false);
        }
    }

    InteractableObjectCanvas IInteractable.iOCanvas()
    {
        return this.iOCanvas;
    }

    public void CheckIfNeededForMission()
    {
        for (int i = 0; i < MissionManager.instance.allCurrAcceptedMissions.Count; i++)
        {
            if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks.Length > 1)
            {
                for (int y = 0; y < MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks.Length; y++)
                {
                    if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.missionTaskType == MissionTaskBase.MissionTaskType.collect)
                    {
                        if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.itemToCollectBase == iBP
                            || MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.itemToCollectBase2 != null 
                            && MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.itemToCollectBase2 == iBP)
                        {
                            if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.completeAfterInteracted)
                            {
                                MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.howManyAlreadyCollected += amountToGet;

                                MissionManager.instance.CheckMissionTaskProgress(MissionManager.instance.allCurrAcceptedMissions[i], MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB);
                            }
                        }
                    }
                }
            }
            else
            {
                if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks.Length > 0)
                {
                    if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.missionTaskType == MissionTaskBase.MissionTaskType.collect)
                    {
                        if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.itemToCollectBase == iBP
                            || MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.itemToCollectBase2 != null
                                && MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.itemToCollectBase2 == iBP)
                        {
                            if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.completeAfterInteracted)
                            {
                                MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.howManyAlreadyCollected += amountToGet;

                                MissionManager.instance.CheckMissionTaskProgress(MissionManager.instance.allCurrAcceptedMissions[i], MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB);
                            }
                        }
                    }
                }
            }
        }
    }
}
