using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombstoneWithText : MonoBehaviour, IInteractable
{
    [HideInInspector] public InteractableObjectCanvas iOCanvas;

    [TextArea(0, 50)] public string textToDisplay;

    // Start is called before the first frame update
    void Start()
    {
        InstantiateIOCanvas();
    }

    // Update is called once per frame
    void Update()
    {
        // Tombstone
        if (Input.GetKeyDown(KeyCode.Escape) && UIManager.instance.readTombstoneUI.activeSelf && UIManager.instance.tombstoneTextToDisplayTxt.text == textToDisplay)
        {
            UIManager.instance.readTombstoneUI.SetActive(false);
            GameManager.instance.gameIsPaused = false;

            GameManager.instance.ContinueGame();
            GameManager.instance.cantPauseRN = false;
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
        return "Lesen";
    }

    public float GetTimeTillInteract()
    {
        return 0;
    }

    public void Interact(Transform transform)
    {
        UIManager.instance.readTombstoneUI.SetActive(true);

        UIManager.instance.tombstoneTextToDisplayTxt.text = textToDisplay;

        GameManager.instance.gameIsPaused = true;

        GameManager.instance.cantPauseRN = true;

        GameManager.instance.PauseGame();
    }

    InteractableObjectCanvas IInteractable.iOCanvas()
    {
        return this.iOCanvas;
    }
}
