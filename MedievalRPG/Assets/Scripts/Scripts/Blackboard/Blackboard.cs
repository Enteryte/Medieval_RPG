using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Blackboard : MonoBehaviour, IInteractable
{
    public static Blackboard instance;

    [HideInInspector] public InteractableObjectCanvas iOCanvas;

    //public Camera blackboardCam;
    //public BlackboardMissionButton[] allBlackboardMB;

    public GameObject blackboardUI;
    public GameObject noteTxtParentGO;
    public TMP_Text currNoteTxt;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Start()
    {
        if (instance == this)
        {
            InstantiateIOCanvas();
        }
    }

    public void Update()
    {
        if (blackboardUI.activeSelf && !noteTxtParentGO.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, true);

                blackboardUI.SetActive(false);

                ThirdPersonController.instance.canMove = true;
            }
        }
        else if (blackboardUI.activeSelf && noteTxtParentGO.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                noteTxtParentGO.SetActive(false);
            }
        }
    }

    //public void AddMissionToBlackboard(MissionBaseProfile missionToAdd)
    //{
    //    if (!MissionManager.instance.allCurrAcceptedMissions.Contains(missionToAdd))
    //    {
    //        allOpenNotAcceptedMissions.Add(missionToAdd);
    //    }
    //}

    //public void RemoveMissionFromBlackboard(MissionBaseProfile missionToRemove)
    //{
    //    allOpenNotAcceptedMissions.Remove(missionToRemove);

    //    if (!MissionManager.instance.allCurrAcceptedMissions.Contains(missionToRemove))
    //    {
    //        MissionManager.instance.allCurrAcceptedMissions.Add(missionToRemove);
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
        return "Ansehen";
    }

    public float GetTimeTillInteract()
    {
        return 0;
    }

    public void Interact(Transform transform)
    {      
        GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, false);

        ThirdPersonController.instance._animator.SetFloat("Speed", 0);

        for (int i = 0; i < MessageManager.instance.collectedMessageParentObj.transform.childCount; i++)
        {
            Destroy(MessageManager.instance.collectedMessageParentObj.transform.GetChild(i).gameObject);
        }

        blackboardUI.SetActive(true);

        //blackboardCam.enabled = true;

        ThirdPersonController.instance.canMove = false;
    }

    InteractableObjectCanvas IInteractable.iOCanvas()
    {
        return this.iOCanvas;
    }
}
