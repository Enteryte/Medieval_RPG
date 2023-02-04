using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class Interacting : MonoBehaviour
{
    public static Interacting instance;

    public float viewRadius;
    public float viewRadius2;
    public float viewRadius3; // Only for sitting
    public float viewRadius4; // Only for merchants ( far away merchants )

    [Range(0, 360)] public float viewAngle;
    [Range(0, 360)] public float viewAngle2;
    [Range(0, 360)] public float viewAngle3; // Only for sitting
    [Range(0, 360)] public float viewAngle4; // Only for sitting

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector] public float nearestDistance;
    [HideInInspector] public Transform nearestObjTrans;

    [HideInInspector] public List<Transform> tIVR;

    public static float timeTillInteract = 0;
    public float currClickedTime = 0;

    public GameObject interactCanvasPrefab;
    public GameObject iOCSParentObj;

    public GameObject howToInteractGO;
    public TMP_Text howToInteractTxt;
    public Image keyToPressFillImg;

    public AnimationClip grabItemAnim;

    public Transform currInteractedObjTrans;

    [Header("Animation Rigging")]
    public Transform rightHandRigTargetTrans;
    public TwoBoneIKConstraint rightHandParentRig;
    public MultiAimConstraint headRig;
    public GameObject playerHeadObj;
    public GameObject playerHandObj;

    [Header("Tutorial")]
    public TutorialBaseProfile interactingTutorial;
    public TutorialBaseProfile selectingAndParryTutorial;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(CutsceneManager.instance.currCP);
        //Debug.Log(CutsceneManager.instance.currCP.cantBeSkipped);
        //Debug.Log(CutsceneManager.instance.playableDirector.playableGraph.IsPlaying());

        FindTargets();

        //Debug.Log(Vector3.Distance(playerHeadObj.transform.position, playerHandObj.transform.position));

        if (ThirdPersonController.instance._animator.GetBool("GrabItem") == true && ThirdPersonController.instance._animator.GetLayerWeight(1) < 1
            && Vector3.Distance(playerHeadObj.transform.position, playerHandObj.transform.position) > 1)
        { 
            //if (nearestObjTrans.GetComponent<Item>().whereToGrabItemTrans != null && GameManager.instance.playerGO.transform.position.y - nearestObjTrans.GetComponent<Item>().whereToGrabItemTrans.position.y < 0)
            //{
            //    ThirdPersonController.instance._animator.SetLayerWeight(1, ThirdPersonController.instance._animator.GetLayerWeight(1) + Time.deltaTime);
            //}
            //else if (nearestObjTrans.GetComponent<Item>().whereToGrabItemTrans == null && GameManager.instance.playerGO.transform.position.y - nearestObjTrans.position.y < 0)
            //{
                ThirdPersonController.instance._animator.SetLayerWeight(1, ThirdPersonController.instance._animator.GetLayerWeight(1) + Time.deltaTime);
            //}

            //Debug.Log(GameManager.instance.playerGO.transform.position.y);
            //Debug.Log(nearestObjTrans.position.y);
            //Debug.Log(GameManager.instance.playerGO.transform.position.y - nearestObjTrans.position.y);
        }
        else if (ThirdPersonController.instance._animator.GetBool("UsingHBItem") == true && ThirdPersonController.instance._animator.GetLayerWeight(1) < 0.75f)
        {
            //if (nearestObjTrans.GetComponent<Item>().whereToGrabItemTrans != null && GameManager.instance.playerGO.transform.position.y - nearestObjTrans.GetComponent<Item>().whereToGrabItemTrans.position.y < 0)
            //{
            //    ThirdPersonController.instance._animator.SetLayerWeight(1, ThirdPersonController.instance._animator.GetLayerWeight(1) + Time.deltaTime);
            //}
            //else if (nearestObjTrans.GetComponent<Item>().whereToGrabItemTrans == null && GameManager.instance.playerGO.transform.position.y - nearestObjTrans.position.y < 0)
            //{
            ThirdPersonController.instance._animator.SetLayerWeight(1, ThirdPersonController.instance._animator.GetLayerWeight(1) + Time.deltaTime);
            //}

            //Debug.Log(GameManager.instance.playerGO.transform.position.y);
            //Debug.Log(nearestObjTrans.position.y);
            //Debug.Log(GameManager.instance.playerGO.transform.position.y - nearestObjTrans.position.y);
        }
    }

    public void FindTargets()
    {
        if (tIVR.Count > 0)
        {
            for (int i = 0; i < tIVR.Count; i++)
            {
                if (tIVR[i] != null)
                {
                    if (tIVR[i].TryGetComponent(out IInteractable interactable))
                    {
                        if (interactable.iOCanvas() != null)
                        {
                            howToInteractGO.SetActive(false);
                            interactable.iOCanvas().iOBillboardParentObj.SetActive(false);

                            tIVR.Remove(tIVR[i]);
                        }
                    }
                }
                else
                {
                    currClickedTime = 0;

                    keyToPressFillImg.fillAmount = 0;

                    howToInteractGO.SetActive(false);

                    tIVR.Remove(tIVR[i]);
                }
            }
        }

        nearestObjTrans = null;

        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        Collider[] targetsInViewRadius2 = Physics.OverlapSphere(transform.position, viewRadius2, targetMask);
        Collider[] targetsInViewRadius3 = Physics.OverlapSphere(transform.position, viewRadius3, targetMask);
        Collider[] targetsInViewRadius4 = Physics.OverlapSphere(transform.position, viewRadius4, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform interactableObj = targetsInViewRadius[i].transform;

            Vector3 dirToObj = (interactableObj.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToObj) < viewAngle / 2)
            {
                float distanceToObj = Vector3.Distance(transform.position, interactableObj.position);

                if (!Physics.Raycast(transform.position, dirToObj, distanceToObj, obstacleMask))
                {
                    if (interactableObj != null && interactableObj.TryGetComponent(out IInteractable interactable) && !ShopManager.instance.shopScreen.activeSelf
                        /*&& !GuessTheCardMinigameManager.instance.gTCUI.activeSelf*//* && !PrickMinigameManager.instance.prickUI.activeSelf*/ && ThirdPersonController.instance.currSeatTrans == null
                        /*&& !Blackboard.instance.blackboardCam.enabled*/ && ThirdPersonController.instance.canMove && interactable.iOCanvas() != null && !GameManager.instance.gameIsPaused)
                    {
                        if (interactableObj.TryGetComponent(out Door door) && door.neededItemsForOpening.Count > 0 && !door.isLocked)
                        {
                            door.canInteract = false;

                            for (int x = 0; x < door.correspondingMissions.Length; x++)
                            {
                                if (MissionManager.instance.allCurrAcceptedMissions.Contains(door.correspondingMissions[x])
                                    && !door.correspondingMissionTasks[x].missionTaskCompleted && door.correspondingMissionTasks[x].canBeDisplayed)
                                {
                                    interactable.iOCanvas().iOBillboardParentObj.SetActive(true);

                                    door.canInteract = true;

                                    if (GameManager.instance.playtimeInSeconds > 5)
                                    {
                                        TutorialManager.instance.CheckIfTutorialIsAlreadyCompleted(interactingTutorial);
                                    }
                                }
                            }
                        }
                        else if (interactableObj.TryGetComponent(out Door door2) && door2.neededItemsForOpening.Count <= 0 && door2.correspondingMissions.Length > 0)
                        {
                            door.canInteract = false;

                            for (int x = 0; x < door.correspondingMissions.Length; x++)
                            {
                                if (MissionManager.instance.allCurrAcceptedMissions.Contains(door.correspondingMissions[x])
                                    && !door.correspondingMissionTasks[x].missionTaskCompleted && door.correspondingMissionTasks[x].canBeDisplayed)
                                {
                                    interactable.iOCanvas().iOBillboardParentObj.SetActive(true);

                                    door.canInteract = true;

                                    if (GameManager.instance.playtimeInSeconds > 5)
                                    {
                                        TutorialManager.instance.CheckIfTutorialIsAlreadyCompleted(interactingTutorial);
                                    }
                                }
                            }
                        }
                        else
                        {
                            interactable.iOCanvas().iOBillboardParentObj.SetActive(true);

                            if (interactableObj.TryGetComponent(out Item item) && item.isOpen)
                            {
                                interactable.iOCanvas().iOBillboardParentObj.SetActive(false);
                            }
                            
                            if (interactableObj.TryGetComponent(out NPC npc))
                            {
                                if (npc.choosenOL == null)
                                {
                                    npc.CheckIfNeededForMission(false);

                                    if (!npc.isNeeded)
                                    {
                                        interactable.iOCanvas().iOBillboardParentObj.SetActive(false);
                                    }
                                }
                            }

                            if (GameManager.instance.playtimeInSeconds > 5)
                            {
                                TutorialManager.instance.CheckIfTutorialIsAlreadyCompleted(interactingTutorial);
                            }
                        }
                    }

                    if (interactableObj != null && interactableObj.GetComponent<Enemy>())
                    {
                        //Debug.Log(interactableObj.gameObject.name);

                        if (FightingActions.instance.equippedWeaponL != null
                        && FightingActions.instance.equippedWeaponL.gameObject.GetComponent<Item>().iBP.weaponType != ItemBaseProfile.WeaponType.bow)
                        {
                            TutorialManager.instance.CheckIfTutorialIsAlreadyCompleted(selectingAndParryTutorial);

                            if (Input.GetKeyDown(KeyCode.Q))
                            {
                                FightManager.instance.TargetEnemy(interactableObj.gameObject);
                            }
                        }
                        else if (FightingActions.instance.equippedWeaponL == null)
                        {
                            TutorialManager.instance.CheckIfTutorialIsAlreadyCompleted(selectingAndParryTutorial);

                            if (Input.GetKeyDown(KeyCode.Q))
                            {
                                FightManager.instance.TargetEnemy(interactableObj.gameObject);
                            }
                        }
                    }
                }
            }

            if (!tIVR.Contains(interactableObj))
            {
                tIVR.Add(interactableObj);
            }
        }

        for (int i = 0; i < targetsInViewRadius2.Length; i++)
        {
            Transform interactableObj = targetsInViewRadius2[i].transform;

            Vector3 dirToObj = (interactableObj.position - transform.position).normalized;
            float distanceToObj = Vector3.Distance(transform.position, interactableObj.position);

            if (Vector3.Angle(transform.forward, dirToObj) < viewAngle2 / 2 && !Physics.Raycast(transform.position, dirToObj, distanceToObj, obstacleMask))
            {
                //if (!Input.anyKey)
                //{
                if (interactableObj.TryGetComponent(out IInteractable interactable))
                {
                    if (interactable.iOCanvas() != null)
                    {
                        if (interactableObj.TryGetComponent(out NPC npc))
                        {
                            if (npc.choosenOL == null)
                            {
                                npc.CheckIfNeededForMission(false);

                                if (!npc.isNeeded)
                                {
                                    return;
                                }
                            }
                        }

                        if (Vector3.Distance(interactableObj.position, transform.position) > nearestDistance || nearestObjTrans == null)
                        {
                            nearestDistance = Vector3.Distance(interactableObj.position, transform.position);
                            nearestObjTrans = interactableObj;
                        }

                        if (!tIVR.Contains(interactableObj))
                        {
                            tIVR.Add(interactableObj);
                        }
                    }
                }

                //}
            }
        }

        for (int i = 0; i < targetsInViewRadius4.Length; i++)
        {
            Transform interactableObj = targetsInViewRadius4[i].transform;

            Vector3 dirToObj = (interactableObj.position - transform.position).normalized;
            float distanceToObj = Vector3.Distance(transform.position, interactableObj.position);

            if (Vector3.Angle(transform.forward, dirToObj) < viewAngle4 / 2 && !Physics.Raycast(transform.position, dirToObj, distanceToObj, obstacleMask))
            {
                if (interactableObj.TryGetComponent(out IInteractable interactable) && interactableObj.TryGetComponent(out Merchant merchant))
                {
                    if (interactable.iOCanvas() != null && merchant.needsBiggerViewRadius)
                    {
                        if (Vector3.Distance(interactableObj.position, transform.position) > nearestDistance || nearestObjTrans == null)
                        {
                            nearestDistance = Vector3.Distance(interactableObj.position, transform.position);
                            nearestObjTrans = interactableObj;
                        }

                        if (!tIVR.Contains(interactableObj))
                        {
                            tIVR.Add(interactableObj);
                        }
                    }
                }
            }
        }

        if (ThirdPersonController.instance.currSeatTrans == null)
        {
            if (nearestObjTrans != null)
            {
                if (nearestObjTrans.TryGetComponent(out IInteractable interactable) && !ThirdPersonController.instance._animator.GetBool("Jump"))
                {
                    if (nearestObjTrans.TryGetComponent(out SeatingObject seatObj) && Vector3.Distance(seatObj.iOCanvasLookAtSitPlaceObj.transform.position, this.gameObject.transform.position) < 0.7f)
                    {
                        if (!ShopManager.instance.shopScreen.activeSelf && !GuessTheCardMinigameManager.instance.gTCUI.activeSelf && !PrickMinigameManager.instance.prickUI.activeSelf
                            && !Blackboard.instance.blackboardUI.activeSelf && ThirdPersonController.instance.canMove)
                        {
                            howToInteractGO.SetActive(true);

                            howToInteractTxt.text = interactable.GetInteractUIText();

                            timeTillInteract = interactable.GetTimeTillInteract();
                        }
                    }
                    else if (!nearestObjTrans.TryGetComponent(out SeatingObject seatObj2))
                    {
                        if (nearestObjTrans.TryGetComponent(out Door door) && door.neededItemsForOpening.Count > 0 && !door.isLocked)
                        {
                            if (door.canInteract)
                            {
                                if (!ShopManager.instance.shopScreen.activeSelf && !GuessTheCardMinigameManager.instance.gTCUI.activeSelf && !PrickMinigameManager.instance.prickUI.activeSelf
                                && !Blackboard.instance.blackboardUI.activeSelf && ThirdPersonController.instance.canMove)
                                {
                                    if (interactable.iOCanvas() != null)
                                    {
                                        howToInteractGO.SetActive(true);

                                        howToInteractTxt.text = interactable.GetInteractUIText();

                                        timeTillInteract = interactable.GetTimeTillInteract();
                                    }
                                }
                            }
                        }
                        else if (nearestObjTrans.TryGetComponent(out Door door2) && door2.neededItemsForOpening.Count <= 0)
                        {
                            if (door2.canInteract)
                            {
                                if (!ShopManager.instance.shopScreen.activeSelf && !GuessTheCardMinigameManager.instance.gTCUI.activeSelf && !PrickMinigameManager.instance.prickUI.activeSelf
                                && !Blackboard.instance.blackboardUI.activeSelf && ThirdPersonController.instance.canMove)
                                {
                                    if (interactable.iOCanvas() != null)
                                    {
                                        howToInteractGO.SetActive(true);

                                        howToInteractTxt.text = interactable.GetInteractUIText();

                                        timeTillInteract = interactable.GetTimeTillInteract();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!ShopManager.instance.shopScreen.activeSelf /*&&*/ /*!GuessTheCardMinigameManager.instance.gTCUI.activeSelf && !PrickMinigameManager.instance.prickUI.activeSelf*/
                               /* && !Blackboard.instance.blackboardCam.enabled*/ && ThirdPersonController.instance.canMove && !GameManager.instance.gameIsPaused)
                            {
                                if (nearestObjTrans.TryGetComponent(out Item item) && item.itemsToGet.Length > 0 || nearestObjTrans.TryGetComponent(out Item item2) && item2.moneyAmount > 0)
                                {
                                    if (!item.isOpen)
                                    {
                                        if (interactable.iOCanvas() != null)
                                        {
                                            howToInteractGO.SetActive(true);

                                            howToInteractTxt.text = interactable.GetInteractUIText();

                                            timeTillInteract = interactable.GetTimeTillInteract();
                                        }
                                    }
                                    else
                                    {
                                        interactable.iOCanvas().iOBillboardParentObj.SetActive(false);
                                    }
                                }
                                else
                                {
                                    if (interactable.iOCanvas() != null)
                                    {
                                        howToInteractGO.SetActive(true);

                                        howToInteractTxt.text = interactable.GetInteractUIText();

                                        timeTillInteract = interactable.GetTimeTillInteract();
                                    }
                                }
                            }
                        }
                    }

                    if (nearestObjTrans.TryGetComponent(out NPC npc))
                    {
                        if (npc.nPCAudioSource != null && npc.nPCAudioSource.isPlaying && npc.GetComponent<NPCScreamingHandler>() == null)
                        {
                            howToInteractGO.SetActive(false);
                        }
                    }

                    if (howToInteractGO.activeSelf)
                    {
                        if (timeTillInteract > 0)
                        {
                            if (CutsceneManager.instance.currCP != null && CutsceneManager.instance.currCP.cantBeSkipped && CutsceneManager.instance.playableDirector.playableGraph.IsValid()
                                    && CutsceneManager.instance.playableDirector.playableGraph.IsPlaying())
                            {
                                howToInteractGO.GetComponentInChildren<Image>().color = Color.gray;
                                howToInteractTxt.color = Color.gray;

                                return;
                            }
                            else
                            {
                                howToInteractGO.GetComponentInChildren<Image>().color = Color.white;
                                howToInteractTxt.color = Color.white;
                            }

                            if (Input.GetKeyDown(KeyCode.E) && nearestObjTrans.GetComponent<Item>() != null && !nearestObjTrans.GetComponent<Item>().onlyExamineObject
                                /* || nearestObjTrans.GetComponent<Enemy>() != null && nearestObjTrans.GetComponent<Enemy>().isDead*/)
                            {
                                if (/*nearestObjTrans.GetComponent<Item>() != null && */nearestObjTrans.GetComponent<Item>().whereToGrabItemTrans != null)
                                {
                                    rightHandRigTargetTrans.position = nearestObjTrans.GetComponent<Item>().whereToGrabItemTrans.position;
                                }
                                else
                                {
                                    rightHandRigTargetTrans.position = nearestObjTrans.position;
                                }

                                rightHandParentRig.weight = 0;
                                headRig.weight = 0;

                                ThirdPersonController.instance._animator.SetLayerWeight(1, 0);

                                GameManager.instance.playerGO.GetComponent<ThirdPersonController>()._animator.SetBool("GrabItem", true);
                            }

                            if (Input.GetKey(KeyCode.E) && currClickedTime < timeTillInteract)
                            {
                                currClickedTime += Time.deltaTime;
                                keyToPressFillImg.fillAmount += 1f / timeTillInteract * Time.deltaTime;

                                if (currClickedTime >= timeTillInteract && ThirdPersonController.instance.currSeatTrans == null)
                                {
                                    interactable.Interact(nearestObjTrans);

                                    Interacting.instance.rightHandParentRig.weight = 0;
                                    headRig.weight = 0;

                                    ThirdPersonController.instance._animator.SetLayerWeight(1, 0);

                                    keyToPressFillImg.fillAmount = 0;

                                    currClickedTime = 0;
                                }
                            }

                            if (Input.GetKeyUp(KeyCode.E))
                            {
                                currClickedTime = 0;

                                keyToPressFillImg.fillAmount = 0;

                                if (nearestObjTrans.GetComponent<Item>() != null && !nearestObjTrans.GetComponent<Item>().onlyExamineObject/* || nearestObjTrans.GetComponent<Enemy>() != null && nearestObjTrans.GetComponent<Enemy>().isDead*/)
                                {
                                    //rightHandRigTargetTrans.position = nearestObjTrans.position;

                                    GameManager.instance.playerGO.GetComponent<ThirdPersonController>()._animator.SetBool("GrabItem", false);

                                    rightHandParentRig.weight = 0;
                                    headRig.weight = 0;

                                    ThirdPersonController.instance._animator.SetLayerWeight(1, 0);
                                }
                            }
                        }
                        else
                        {
                            if (CutsceneManager.instance.currCP != null && CutsceneManager.instance.currCP.cantBeSkipped && CutsceneManager.instance.playableDirector.playableGraph.IsValid()
                                && CutsceneManager.instance.playableDirector.playableGraph.IsPlaying())
                            {
                                howToInteractGO.GetComponentInChildren<Image>().color = Color.gray;
                                howToInteractTxt.color = Color.gray;

                                return;
                            }
                            else
                            {
                                howToInteractGO.GetComponentInChildren<Image>().color = Color.white;
                                howToInteractTxt.color = Color.white;
                            }

                            if (Input.GetKeyDown(KeyCode.E) && interactable != null && nearestObjTrans != null)
                            {
                                Debug.Log(interactable);
                                Debug.Log(nearestObjTrans);

                                interactable.Interact(nearestObjTrans);

                                currClickedTime = 0;

                                keyToPressFillImg.fillAmount = 0;

                                rightHandParentRig.weight = 0;
                                headRig.weight = 0;

                                ThirdPersonController.instance._animator.SetLayerWeight(1, 0);
                            }
                        }
                    }
                }
                //else if (nearestObjTrans.TryGetComponent(out Enemy enemy))
                //{
                //    Debug.Log("ENEMYYYYYYYYYYYYYYYYYY");

                //    if (!enemy.isDead)
                //    {
                //        Debug.Log("ENEMY");

                //        if (Input.GetKeyDown(KeyCode.Q))
                //        {
                //            // ---------------------------------------------- WIP: Hier Target-Enemy einfügen!
                //            FightManager.instance.TargetEnemy(nearestObjTrans.gameObject);
                //        }
                //    }
                //}

                if (rightHandParentRig.weight != 0)
                {
                    rightHandParentRig.weight = 0;
                    headRig.weight = 0;
                }
            }
        }
        else
        {
            var currAnimInfo = ThirdPersonController.instance._animator.GetCurrentAnimatorClipInfo(0);

            if (currAnimInfo[0].clip.name == "Sitting Idle")
            {
                howToInteractGO.SetActive(true);

                howToInteractTxt.text = "Aufstehen";

                timeTillInteract = ThirdPersonController.instance.currSeatTrans.GetComponent<SeatingObject>().GetTimeTillInteract();

                if (timeTillInteract > 0)
                {
                    if (Input.GetKey(KeyCode.E) && currClickedTime < timeTillInteract)
                    {
                        currClickedTime += Time.deltaTime;
                        keyToPressFillImg.fillAmount += 1f / timeTillInteract * Time.deltaTime;

                        if (currClickedTime >= timeTillInteract)
                        {
                            ThirdPersonController.instance.currSeatTrans.GetComponent<SeatingObject>().Interact(ThirdPersonController.instance.currSeatTrans);

                            currClickedTime = 0;

                            keyToPressFillImg.fillAmount = 0;
                        }
                    }

                    if (Input.GetKeyUp(KeyCode.E))
                    {
                        currClickedTime = 0;

                        keyToPressFillImg.fillAmount = 0;
                    }
                }
                else
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        ThirdPersonController.instance.currSeatTrans.GetComponent<SeatingObject>().Interact(ThirdPersonController.instance.currSeatTrans);

                        currClickedTime = 0;

                        keyToPressFillImg.fillAmount = 0;
                    }
                }
            }
        }

        if (rightHandParentRig.weight != 0)
        {
            rightHandParentRig.weight = 0;
            headRig.weight = 0;
        }
    }          

    public Vector3 DirFromAngles(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
