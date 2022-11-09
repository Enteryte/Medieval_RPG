using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class NPCHeadTracking : MonoBehaviour
{
    public Transform normalTarget;
    public Rig headRig;

    public GameObject playerGO;

    public float radius = 2f;
    float radiusSqr;

    float maxAngle = 60f;

    // Start is called before the first frame update
    void Start()
    {
        //playerGO = GameManager.instance.playerGO;

        radiusSqr = radius * radius;
    }

    // Update is called once per frame
    void Update()
    {
        Transform tracking = null;

        Vector3 delta = playerGO.transform.position - transform.position;

        if (delta.sqrMagnitude < radiusSqr)
        {
            float angle = Vector3.Angle(transform.forward, delta);

            if (angle < maxAngle)
            {
                tracking = playerGO.transform;
            }
        }

        float rigWeight = 0;

        Vector3 targetPos = transform.position + (transform.forward * 2f);

        if (tracking != null)
        {
            targetPos = tracking.position;

            rigWeight = 1;
        }

        normalTarget.position = Vector3.Lerp(normalTarget.position, targetPos, Time.deltaTime * 3.5f);

        headRig.weight = Mathf.Lerp(headRig.weight, rigWeight, Time.deltaTime * 1);
    }
}
