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

    // Start is called before the first frame update
    void Start()
    {
        InstantiateIOCanvas();
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
        return "Einsammeln";
    }

    public float GetTimeTillInteract()
    {
        //return 1.5f;
        return Interacting.instance.grabItemAnim.length;
    }

    public void Interact(Transform transform)
    {
        GameManager.instance.playerGO.GetComponent<ThirdPersonController>()._animator.SetBool("GrabItem", false);

        InventoryManager.instance.inventory.AddItem(iBP, amountToGet);

        MessageManager.instance.CreateCollectedMessage(iBP);

        CheckIfNeededForMission();

        Interacting.instance.rightHandParentRig.weight = 0;
        Interacting.instance.headRig.weight = 0;

        ThirdPersonController.instance._animator.SetLayerWeight(1, 0);

        Destroy(iOCanvas.gameObject);
        Destroy(this.gameObject);
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
                        if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.itemToCollectBase == iBP)
                        {
                            MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB.howManyAlreadyCollected += amountToGet;

                            MissionManager.instance.CheckMissionTaskProgress(MissionManager.instance.allCurrAcceptedMissions[i], MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[y].mTB);
                        }
                    }
                }
            }
            else
            {
                if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.missionTaskType == MissionTaskBase.MissionTaskType.collect)
                {
                    if (MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.itemToCollectBase == iBP)
                    {
                        MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB.howManyAlreadyCollected += amountToGet;

                        MissionManager.instance.CheckMissionTaskProgress(MissionManager.instance.allCurrAcceptedMissions[i], MissionManager.instance.allCurrAcceptedMissions[i].allMissionTasks[0].mTB);
                    }
                }
            }
        }
    }
}
