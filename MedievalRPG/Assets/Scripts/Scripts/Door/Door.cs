using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [HideInInspector] public InteractableObjectCanvas iOCanvas;

    public bool isOpen = false;

    public float speed = 1;
    public float rotationAmount = 90f;
    public float forwardDirection = 0;

    public Vector3 startRotation;
    public Vector3 forward;

    public Coroutine animationCoroutine;

    public void Awake()
    {
        startRotation = transform.rotation.eulerAngles;
        forward = transform.right;
    }

    public void Start()
    {
        InstantiateIOCanvas();
    }

    public void OpenDoor(Vector3 playerPos)
    {
        if (!isOpen)
        {
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
            }

            float dot = Vector3.Dot(forward, (playerPos - transform.position).normalized);

            animationCoroutine = StartCoroutine(DoRotationOpen(dot));
        }
    }

    public IEnumerator DoRotationOpen(float forwardAmount)
    {
        Quaternion startRotationOnOpen = transform.rotation;
        Quaternion endRotation;

        if (forwardAmount >= forwardDirection)
        {
            endRotation = Quaternion.Euler(new Vector3(0, startRotation.y + rotationAmount, 0));
        }
        else
        {
            endRotation = Quaternion.Euler(new Vector3(0, startRotation.y - rotationAmount, 0));
        }

        isOpen = true;

        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotationOnOpen, endRotation, time);

            yield return null;

            time += Time.deltaTime * speed;
        }
    }

    public void CloseDoor()
    {
        if (isOpen)
        {
            if (animationCoroutine != null)
            {
                StopCoroutine(animationCoroutine);
            }

            animationCoroutine = StartCoroutine(DoRotationClose());
        }
    }

    public IEnumerator DoRotationClose()
    {
        Quaternion startRotationOnClose = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(startRotation);

        isOpen = false;

        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotationOnClose, endRotation, time);

            yield return null;

            time += Time.deltaTime * speed;
        }
    }

    public string GetInteractUIText()
    {
        if (isOpen)
        {
            return "Schließen";
        }
        else
        {
            return "Öffnen";
        }
    }

    public float GetTimeTillInteract()
    {
        return 0;
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
        if (isOpen)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor(GameManager.instance.playerGO.transform.position);
        }
    }

    InteractableObjectCanvas IInteractable.iOCanvas()
    {
        return this.iOCanvas;
    }
}
