using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interacting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FindTargets()
    {
        // WIP: Hier fehlt noch eine for-Schleife ( Siehe anderes Projekt ) + Transform muss durch die Transform des interagierbaren Objektes ersetzt werden.
        if (transform.TryGetComponent(out IInteractable interactable))
        {
            interactable.Interact(transform);
            interactable.iOCanvas().howToInteractTxt.text = interactable.GetInteractUIText();
            interactable.iOCanvas().iOCanvas.SetActive(true);
        }
    }
}
