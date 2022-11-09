using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void InstantiateIOCanvas();

    void Interact(Transform transform);

    string GetInteractUIText();

    float GetTimeTillInteract();

    InteractableObjectCanvas iOCanvas();
}
