using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, IInteractable
{
    [HideInInspector] public InteractableObjectCanvas iOCanvas;

    public bool isOpen = false;

    [HideInInspector] public bool canInteract = true;

    public float speed = 1;
    public float rotationAmount = 90f;
    public float forwardDirection = 0;

    public Vector3 startRotation;
    public Vector3 forward;

    public Coroutine animationCoroutine;

    public MissionBaseProfile[] correspondingMissions;
    public MissionTaskBase[] correspondingMissionTasks;

    [Header("Needed Items For Opening")]
    public List<ItemBaseProfile> neededItemsForOpening;

    public bool isLocked = false;

    [Header("Scene Change")]
    public bool isSceneChangeDoor = false;
    public LoadingScreenProfile[] possibleLSProfiles;

    public Vector3 playerSpawnPos;
    public Quaternion playerSpawnRot;

    public void Awake()
    {
        startRotation = transform.rotation.eulerAngles;
        forward = transform.right;

        if (neededItemsForOpening.Count > 0)
        {
            isLocked = true;
        }
    }

    public void Start()
    {
        InstantiateIOCanvas();

        GameManager.instance.allInteractableDoors.Add(this);
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
        if (correspondingMissions.Length > 0 && !isLocked)
        {
            for (int i = 0; i < correspondingMissions.Length; i++)
            {
                if (MissionManager.instance.allCurrAcceptedMissions.Contains(correspondingMissions[i]) && !correspondingMissionTasks[i].missionTaskCompleted && correspondingMissionTasks[i].canBeDisplayed)
                {
                    return "Interagieren";
                }
            }

            return "";
        }
        else
        {
            if (neededItemsForOpening.Count > 0 && isLocked)
            {
                var itemsInInv = 0;

                for (int i = 0; i < InventoryManager.instance.inventory.slots.Count; i++)
                {
                    if (neededItemsForOpening.Contains(InventoryManager.instance.inventory.slots[i].itemBase))
                    {
                        itemsInInv += 1;
                    }
                }

                if (itemsInInv == neededItemsForOpening.Count)
                {
                    return "Aufschlieﬂen";
                }
                else
                {
                    return "Verschlossen";
                }
            }
            else if (isSceneChangeDoor)
            {
                return "Betreten";
            }
            else
            {
                if (isOpen)
                {
                    return "Schlieﬂen";
                }
                else
                {
                    return "÷ffnen";
                }
            }
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
        if (isSceneChangeDoor)
        {
            SceneChangeManager.instance.wentThroughTrigger = true;

            //PlayerValueManager.instance.isDead = LoadingScreen.instance.playerWasDead;

            var randomLSPNumber = Random.Range(0, possibleLSProfiles.Length);
            var randomLSP = possibleLSProfiles[randomLSPNumber];

            if (LoadingScreen.instance != null)
            {
                LoadingScreen.currLSP = randomLSP;
                LoadingScreen.currSpawnPos = playerSpawnPos;
                LoadingScreen.currSpawnRot = playerSpawnRot;

                LoadingScreen.instance.placeNameTxt.text = randomLSP.placeName;
                LoadingScreen.instance.backgroundImg.sprite = randomLSP.backgroundSprite;
                LoadingScreen.instance.descriptionTxt.text = randomLSP.descriptionTextString;

                LoadingScreen.instance.gameObject.SetActive(true);
                LoadingScreen.instance.ActivateAnimator();

                //SceneChangeManager.instance.GetComponent<Animator>().enabled = false;
                SceneChangeManager.instance.GetComponent<Animator>().Rebind();
                //SceneChangeManager.instance.GetComponent<Animator>().enabled = true;

                SaveSystem.instance.SaveAutomatic();

                SceneChangeManager.instance.loadingScreen.SetActive(true);
                SceneChangeManager.instance.gameObject.GetComponent<Animator>().Play("OpenLoadingScreenInStartScreenAnim");
            }
            else
            {
                SceneManager.LoadScene(randomLSP.sceneToLoadIndex);
            }
        }
        else if (correspondingMissions.Length > 0 && !isLocked)
        {
            for (int i = 0; i < correspondingMissions.Length; i++)
            {
                if (MissionManager.instance.allCurrAcceptedMissions.Contains(correspondingMissions[i]) && !correspondingMissionTasks[i].missionTaskCompleted && correspondingMissionTasks[i].canBeDisplayed)
                {
                    if (correspondingMissionTasks[i].completeAfterInteracted)
                    {
                        correspondingMissionTasks[i].missionTaskCompleted = true;
                    }

                    CutsceneManager.instance.DeactivateHUDUI();
                    CutsceneManager.instance.currCP = correspondingMissionTasks[i].cutsceneToTrigger;
                    CutsceneManager.instance.playableDirector.playableAsset = correspondingMissionTasks[i].cutsceneToTrigger.cutscene;
                    CutsceneManager.instance.playableDirector.Play();
                }
            }
        }
        else
        {
            if (neededItemsForOpening.Count > 0 && isLocked)
            {
                if (GetInteractUIText() == "Aufschlieﬂen")
                {
                    for (int i = 0; i < neededItemsForOpening.Count; i++)
                    {
                        InventoryManager.instance.inventory.RemoveItem(neededItemsForOpening[i], 1);
                    }

                    isLocked = false;
                }
            }
            else
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
        }
    }

    InteractableObjectCanvas IInteractable.iOCanvas()
    {
        return this.iOCanvas;
    }
}
