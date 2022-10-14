using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour, IInteractable
{
    public MerchantBaseProfile mBP;
    public InteractableObjectCanvas iOCanvas;

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
        return "Sprechen";
    }

    public void Interact(Transform transform)
    {
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

        ShopManager.instance.DisplayShopItems();

        ShopManager.instance.shopScreen.SetActive(true);
    }

    InteractableObjectCanvas IInteractable.iOCanvas()
    {
        return this.iOCanvas;
    }
}
