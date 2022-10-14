using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Interacting : MonoBehaviour
{
    public static Interacting instance;

    public float viewRadius;
    public float viewRadius2;

    [Range(0, 360)] public float viewAngle;
    [Range(0, 360)] public float viewAngle2;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector] public float nearestDistance;
    [HideInInspector] public Transform nearestObjTrans;

    [HideInInspector] public List<Transform> tIVR;

    public float timeTillInteract = 5;
    public float currClickedTime = 0;

    public GameObject interactCanvasPrefab;
    public GameObject iOCSParentObj;

    public GameObject howToInteractGO;
    public TMP_Text howToInteractTxt;
    public Image keyToPressFillImg;

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

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform interactableObj = targetsInViewRadius[i].transform;

            Vector3 dirToObj = (interactableObj.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirToObj) < viewAngle / 2)
            {
                float distanceToObj = Vector3.Distance(transform.position, interactableObj.position);

                if (!Physics.Raycast(transform.position, dirToObj, distanceToObj, obstacleMask))
                {
                    if (interactableObj.TryGetComponent(out IInteractable interactable))
                    {
                        interactable.iOCanvas().iOBillboardParentObj.SetActive(true);
                    }
                }
            }

            tIVR.Add(interactableObj);
        }

        for (int i = 0; i < targetsInViewRadius2.Length; i++)
        {
            Transform interactableObj = targetsInViewRadius2[i].transform;

            Debug.Log(interactableObj.gameObject.name);

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

        if (nearestObjTrans != null)
        {
            if (nearestObjTrans.TryGetComponent(out IInteractable interactable))
            {
                howToInteractGO.SetActive(true);
                howToInteractTxt.text = interactable.GetInteractUIText();

                if (Input.GetKey(KeyCode.E) && currClickedTime < timeTillInteract)
                {
                    currClickedTime += Time.deltaTime;
                    keyToPressFillImg.fillAmount += 1f / timeTillInteract * Time.deltaTime;

                    if (currClickedTime >= timeTillInteract)
                    {
                        interactable.Interact(nearestObjTrans);
                    }
                }

                if (Input.GetKeyUp(KeyCode.E))
                {
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
