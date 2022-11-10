using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrickBoard : MonoBehaviour, IInteractable
{
    [HideInInspector] public InteractableObjectCanvas iOCanvas;

    public Transform prickManagerTrans;

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
        prickManagerTrans.GetComponent<PrickMinigameManager>().enabled = true;
        prickManagerTrans.GetComponent<PrickMinigameManager>().prickUI.SetActive(true);
        prickManagerTrans.GetComponent<PrickMinigameManager>().prickCamera.enabled = true;

        ThirdPersonController.instance.canMove = false;

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
