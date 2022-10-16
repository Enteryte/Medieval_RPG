using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    public ItemBaseProfile iBP;

    public InteractableObjectCanvas iOCanvas;

    public int amountToGet = 1;

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
        return 1.5f;
    }

    public void Interact(Transform transform)
    {
        InventoryManager.instance.inventory.AddItem(iBP, amountToGet);

        MessageManager.instance.CreateCollectedMessage(iBP);

        Destroy(iOCanvas.gameObject);
        Destroy(this.gameObject);
    }

    InteractableObjectCanvas IInteractable.iOCanvas()
    {
        return this.iOCanvas;
    }
}
