using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GTCBoard : MonoBehaviour, IInteractable
{
    [HideInInspector] public InteractableObjectCanvas iOCanvas;

    public Transform gTCManagerTrans;

    void Start()
    {
        InstantiateIOCanvas();
    }

    public void InstantiateIOCanvas()
    {
        GameObject newIOCanvas = Instantiate(Interacting.instance.interactCanvasPrefab, Interacting.instance.iOCSParentObj.transform);

        newIOCanvas.GetComponent<InteractableObjectCanvas>().correspondingGO = this.gameObject;

        iOCanvas = newIOCanvas.GetComponent<InteractableObjectCanvas>();

        newIOCanvas.transform.SetAsFirstSibling();
    }

    public void Interact(Transform transform)
    {
        gTCManagerTrans.GetComponent<GuessTheCardMinigameManager>().enabled = true;
        gTCManagerTrans.GetComponent<GuessTheCardMinigameManager>().gTCUI.SetActive(true);
        gTCManagerTrans.GetComponent<GuessTheCardMinigameManager>().gTCCamera.enabled = true;

        GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, false);

        ThirdPersonController.instance._animator.SetFloat("Speed", 0);

        for (int i = 0; i < MessageManager.instance.collectedMessageParentObj.transform.childCount; i++)
        {
            Destroy(MessageManager.instance.collectedMessageParentObj.transform.GetChild(i).gameObject);
        }
    }

    public string GetInteractUIText()
    {
        return "Spielen";
    }

    public float GetTimeTillInteract()
    {
        return 1.5f;
    }

    InteractableObjectCanvas IInteractable.iOCanvas()
    {
        return this.iOCanvas;
    }
}
