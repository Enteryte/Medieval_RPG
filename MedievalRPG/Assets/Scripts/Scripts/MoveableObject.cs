using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class MoveableObject : MonoBehaviour, IInteractable
{
    [HideInInspector] public InteractableObjectCanvas iOCanvas;

    public float maxPosX;
    public float minPosX;

    public bool moveObject = false;

    public TimelineAsset startMovingTL;
    public TimelineAsset stopMovingTL;

    public Camera mOCamera;
    public Camera normalCam;

    // Start is called before the first frame update
    void Start()
    {
        InstantiateIOCanvas();
    }

    // Update is called once per frame
    void Update()
    {
        if (moveObject)
        {
            MoveObject();
        }
    }

    public void MoveObject()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMoveObjectView();
        }

        if (Input.GetKey(KeyCode.W) && this.gameObject.transform.position.x < maxPosX)
        {
            if (!ThirdPersonController.instance._animator.GetBool("DoPush"))
            {
                ThirdPersonController.instance._animator.SetBool("DoPush", true);
            }

            var newPos = this.gameObject.transform.position;
            newPos.x += 0.025f;

            this.gameObject.transform.position = newPos;

            if (this.gameObject.transform.position.x >= maxPosX)
            {
                if (ThirdPersonController.instance._animator.GetBool("DoPush"))
                {
                    ThirdPersonController.instance._animator.SetBool("DoPush", false);

                    //CloseMoveObjectView();

                    this.gameObject.layer = LayerMask.NameToLayer("Default");

                    Destroy(iOCanvas.gameObject);
                }
            }
        }
        else if (!Input.anyKey)
        {
            if (ThirdPersonController.instance._animator.GetBool("DoPush"))
            {
                ThirdPersonController.instance._animator.SetBool("DoPush", false);
            }
        }
    }

    public void SetPlayerParentToMoveableObject()
    {
        CutsceneManager.instance.playableDirector.playableAsset = startMovingTL;
        CutsceneManager.instance.playableDirector.Play();

        GameManager.instance.playerGO.transform.parent = this.gameObject.transform;

        moveObject = true;

        GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, false);

        ThirdPersonController.instance.canMove = false;
        ThirdPersonController.instance._animator.SetFloat("Speed", 0);

        ThirdPersonController.instance._animator.Play("Push_Idle");
    }

    public void CloseMoveObjectView()
    {
        CutsceneManager.instance.playableDirector.playableAsset = stopMovingTL;
        CutsceneManager.instance.playableDirector.Play();

        CutsceneManager.instance.ChangePlayerParentToNull();

        GameManager.instance.FreezeCameraAndSetMouseVisibility(ThirdPersonController.instance, ThirdPersonController.instance._input, true);

        ThirdPersonController.instance.canMove = true;
        ThirdPersonController.instance._animator.SetFloat("Speed", 1);

        //normalCam.gameObject.SetActive(true);
        //mOCamera.gameObject.SetActive(false);

        ThirdPersonController.instance._animator.SetBool("DoPush", false);

        ThirdPersonController.instance._animator.enabled = false;
        ThirdPersonController.instance._animator.Rebind();
        ThirdPersonController.instance._animator.enabled = true;

        FightingActions.instance.GetWeapon();
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
        return "Bewegen";
    }

    public float GetTimeTillInteract()
    {
        return 0;
    }

    public void Interact(Transform transform)
    {
        SetPlayerParentToMoveableObject();
    }

    InteractableObjectCanvas IInteractable.iOCanvas()
    {
        return this.iOCanvas;
    }
}
