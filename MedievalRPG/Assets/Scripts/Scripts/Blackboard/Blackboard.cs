using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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

            if (SceneManager.GetActiveScene().buildIndex != 1)
            {
                this.gameObject.SetActive(false);
            }
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
        if (!blackboardUI)
        {
            return;
        }

        if (blackboardUI.activeSelf && !noteTxtParentGO.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.instance.ContinueGame();

                blackboardUI.SetActive(false);

                ThirdPersonController.instance.canMove = true;

                GameManager.instance.cantPauseRN = false;

                GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, true);
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

        GameManager.instance.PauseGame();
        GameManager.instance.cantPauseRN = true;
    }

    InteractableObjectCanvas IInteractable.iOCanvas()
    {
        return this.iOCanvas;
    }
}
