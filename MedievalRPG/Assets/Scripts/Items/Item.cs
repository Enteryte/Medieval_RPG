using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IInteractable
{
    public ItemBaseProfile iBP;
    public InteractableObjectCanvas iOCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetInteractUIText()
    {
        return "Einsammeln";
    }

    public void Interact(Transform transform)
    {
        Debug.Log("INTERACTING");
    }

    InteractableObjectCanvas IInteractable.iOCanvas()
    {
        return this.iOCanvas;
    }
}
