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

    [Range(0, 360)] public float viewAngle;
    [Range(0, 360)] public float viewAngle2;
    [Range(0, 360)] public float viewAngle3; // Only for sitting

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

    [Header("Animation Rigging")]
    public Transform rightHandRigTargetTrans;
    public TwoBoneIKConstraint rightHandParentRig;

    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FindTargets();
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
                        howToInteractGO.SetActive(false);
                        interactable.iOCanvas().iOBillboardParentObj.SetActive(false);

                        tIVR.Remove(tIVR[i]);
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

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform interactableObj = targetsInViewRadius[i].transform;

            Vector3 dirToObj = (interactableObj.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToObj) < viewAngle / 2)
            {
                float distanceToObj = Vector3.Distance(transform.position, interactableObj.position);

                if (!Physics.Raycast(transform.position, dirToObj, distanceToObj, obstacleMask))
                {
                    if (interactableObj.TryGetComponent(out IInteractable interactable) && !ShopManager.instance.shopScreen.activeSelf
                        && !GuessTheCardMinigameManager.instance.gTCUI.activeSelf && !PrickMinigameManager.instance.prickUI.activeSelf && ThirdPersonController.instance.currSeatTrans == null
                        && !Blackboard.instance.blackboardCam.enabled)
                    {
                        interactable.iOCanvas().iOBillboardParentObj.SetActive(true);
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

        if (ThirdPersonController.instance.currSeatTrans == null)
        {
            if (nearestObjTrans != null)
            {
                if (nearestObjTrans.TryGetComponent(out IInteractable interactable) && !ThirdPersonController.instance._animator.GetBool("Jump"))
                {
                    if (nearestObjTrans.TryGetComponent(out SeatingObject seatObj) && Vector3.Distance(seatObj.iOCanvasLookAtSitPlaceObj.transform.position, this.gameObject.transform.position) < 0.7f)
                    {
                        if (!ShopManager.instance.shopScreen.activeSelf && !GuessTheCardMinigameManager.instance.gTCUI.activeSelf && !PrickMinigameManager.instance.prickUI.activeSelf
                            && !Blackboard.instance.blackboardCam.enabled)
                        {
                            howToInteractGO.SetActive(true);

                            howToInteractTxt.text = interactable.GetInteractUIText();

                            timeTillInteract = interactable.GetTimeTillInteract();
                        }
                    }
                    else if (!nearestObjTrans.TryGetComponent(out SeatingObject seatObj2))
                    {
                        if (!ShopManager.instance.shopScreen.activeSelf && !GuessTheCardMinigameManager.instance.gTCUI.activeSelf && !PrickMinigameManager.instance.prickUI.activeSelf
                            && !Blackboard.instance.blackboardCam.enabled)
                        {
                            howToInteractGO.SetActive(true);

                            howToInteractTxt.text = interactable.GetInteractUIText();

                            timeTillInteract = interactable.GetTimeTillInteract();
                        }
                    }

                    if (howToInteractGO.activeSelf)
                    {
                        if (timeTillInteract > 0)
                        {
                            if (Input.GetKeyDown(KeyCode.E) && nearestObjTrans.GetComponent<Item>() != null)
                            {
                                rightHandRigTargetTrans.position = nearestObjTrans.position;
                                rightHandParentRig.weight = 0;

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
                                }
                            }

                            if (Input.GetKeyUp(KeyCode.E))
                            {
                                currClickedTime = 0;

                                keyToPressFillImg.fillAmount = 0;

                                if (nearestObjTrans.GetComponent<Item>() != null)
                                {
                                    //rightHandRigTargetTrans.position = nearestObjTrans.position;

                                    GameManager.instance.playerGO.GetComponent<ThirdPersonController>()._animator.SetBool("GrabItem", false);

                                    rightHandParentRig.weight = 0;
                                }
                            }
                        }
                        else
                        {
                            if (Input.GetKeyDown(KeyCode.E))
                            {
                                interactable.Interact(nearestObjTrans);

                                currClickedTime = 0;

                                keyToPressFillImg.fillAmount = 0;

                                rightHandParentRig.weight = 0;
                            }
                        }
                    }
                }
                else if (nearestObjTrans.TryGetComponent(out Enemy enemy))
                {
                    Debug.Log("ENEMY");

                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        FightManager.instance.TargetEnemy(nearestObjTrans.gameObject);
                    }
                }
            }
        }
        else
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

    public Vector3 DirFromAngles(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
