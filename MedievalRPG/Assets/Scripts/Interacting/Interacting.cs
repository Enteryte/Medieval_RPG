using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interacting : MonoBehaviour
{
    public float viewRadius;
    public float viewRadius2;

    [Range(0, 360)] public float viewAngle;
    [Range(0, 360)] public float viewAngle2;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector] public float nearestDistance;
    [HideInInspector] public Transform nearestObjTrans;

    [HideInInspector] List<Transform> tIVR;

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
        for (int i = 0; i < tIVR.Count; i++)
        {
            if (tIVR[i].TryGetComponent(out IInteractable interactable))
            {
                interactable.iOCanvas().iOTextParentObj.SetActive(false);
                interactable.iOCanvas().iOBillboardParentObj.SetActive(false);
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
                interactable.iOCanvas().iOTextParentObj.SetActive(true);
                interactable.iOCanvas().howToInteractTxt.text = interactable.GetInteractUIText();

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact(nearestObjTrans);
                }
            }
        }

        // WIP: Hier fehlt noch eine for-Schleife ( Siehe anderes Projekt ) + Transform muss durch die Transform des interagierbaren Objektes ersetzt werden.
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
