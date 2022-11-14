using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeatingObject : MonoBehaviour, IInteractable
{
    [HideInInspector] public InteractableObjectCanvas iOCanvas;

    public GameObject iOCanvasLookAtSitPlaceObj;

    public bool isFree = true;

    private Coroutine changeRotationCoro = null;

    public float rotationY;

    // Start is called before the first frame update
    void Start()
    {
        InstantiateIOCanvas();
    }

    public IEnumerator ChangePlayerRotation()
    {
        ThirdPersonController.instance._animator.SetBool("SittingDown", true);

        var RotY = rotationY + 180;

        var StartRot = GameManager.instance.playerGO.transform.rotation;

        var TargetRot = new Quaternion(iOCanvasLookAtSitPlaceObj.transform.rotation.x, RotY, this.gameObject.transform.rotation.z, 0);

        //Debug.Log(rotationY + 180);
        //Debug.Log(iOCanvasLookAtSitPlaceObj.transform.rotation.y);

        float time = 0;
        var speed = 1;

        //while (GameManager.instance.playerGO.transform.rotation.y != 270)
        //{
        var rot = GameManager.instance.playerGO.transform.eulerAngles;
        rot.y = rotationY;

        GameManager.instance.playerGO.transform.eulerAngles = rot;

        if (rotationY == 0 || rotationY == 180)
        {
            GameManager.instance.playerGO.transform.position = new Vector3(iOCanvasLookAtSitPlaceObj.transform.position.x, GameManager.instance.playerGO.transform.position.y,
                GameManager.instance.playerGO.transform.position.z);
        }
        else
        {
            GameManager.instance.playerGO.transform.position = new Vector3(GameManager.instance.playerGO.transform.position.x, GameManager.instance.playerGO.transform.position.y,
                iOCanvasLookAtSitPlaceObj.transform.position.z);
        }

        Interacting.instance.currClickedTime = 0;

        Interacting.instance.keyToPressFillImg.fillAmount = 0;

        //time = time + Time.deltaTime;

        yield return null;
        //}

        //ThirdPersonController.instance._animator.SetBool("SittingDown", true);
        Debug.Log("DONE");
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
        return "Hinsetzen";
    }

    public float GetTimeTillInteract()
    {
        return 0.5f;
    }

    public void Interact(Transform transform)
    {
        if (isFree)
        {
            isFree = false;

            ThirdPersonController.instance.characterController.enabled = false;
            ThirdPersonController.instance.currSeatTrans = this.transform;

            changeRotationCoro = StartCoroutine(ChangePlayerRotation());

            //GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, false);

            ThirdPersonController.instance._animator.SetFloat("Speed", 0);

            for (int i = 0; i < MessageManager.instance.collectedMessageParentObj.transform.childCount; i++)
            {
                Destroy(MessageManager.instance.collectedMessageParentObj.transform.GetChild(i).gameObject);
            }
        }
        else if (!isFree)
        {
            StopCoroutine(changeRotationCoro);

            Debug.Log("HERE2");

            isFree = true;

            ThirdPersonController.instance._animator.SetBool("StartSitting", false);
            ThirdPersonController.instance._animator.SetBool("SittingDown", false);

            ThirdPersonController.instance.characterController.enabled = true;
            ThirdPersonController.instance.currSeatTrans = null;

            ThirdPersonController.instance._animator.SetFloat("Speed", 0);
        }
    }

    InteractableObjectCanvas IInteractable.iOCanvas()
    {
        return this.iOCanvas;
    }
}
